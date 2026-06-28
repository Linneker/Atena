## Why

W9. Rescisão CLT é uma folha **especial e crítica**: calcula tudo que o funcionário tem direito ao sair (saldo salário, férias proporcionais + 1/3, 13º proporcional, multa 40% FGTS se for o caso, aviso prévio indenizado), gera o **TRCT** (Termo de Rescisão do Contrato de Trabalho) homologado, e dispara o evento eSocial **S-2299** (W13).

Sem rescisão não há ciclo CLT completo. Esta onda fecha o ciclo de vida do funcionário no sistema.

## What Changes

### Novas entidades

- `Rescisao`
  - funcionario_id, data_aviso, data_rescisao_efetiva
  - tipo (`SemJustaCausaEmpresa`, `PedidoDemissao`, `JustaCausaEmpresa`, `RescisaoIndireta`, `AcordoConsensual`, `Aposentadoria`, `Obito`, `TerminoContratoExperiencia`)
  - tipo_aviso_previo (`Trabalhado`, `Indenizado`, `Dispensado`)
  - dias_aviso_previo
  - tem_direito_multa_fgts (computed por tipo)
  - tem_direito_seguro_desemprego BOOL (computed)
  - causa_descricao TEXT (para justa causa)
  - homologacao_data, homologacao_local, homologador_nome
  - trct_url (PDF), folha_id, status (`Programada`, `Calculada`, `Homologada`, `Concluida`, `Cancelada`)

- `MotivoRescisaoCodigoEsocial` (catálogo eSocial S-2299)
  - codigo, descricao

### Engine especializado — `EngineFolhaRescisao`

```
Calcula:
  + Saldo de salário (dias_trabalhados_mes × salario_diario)
  + Aviso prévio indenizado (se aplicável; 30d + 3d por ano até 90d)
  + Férias vencidas (não gozadas) + 1/3
  + Férias proporcionais (meses_decorridos_no_periodo / 12) × salario + 1/3
  + 13º proporcional (meses_trabalhados_ano / 12) × salario
  - INSS sobre rubricas incidentes
  - IRRF sobre base
  - Multa rescisão (40% saldo FGTS, se SemJustaCausa)
  + Indenização adicional Lei 7.787 (10% — atualmente suspenso, manter cálculo conditional)
  - Adiantamentos pendentes a descontar
  - Quitação de banco horas (se saldo positivo: paga; se negativo: desconta)
```

Por tipo de rescisão:

| Tipo                          | Aviso Prévio | Multa 40% | Sal-Maternidade | 13º prop | Férias prop | Seguro Desemp. |
|------------------------------:|:-:|:-:|:-:|:-:|:-:|:-:|
| Sem Justa Causa Empresa       | sim | sim | sim | sim | sim | sim |
| Pedido Demissão               | sim (trabalhado) | não | sim | sim | sim | não |
| Justa Causa Empresa           | não | não | não | não | sim (vencidas só) | não |
| Rescisão Indireta             | sim | sim | sim | sim | sim | sim |
| Acordo Consensual             | sim/2 | sim/2 (20%) | sim | sim | sim | não |
| Aposentadoria                 | sim | sim (peculiar) | sim | sim | sim | – |
| Óbito                         | – | sim | – | sim | sim | – |
| Término Contrato Experiência  | não | não | sim | sim | sim | não |

### TRCT (Termo de Rescisão do Contrato de Trabalho)

Formulário oficial MTE (anexo da NR-127/2015, atualizado). Layout PDF com:
- Cabeçalho: identificação empregador + empregado + contrato.
- Tabela: rubricas (proventos e descontos).
- Total bruto, total descontos, líquido.
- Assinatura empregado + empresa + homologador (sindicato ou DRT).

### Workflow

```
[Programada] (RH cria com tipo + datas)
     │
     ▼
[Calculada] (engine roda)
     │
     ▼
[Homologada] (RH confirma após assinatura)
     │
     ▼
[Concluida] (pagamento efetuado, folha fechada, eSocial S-2299 transmitido em W13)
```

### Endpoints

```
POST   /api/v1/rh/rescisoes                       criar (tipo, data aviso, data rescisão)
POST   /api/v1/rh/rescisoes/{id}/calcular
GET    /api/v1/rh/rescisoes/{id}
GET    /api/v1/rh/rescisoes/{id}/trct.pdf
POST   /api/v1/rh/rescisoes/{id}/homologar { data, local, homologador }
POST   /api/v1/rh/rescisoes/{id}/cancelar
POST   /api/v1/rh/rescisoes/{id}/concluir
GET    /api/v1/rh/rescisoes/motivos-codigos-esocial
```

### Pós-rescisão

- `Funcionario.status = Desligado`, `Funcionario.dataDemissao = data_rescisao_efetiva`.
- `Usuario.status = Desativado` (mantém histórico, bloqueia login).
- Pendência S-2299 para W13.

### Permissions

- `Recursos.RhRescisao` + `Acoes.Calcular, Homologar, Cancelar, Concluir`.

## Capabilities

### New Capabilities
- `rh-rescisao` — Cálculo CLT de rescisão por tipo, TRCT, workflow Programada→Concluida.

### Modified Capabilities
- `rh-folha` — engine reaproveita helpers para férias prop, 13º prop.
- `rh-cadastros` — `Funcionario` desligado entra em estado terminal.

## Out of Scope
- Homologação online direto com sindicato/DRT (não há API).
- Cálculo de indenização por danos morais (juridíco, não folha).
- Acordo consensual via assinatura digital direto.

## Risks

- **R1**: Cada tipo de rescisão tem nuances jurisprudenciais. Mitigação: 8 fixtures por tipo + validação contábil.
- **R2**: Multa FGTS depende de saldo FGTS conhecido — informação que vem da Caixa, não está no Atena. Mitigação: campo manual `saldoFgtsConhecido` no momento da rescisão, com nota explicativa.
- **R3**: TRCT v.2 (vigente) vs v.1 (antiga) — layout pode mudar. Mitigação: versionar `LayoutTrctV2`.

## Success Criteria

- 8 fixtures (uma por tipo de rescisão) com valores conferidos por contador.
- TRCT PDF gerado em formato oficial vigente.
- Pendência eSocial S-2299 criada (consumida em W13).
- Funcionário desligado fica corretamente marcado e Usuario desativado.
- `openspec validate rh-rescisao --strict` válido.
