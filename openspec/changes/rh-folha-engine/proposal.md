## Why

W6. **O coração do programa.** Com cadastros (W1), ponto (W2/W4), tabelas legais e rubricas (W5), agora calculamos a **folha mensal de pagamento** brasileira (CLT): pega apontamentos do mês + cadastros + tabelas + rubricas → produz holerite com proventos, descontos, líquido a pagar, e pendências fiscais (INSS, IRRF, FGTS).

## What Changes

### Backend — novas entidades

- `FolhaMensal` — cabeçalho da rodada de cálculo
  - tenant_id, empresa_id, competencia (YYYY-MM), tipo (`Normal`, `Adiantamento`, `Decimo3oParcela1`, `Decimo3oParcela2`, `Ferias`, `Rescisao`)
  - status (`Aberta`, `EmCalculo`, `Calculada`, `Conferida`, `Fechada`, `Reaberta`)
  - data_pagamento_prevista, data_pagamento_efetiva
  - totalizadores (totalProventos, totalDescontos, totalLiquido, totalInssDescontado, totalInssPatronal, totalIrrf, totalFgts)
  - fechada_em, fechada_por

- `HoleriteFuncionario` — uma linha por funcionário na rodada
  - folha_id, funcionario_id
  - rubricas_calculadas_json (array de { rubricaCodigo, descricao, tipo, valor, baseCalculo, observacao })
  - totalProventos, totalDescontos, valorLiquido
  - bases (baseInss, baseIrrf, baseFgts, baseDsr)
  - guias_geradas_json (referência ao GPS/DARF/GRF quando W10 rodar)
  - pdf_url (S3)

- `CalculoFolhaErro`
  - folha_id, funcionario_id, erro (string), stacktrace
  - resolvido BOOLEAN, resolvido_por, resolvido_em

- `ParametrosCalculoFolha` (por tenant/empresa)
  - aceita_he_negativa BOOLEAN
  - paga_dsr_sobre_he BOOLEAN
  - arredondamento_centavos ENUM
  - regra_vt (default 6%)
  - outros parâmetros opcionais

### Engine de cálculo (núcleo do change)

`Acme.Sistemas.Services.V1.Rh.Folha.Engine/`:

```
EngineFolhaMensal
  ├── PreparaContextoFuncionario(funcionarioId, competencia)
  │     ├── lê salário vigente
  │     ├── lê dependentes IRRF/SF
  │     ├── lê benefícios vigentes
  │     ├── lê apontamentos (W2): horas trabalhadas/extras/faltas/atestados
  │     ├── lê eventos do mês (W8 não existe ainda — placeholder)
  │     └── lê CCT vigente (W7 não existe ainda — placeholder)
  │
  ├── ResolveRubricasDeOficio(ctx)
  │     ├── 1XX Salário base → calcula a partir de horas e salário vigente
  │     ├── 2XX Horas extras (50%, 100%, noturna)
  │     ├── 3XX Adicional noturno
  │     ├── 4XX Adicional periculosidade/insalubridade
  │     ├── 5XX DSR sobre HE (se parametrizado)
  │     ├── 6XX Salário-família (se dependentes elegíveis)
  │     ├── 7XX Outros proventos (lista benefícios + custom rubricas tenant)
  │     ├── 8XX VT recebido (informativo)
  │     │
  │     ├── 9XX Descontos:
  │     │   ├── INSS (aplicaTabelaInss sobre baseInss)
  │     │   ├── IRRF (aplicaTabelaIrrf)
  │     │   ├── VT desconto (min(6% sal-base, custoVT))
  │     │   ├── Outros descontos (custom rubricas tenant)
  │     │   └── Adiantamento já pago (se houver)
  │     │
  │     └── INFO: FGTS (8% sobre baseFgts — não desconta, informa)
  │
  ├── AplicaRubricasCustomTenant(ctx)
  │     └── interpreta DSL via RubricaExpressionEvaluator (W5) em ordem topológica
  │
  ├── ConsolidaTotais(ctx)
  │     └── calcula totalProventos, totalDescontos, valorLiquido
  │
  ├── GeraHolerite(ctx)
  │     ├── persiste HoleriteFuncionario
  │     └── enfileira PDF via worker
  │
  └── RegistraPendenciasFiscais
        └── (para W10 consumir: GPS, DARF, GRF)
```

### Endpoints

- `POST /api/v1/rh/folha/{competencia}/abrir { empresaId, tipo, dataPagamento }` (cria FolhaMensal)
- `POST /api/v1/rh/folha/{id}/calcular` (assíncrono — dispara worker)
- `GET /api/v1/rh/folha/{id}/status`
- `GET /api/v1/rh/folha/{id}/holerites?funcionarioId=&status=`
- `GET /api/v1/rh/folha/{id}/holerites/{funcionarioId}` (detalhe)
- `GET /api/v1/rh/folha/{id}/holerites/{funcionarioId}.pdf`
- `POST /api/v1/rh/folha/{id}/holerites/{funcionarioId}/recalcular`
- `POST /api/v1/rh/folha/{id}/conferir` (status → Conferida)
- `POST /api/v1/rh/folha/{id}/fechar` (status → Fechada)
- `POST /api/v1/rh/folha/{id}/reabrir` (admin)
- `GET /api/v1/rh/folha/{id}/totais` (sumário)
- `GET /api/v1/rh/folha/{id}/divergencias` (erros)

### Worker

`CalculoFolhaWorker` (RabbitMQ):
- Recebe mensagem `CalcularFolhaMessage { folhaId, funcionarioId }`.
- Roda engine para 1 funcionário.
- Atualiza holerite.
- Em caso de erro, persiste `CalculoFolhaErro` (não derruba o lote inteiro).

Abrir folha enfileira 1 mensagem por funcionário ativo da empresa → processamento paralelo (subject ao throttle do RabbitMQ).

### Holerite PDF

Lib: QuestPDF (já adotada em W2).

Layout padrão CLT:
```
┌────────────────────────────────────────────────────────────┐
│ RAZÃO SOCIAL          CNPJ      COMPETÊNCIA  MES/ANO       │
│ Endereço                                                   │
├────────────────────────────────────────────────────────────┤
│ NOME DO FUNCIONÁRIO       CPF      MATRÍCULA   CARGO       │
│ Departamento  Lotação  Admissão                            │
├────────────────────────────────────────────────────────────┤
│ COD  DESCRIÇÃO                          PROVENTO  DESCONTO │
│ 001  Salário Base                      3.000,00            │
│ 020  Horas Extras 50%                    250,00            │
│ 100  INSS                                            300,00│
│ 110  IRRF                                             45,00│
│ ...                                                         │
├────────────────────────────────────────────────────────────┤
│ TOTAIS                                  3.250,00     345,00│
│ LÍQUIDO A RECEBER                                  2.905,00│
├────────────────────────────────────────────────────────────┤
│ Bases: INSS 3.250  IRRF 2.950  FGTS 3.000                  │
│ FGTS do mês (depositado pela empresa): 240,00              │
├────────────────────────────────────────────────────────────┤
│ Banco/Agência/Conta:  XXX / YYYY / ZZZZZZ                  │
│ Data pagamento: DD/MM/AAAA                                 │
└────────────────────────────────────────────────────────────┘
```

### Estados e transições

```
[Aberta] ──calcular──► [EmCalculo] ──fim──► [Calculada]
                                                  │
                                              conferir
                                                  ▼
                                            [Conferida]
                                                  │
                                                fechar
                                                  ▼
                                             [Fechada]
                                                  │
                                              reabrir (admin)
                                                  ▼
                                            [Reaberta]
                                                  │
                                              calcular
                                                  ▼
                                           [EmCalculo] ...
```

Estado `Fechada` trava edição definitiva (apenas reabertura por admin com audit).

### Cálculos brasileiros — visão (não exaustiva)

**INSS empregado** (vigência 2026):
```
faixas:
  até 1518.00          → 7.5%
  1518.01-2793.88      → 9.0%
  2793.89-4190.83      → 12.0%
  4190.84-8157.41      → 14.0% (teto)
  acima do teto        → fixo no teto
```
Aplicado escalonado (não em cascata).

**IRRF** (vigência 2026):
```
base = remBruta - INSS - (dependentes * 189.59) - pensãoAlim
faixas anuais (mensais):
  até 2259.20          → isento
  2259.21-2826.65      → 7.5% - 169.44
  2826.66-3751.05      → 15% - 381.44
  3751.06-4664.68      → 22.5% - 662.77
  acima de 4664.68     → 27.5% - 896.00
```

**FGTS**:
```
8% sobre proventos com incidência (sal-base + HE + adicionais).
Não desconta do funcionário — info.
```

**Adicional noturno** (CLT art. 73):
```
20% sobre horas trabalhadas entre 22h e 5h.
Hora noturna reduzida: 52min30s (informativo).
```

**HE** padrão CLT (a CCT pode mudar — W7):
```
50% sobre horas além da jornada (até 2h/dia ou conforme acordo).
100% sobre domingos, feriados, e além das 2h diárias.
```

**Salário-família** (vigência 2026):
```
limite_remuneracao: depende de tabela vigente
valor_por_dependente: depende de tabela vigente
```

**DSR sobre HE**:
```
se parametrizado true:
  dsr = (totalHe / diasUteisMes) * diasNaoUteisMes
```

**VT desconto** (regra fixa):
```
desconto = min(salBase * 0.06, custoTotalVT)
```

## Capabilities

### New Capabilities
- `rh-folha` — Motor de cálculo de folha mensal CLT.

### Modified Capabilities
- `rh-tabelas-legais` — engine consome via `aplicaTabelaInss`, `aplicaTabelaIrrf`.

## Out of Scope

- 13º, férias, rescisão (W8/W9).
- Bridge para Financeiro (W10).
- eSocial S-1200/1210 (W14).
- CCTs (W7 — engine deve ser extensível, mas sem CCT roda na base CLT).
- Cálculo retroativo de meses fechados (recálculo só por reabertura).

## Risks

- **R1**: Cálculo errado é catastrófico — funcionário recebe a menos ou empresa paga a mais. Mitigação: bateria de 30+ fixtures conferidos com contador externo.
- **R2**: DSL de rubrica custom pode acessar dados sensíveis ou cair em loop. Mitigação: W5 já isolou via sandbox + timeout.
- **R3**: Performance: cálculo de 1000 funcionários síncrono = inviável. Mitigação: worker paralelo via RabbitMQ.
- **R4**: Mudanças retroativas (admin corrige salário do mês passado) bagunçam folha já fechada. Mitigação: reabertura explícita + warning + audit.

## Success Criteria

- Folha mensal de funcionário CLT padrão (sem complicações) bate 100% com cálculo manual feito por contador externo.
- 30 fixtures (CLT puro, com HE, com noturno, com peric/insalub, com dependentes, com VT, com adiantamento, sem férias, com banco horas, com benefícios diversos, com rubrica custom tenant) batem com expected.
- Folha de empresa com 100 funcionários processa em < 3 minutos (worker paralelo).
- Holerite PDF determinístico (mesmos inputs → mesmo PDF byte-identico).
- Erros isolados não derrubam o lote inteiro.
- `openspec validate rh-folha-engine --strict` válido.
