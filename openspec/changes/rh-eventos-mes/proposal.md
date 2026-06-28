## Why

W8. Após folha mensal padrão (W6) e CCTs (W7), faltam os **eventos do mês** que não cabem na folha regular: férias, 13º salário (1ª e 2ª parcelas), adiantamentos, afastamentos (atestado, INSS, acidente), licenças (maternidade, paternidade, casamento, óbito). Cada um tem regras CLT próprias e gera **folha especial** (tipo diferente de Normal).

## What Changes

### Novas entidades

- `Ferias`
  - funcionario_id, tipo (`Direito`, `Concedida`, `Gozada`, `PagaAntecipadamente`, `Vendida`)
  - periodo_aquisitivo_inicio/fim (12 meses)
  - dias_direito (default 30), dias_pendentes
  - data_inicio_gozo, data_fim_gozo, dias_gozados
  - dias_vendidos (até 10), abono_pecuniario_valor
  - adiantamento_13o (BOOL) — quando funcionário pede
  - status (`Pendente`, `Programada`, `EmGozo`, `Concluida`, `Vencida`)
  - folha_id (FK quando paga)

- `Decimo3o`
  - funcionario_id, ano
  - tipo_parcela (`Primeira`, `Segunda`, `Antecipada`)
  - data_pagamento, valor_calculado
  - meses_trabalhados, base_calculo
  - folha_id

- `Adiantamento`
  - funcionario_id, competencia
  - valor, data_pagamento, motivo
  - descontado_em_folha BOOL, folha_id

- `Afastamento`
  - funcionario_id, tipo (`Atestado`, `InssCID`, `Acidente`, `MaternidadeINSS`, `PaternidadeEmpresa`, `Casamento`, `Obito`, `Servico Militar`, `Outro`)
  - data_inicio, data_fim (NULL = aberto)
  - cid, atestado_url
  - paga_durante (`Empresa`, `INSS`, `Misto`)
  - dias_pagos_empresa, dias_inss
  - aprovado_por

- `LicencaRemunerada` (subset de afastamento, casos especiais como gala, nojo, etc.)

### Tipos de folha — extensão do W6

`FolhaMensal.tipo` já é enum no W6. Esta onda adiciona uso de:
- `Adiantamento`
- `Decimo3oParcela1` (pago até 30/nov)
- `Decimo3oParcela2` (pago até 20/dez)
- `Ferias` (paga junto com início do gozo, em folha avulsa)

### Engines especializadas

Cada tipo de evento estende o engine do W6:

- `EngineFolhaFerias`: calcula 1 mês de salário + 1/3 constitucional + abono se vendeu + adiantamento 13º se pediu - descontos.
- `EngineFolha13o`:
  - 1ª parcela = 50% do salário vigente em novembro × meses_trabalhados/12, sem desconto.
  - 2ª parcela = (sal vigente em dezembro × meses_trab/12) - 1ª parcela - INSS - IRRF.
- `EngineFolhaAdiantamento`: % do salário (configurável, default 40%).

### Trigger automático

- Job mensal verifica funcionários com período aquisitivo completo → cria `Ferias.Pendente`.
- Job mensal em novembro/dezembro → cria 13º automaticamente para todos ativos.
- Atestado >15 dias → INSS assume (alerta automático).
- Licença maternidade → 120 dias INSS + extensão empresa-cidadã opcional.

### Endpoints

```
/api/v1/rh/ferias
  GET    /                                        listar pendentes/programadas/gozadas
  POST   /                                        cria férias programadas
  GET    /{id}
  PUT    /{id}                                    altera datas (antes do gozo)
  POST   /{id}/programar { dataInicio, diasGozar, vender? abono? adiantar13? }
  POST   /{id}/cancelar { motivo }
  POST   /{id}/marcar-gozado                      manual quando funcionário retorna
  GET    /funcionarios/{id}/saldo                 saldo de dias + período aquisitivo

/api/v1/rh/decimo-terceiro
  POST   /{ano}/processar-parcela-1                gera folha tipo Decimo3oParcela1 para todos
  POST   /{ano}/processar-parcela-2
  GET    /{ano}/funcionarios/{id}                  preview do valor

/api/v1/rh/adiantamentos
  POST   /                                          cria adiantamento manual
  GET    /                                          listar por competência
  POST   /folha-mes/{ymd}/gerar-adiantamentos       gera folha de adiantamentos do mês

/api/v1/rh/afastamentos
  POST   /                                          registra afastamento
  GET    /                                          listar
  PUT    /{id}/encerrar { dataFim }
  GET    /alertas-15-dias                            que vão pra INSS
```

### Permissions

- `Recursos.RhFerias`, `Recursos.RhDecimo3o`, `Recursos.RhAdiantamento`, `Recursos.RhAfastamento`
- `Acoes.Programar`, `Acoes.MarcarGozado`, `Acoes.ProcessarParcela`

### Frontend

- Tela "Férias" — lista de todos funcionários com status + saldo + botão "Programar"
- Wizard "Programar férias" — datas, vender? adiantar 13º? abono?
- Aviso de férias PDF (gerado no programar)
- Tela "13º" — preview por funcionário + processar lote
- Tela "Adiantamentos"
- Tela "Afastamentos" — CRUD com upload de atestado
- Calendário gerencial de afastamentos da empresa

## Capabilities

### New Capabilities
- `rh-eventos-mes` — Férias, 13º, adiantamentos, afastamentos com cálculos CLT específicos.

### Modified Capabilities
- `rh-folha` — novos `EngineFolhaFerias`, `EngineFolha13o`, `EngineFolhaAdiantamento` reusam `ContextoFuncionarioFolha`.

## Out of Scope
- Rescisão (W9).
- Tickets de afastamento integrados ao INSS (não há API pública estável).
- Notificação automática ao funcionário do início das férias (deferir push).

## Risks

- **R1**: Cálculo de 13º proporcional com fração de mês — regras de borda (admissão dia 14, demissão dia 16) precisam fixture clara.
- **R2**: Período aquisitivo de férias pode ter perda parcial por faltas (CLT art. 130). Mitigação: calcular dias_direito considerando faltas.
- **R3**: Afastamento aberto sem fim trava cálculos. Mitigação: alerta quando passa 15 dias (INSS assume).
- **R4**: Pagamento antecipado de 1/3 férias gera retenção IRRF diferenciada — fixture específica.

## Success Criteria

- 15 fixtures (férias normal, férias com venda 10 dias, férias com adto 13º, férias com abono, 13º proporcional 6 meses, 13º com aumento no meio do ano, afastamento empresa+INSS, etc.).
- Cada engine especializado bate ao centavo com cálculo de contador.
- Programação de férias gera aviso PDF + folha avulsa correta.
- `openspec validate rh-eventos-mes --strict` válido.
