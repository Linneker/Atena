## Why

W7. CCT (Convenção Coletiva de Trabalho) e ACT (Acordo Coletivo de Trabalho) **modificam a folha CLT base** com regras categoria-específicas: piso salarial, percentual de HE diferenciado, anuênio/biênio/quinquênio, gatilhos de auxílio-alimentação, multas em rescisão, etc. Decisão Q3 = **estrutura formal com aplicação automática de regras**.

Esta onda introduz:
- Modelo de Convenção (CCT/ACT) com vigência.
- Catálogo de "regras" estruturadas: piso por cargo, percentual de HE, periculosidade incremental, anuênio, etc.
- Vínculo Empresa × Convenção (a empresa adere a 1 ou mais).
- Vínculo Funcionario × Convenção (override ou por categoria).
- Aplicação automática no engine de folha (W6) e em férias/13º/rescisão (W8/W9).

## What Changes

### Backend — novas entidades

- `Convencao`
  - tenant_id, codigo (único por tenant, ex: "METAL-SP-2026"), descricao
  - categoria_profissional, sindicato_nome, sindicato_cnpj
  - tipo (`Convencao`, `Acordo`)
  - vigencia_inicio, vigencia_fim
  - documento_url (PDF original)
  - dataBase (mês do dissídio, para histórico)
  - status (`Ativa`, `Suspensa`, `Expirada`)

- `RegraConvencao` — entidade genérica polimórfica
  - convencao_id
  - tipo (enum, ver abaixo)
  - parametros_json (varia por tipo)
  - ordem (precedência quando há múltiplas)
  - condicao_dsl (opcional — DSL booleana para aplicar condicionalmente)

- `AdesaoConvencao` — vínculo empresa×convencao
  - empresa_id, convencao_id, vigencia_inicio, vigencia_fim
  - observacoes

- `OverrideConvencaoFuncionario` — vínculo funcionario×convencao
  - funcionario_id, convencao_id, vigencia_inicio, vigencia_fim
  - motivo (texto)

- `ResolucaoConvencao` — cache de "qual convenção vigente para funcionário X em competência Y" (atualizado por trigger ou job)

### Tipos de regra (enum + schema do parametros_json)

```
RegraConvencao.tipo:
  PisoSalarial            { cargoIds: [], salarioMinimo: decimal }
  PisoSalarialPorCbo      { codigosCbo: [], salarioMinimo: decimal }
  AdicionalHeDiurnoPct    { pct: 50|60|70|100, comDsr: bool }
  AdicionalHeNoturnoPct   { pct: ..., horaReduzida: bool }
  AdicionalNoturnoPct     { pct: 20|25|30, janelaInicio, janelaFim }
  PericulosidadePct       { pct: 30, baseCalcula: enum(SalBase|SalBruto) }
  InsalubridadeGrau       { grauMin, grauMed, grauMax, baseCalcula }
  AnueniePct              { pctPorAno: 1, tetoAnos: 20 }
  AdicionalTempoServico   { faixas: [{ anos: 5, pct: 5 }, ...] }
  ValeAlimentacao         { valorMinimo, descontoFuncionarioPct }
  AuxilioCreche           { idadeMax, valorMinimo, criteriosElegibilidade }
  MultaRescisaoSemJustaCausa { multaFgtsPct: 40, multaSocial: 10 }
  AvisoPrevioDias         { porAnoServico: 3, maximo: 90 }
  GatilhoReajuste         { mesReajuste, percentualMinimoOuIndice }
  RegraCustomDsl          { dsl: "..." } -- escape hatch
```

### Resolução de qual convenção aplicar

```
Para funcionario F em competência C:
  1. tem override_convencao_funcionario vigente? → aplica essa
  2. senão, empresa do funcionário tem adesão vigente? → aplica essa
  3. senão, sem convenção (CLT base pura)
  4. se múltiplas vigentes, aplica todas em ordem; última prevalece
```

### Aplicação automática

W6 (engine de folha) carrega `Convencao` resolvida e passa para `ContextoFuncionarioFolha`:

```csharp
// no engine W6
var cct = await ResolverConvencaoAsync(funcId, comp);
ctx.PctHeDiurno = cct?.Regras.OfType<AdicionalHeDiurnoPct>().FirstOrDefault()?.Pct ?? 50m;
ctx.PctNoturno = cct?.Regras.OfType<AdicionalNoturnoPct>().FirstOrDefault()?.Pct ?? 20m;
ctx.PisoSalarial = cct?.ResolvePiso(funcionario.CargoId);
// etc.

// se cct tem RegraCustomDsl, adiciona ao processamento de rubricas custom
```

W8 (eventos) e W9 (rescisão) idem.

### Endpoints

```
GET    /api/v1/rh/convencoes
POST   /api/v1/rh/convencoes
GET    /api/v1/rh/convencoes/{id}
PUT    /api/v1/rh/convencoes/{id}
DELETE /api/v1/rh/convencoes/{id}                 (soft delete se já usada)

POST   /api/v1/rh/convencoes/{id}/regras           (adiciona regra)
PUT    /api/v1/rh/convencoes/{id}/regras/{regraId}
DELETE /api/v1/rh/convencoes/{id}/regras/{regraId}

POST   /api/v1/rh/convencoes/{id}/empresas/{empresaId}   (aderir)
DELETE /api/v1/rh/convencoes/{id}/empresas/{empresaId}

POST   /api/v1/rh/funcionarios/{id}/convencao-override
DELETE /api/v1/rh/funcionarios/{id}/convencao-override

POST   /api/v1/rh/convencoes/{id}/simular-impacto
   Body: { funcionarioIds: [], competencia: "..." }
   Retorna diff esperado de folha aplicando vs sem aplicar.
```

### Frontend

- Tela CRUD de Convenções
- Construtor visual de regra: dropdown de tipo + form específico
- Tela "Adesão de empresa a convenção"
- Tela "Override por funcionário"
- Simulador de impacto (RH testa antes de ativar)
- Visualização de diff de holerite (antes vs depois da CCT)

### Importação

Import via arquivo:
- Tela de upload de "padrão CCT" (JSON estruturado) — permite tenants partilharem CCTs comuns.
- Catálogo nacional opcional (em `documentacao/rh/cct/` JSONs de CCTs públicas conhecidas — só metadado e regras, sem documento legal).

## Capabilities

### New Capabilities
- `rh-cct` — Convenções coletivas estruturadas com aplicação automática no cálculo de folha, eventos e rescisão.

### Modified Capabilities
- `rh-folha` — engine aceita `Convencao` no contexto, aplica overrides de percentual.
- `rh-eventos-mes` (futuro W8) — usa CCT para regras de férias/13º.
- `rh-rescisao` (futuro W9) — usa CCT para aviso prévio, multas.

## Out of Scope
- Negociação coletiva online (sindicatos chat).
- Vinculação ao banco de dados oficial do MTE de convenções homologadas (depende de API externa).
- Aplicação retroativa a folhas já fechadas (precisa reabertura — W6).

## Risks

- **R1**: Variabilidade extrema de CCTs (sindicatos têm regras criativas). Mitigação: tipos + `RegraCustomDsl` como escape hatch.
- **R2**: Conflito entre múltiplas convenções vigentes. Mitigação: ordem + precedência + warning explícito.
- **R3**: Simulação de impacto pode ser lenta. Mitigação: cache + endpoint assíncrono opcional.
- **R4**: CCT muda salário base via piso → todas as folhas vigentes ficam desatualizadas. Mitigação: detectar mudança de CCT vigente e marcar folhas Calculadas como "precisam recalcular".

## Success Criteria

- 10 fixtures de CCT (metalúrgicos SP, comerciários RJ, professores MG, motoristas BA, etc.) modeladas em estrutura.
- Engine W6 aplica CCT corretamente em 10 cenários de holerite (HE 60%, anuênio 5%, piso, etc.) com expected validado.
- Simulação de impacto produz diff legível antes de RH ativar.
- `openspec validate rh-cct-engine --strict` válido.
