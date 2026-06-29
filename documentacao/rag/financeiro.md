# Financeiro

## Propósito

Gestão financeira: Despesas e Receitas, geração de Contas a Pagar/Receber,
Fluxo de Caixa, Conciliação Bancária, Plano de Contas e Centros de Custo.
Integra com Compras (PedidoCompra gera ContaPagar) e Vendas (Faturamento gera
ContaReceber).

## Entidades principais

| Entidade | Path | Highlights |
|----------|------|-----------|
| `Despesa` | `Domain/Entities/Financeiro/Despesa.cs` | Fornecedor, valor, data, status, plano de contas, centro de custo, recorrência |
| `Receita` | `Domain/Entities/Financeiro/Receita.cs` | Cliente, valor, data, status, plano de contas |
| `ContaPagar` | `Domain/Entities/Financeiro/ContaPagar.cs` | Vinculada a Despesa, vencimento, status (Aberta/Paga/Atrasada/Cancelada), forma pagamento |
| `ContaReceber` | `Domain/Entities/Financeiro/ContaReceber.cs` | Vinculada a Receita/Faturamento, vencimento, status |
| `Pagamento` | `Domain/Entities/Financeiro/Pagamento.cs` | Baixa parcial ou total, conta bancária, data, valor |
| `FluxoDeCaixa` | `Domain/Entities/Financeiro/FluxoDeCaixa.cs` | View materializada por período |
| `ConciliacaoBancaria` | `Domain/Entities/Financeiro/ConciliacaoBancaria.cs` | OFX/CSV importado vs `Pagamento` existente |
| `PlanoDeContas` | `Domain/Entities/Financeiro/PlanoDeContas.cs` | Hierarquia 4 níveis (Receita, Despesa, Ativo, Passivo) |
| `CentroDeCusto` | `Domain/Entities/Financeiro/CentroDeCusto.cs` | Apropriação por departamento/projeto |
| `Divida` | `Domain/Entities/Financeiro/Divida.cs` | Empréstimos com parcelas |

## Conciliação Bancária

`ConciliacaoMatcher` em `Services/V1/ConciliacaoBancaria/Services/` faz matching
heurístico entre extratos OFX/CSV e `Pagamento`s pendentes (data ±3 dias, valor
exato ou parcial, descrição fuzzy). Score por candidato; usuário confirma.

## Recorrência

- `Despesa.RecorrenciaConfig` (JSON: frequência, dia, parcelas, fim).
- `RecorrenciaFinanceiraWorker` (diário) gera próximas `ContaPagar` /
  `ContaReceber` a partir da config.

## Endpoints REST

| Método | Rota | Permissão |
|--------|------|-----------|
| GET/POST/PUT/DEL | `/api/v1/financeiro/despesas` | `despesa:*` |
| GET/POST/PUT/DEL | `/api/v1/financeiro/receitas` | `receita:*` |
| GET/POST | `/api/v1/financeiro/contas-pagar` | `conta-pagar:*` |
| POST | `/api/v1/financeiro/contas-pagar/{id}/baixar` | `conta-pagar:editar` |
| GET/POST | `/api/v1/financeiro/contas-receber` | `conta-receber:*` |
| POST | `/api/v1/financeiro/contas-receber/{id}/baixar` | `conta-receber:editar` |
| GET | `/api/v1/financeiro/fluxo-de-caixa` | `fluxo-de-caixa:ler` |
| POST | `/api/v1/financeiro/conciliacao/importar-ofx` | `conciliacao-bancaria:criar` |
| POST | `/api/v1/financeiro/conciliacao/{id}/confirmar` | `conciliacao-bancaria:aprovar` |
| GET/POST/PUT/DEL | `/api/v1/financeiro/plano-de-contas` | `plano-de-contas:*` |
| GET/POST/PUT/DEL | `/api/v1/financeiro/centros-de-custo` | `centro-de-custo:*` |

## Decisões

- `Despesa` e `Receita` são **fatos** (sempre existem após registrados).
  `ContaPagar`/`Receber` são **promessas** (podem ser canceladas).
- Status segue máquina simples: `Aberta → Paga` ou `Aberta → Cancelada`. Sem
  reabertura — gera nova conta se necessário.
- Conciliação é **assistida** (matcher sugere, humano confirma); nunca
  automática para evitar batidas erradas.

## Frontend

- `site/atena-web/src/app/features/financeiro/` com sub-rotas: despesas,
  contas-pagar, contas-receber, fluxo-de-caixa, conciliacao, plano-contas.
- `financeiro.routes.ts` + `financeiro.services.ts`.

## Arquivos para consultar

- `src/Service/Acme.Sistemas.Domain/Entities/Financeiro/`
- `src/Service/Acme.Sistemas.Services/V1/{Despesa,Receita,ContaPagar,ContaReceber,FluxoDeCaixa,ConciliacaoBancaria,PlanoDeContas,CentroDeCusto}/`
- `src/Service/Acme.Sistemas.Services/V1/ConciliacaoBancaria/Services/ConciliacaoMatcher.cs`
- `src/Api/Acme.Sistemas.Atena.Api/Endpoints/V1/Financeiro/`
- `src/Api/Acme.Sistemas.Atena.Api/Hosted/RecorrenciaFinanceiraWorker.cs`
- `site/atena-web/src/app/features/financeiro/`

## Follow-ups conhecidos

- DRE automática (Demonstração de Resultado).
- Integração com Open Banking (extrato direto).
- Multi-moeda.
