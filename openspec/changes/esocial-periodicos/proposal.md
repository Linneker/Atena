## Why

W14. **Eventos periódicos** do eSocial — fecham a competência mensal:

- **S-1200**: Remuneração do trabalhador (1 por funcionário com rubricas calculadas).
- **S-1210**: Pagamentos (data e valor pago do líquido).
- **S-1299**: Fechamento de Eventos Periódicos (encerra a competência).

Adicionalmente:
- **S-3000**: Exclusão de evento (qualquer tipo).
- Fluxo de retificação por NSR.

Esta onda fecha o ciclo: folha W6 + W7 + bridge W10 + eventos não-periódicos W13 → competência fecha no eSocial.

## What Changes

### Eventos

| Código | Nome | Trigger |
|--------|------|---------|
| **S-1200** | Remuneração | Para cada `HoleriteFuncionario` em `FolhaMensal.Fechada` |
| **S-1210** | Pagamentos | Para cada pagamento de líquido confirmado (W10 reverse sync) |
| **S-1299** | Fechamento | Manual após todos S-1200/S-1210 da competência Aceitos |
| **S-3000** | Exclusão | Cancelar evento por equívoco |

### Fluxo da competência

```
FolhaMensal.Fechada
  └── trigger automatico:
       └── para cada HoleriteFuncionario:
            ├── gera S-1200 (1 evento × funcionário × competência)
            └── envia via W11

ContaPagar de líquido marcada como Paga (W10 sync)
  └── trigger:
       └── gera S-1210 do pagamento

Após todos S-1200 + S-1210 Aceito:
  ├── RH revisa dashboard
  └── /esocial/periodicos/{competencia}/fechar
       └── gera S-1299
              ├── envia
              └── recebe Aceito → competência fechada no eSocial
```

### Retificação

```
S-1200 já enviado e Aceito, descobre-se erro:
  1. Cria NOVO S-1200 com indRetif=2 + nrRecibo do anterior
  2. Envia → Aceito = retificado
  3. Evento anterior fica marcado Retificado

S-3000 (exclusão):
  1. Cria S-3000 apontando para o evento a excluir
  2. Envia → evento original fica Excluido
```

Após S-1299 da competência, retificações exigem **reabertura** via S-3000 do S-1299 → re-envia eventos corrigidos → re-fecha com novo S-1299.

### Endpoints

```
POST /api/v1/esocial/periodicos/{competencia}/gerar-s1200          (lote)
POST /api/v1/esocial/periodicos/{competencia}/gerar-s1210          (lote, após pagamentos)
POST /api/v1/esocial/periodicos/{competencia}/fechar               (envia S-1299)
POST /api/v1/esocial/periodicos/{competencia}/reabrir              (envia S-3000 do S-1299)

GET  /api/v1/esocial/periodicos/{competencia}/status               (visão dashboard)
POST /api/v1/esocial/eventos/{id}/retificar { camposCorrigidos }
POST /api/v1/esocial/eventos/{id}/excluir { motivo }
```

### Permissions

- `Recursos.EsocialPeriodicos` × `Acoes.Gerar, Fechar, Reabrir, Retificar, Excluir`.

## Capabilities

### New Capabilities
- `esocial-periodicos` — Eventos periódicos S-1200/S-1210/S-1299, exclusão S-3000, fluxo de retificação.

### Modified Capabilities
- `rh-folha` — fechar folha dispara geração S-1200.
- `rh-financeiro-bridge` — pagamento confirma dispara S-1210.
- `esocial-transmissao` — pipeline genérico transmite estes eventos.

## Out of Scope
- Relatórios analíticos eSocial (W15).
- Importação de eventos eSocial gerados por terceiros.
- eSocial Doméstico (sistema separado da Receita).

## Risks

- **R1**: S-1200 enorme — muitas rubricas por funcionário, várias incidências, bases discriminadas. Mitigação: mapper builder testado contra fixtures complexas.
- **R2**: S-1299 só pode ser enviado quando TUDO da competência está Aceito. Mitigação: validação pré-envio + dashboard claro.
- **R3**: Retificação encadeia muitos eventos (re-S-1200 + S-1210 + novo S-1299). Mitigação: orquestrador.
- **R4**: Mudança de versão XSD a cada 1-2 anos. Mitigação: versionar layouts.

## Success Criteria

- Ciclo competência completo em Restrita: 5 funcionários × S-1200 + S-1210 + S-1299 → Aceito.
- Retificação de S-1200 funciona end-to-end.
- S-3000 exclui evento corretamente.
- Reabertura de competência funciona.
- `openspec validate esocial-periodicos --strict` válido.
