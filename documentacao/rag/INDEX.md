# Índice RAG — Atena

Knowledge base estruturada por funcionalidade. Cada arquivo é **auto-contido**
(retrieval-friendly): entidades, endpoints, handlers, decisões, paths.

> Atualizar o arquivo correspondente quando alterar a funcionalidade. Sem este
> índice, queries semânticas podem casar com o arquivo errado.

## Plataforma & Convenções

- [plataforma.md](plataforma.md) — Multi-tenancy, JWT, RBAC, ITenantContext, blueprint Acme, CQRS Mediator
- [auditoria-observabilidade.md](auditoria-observabilidade.md) — AuditLog, ApiRequestAudit, hash-chain do ponto, NLog
- [infraestrutura.md](infraestrutura.md) — Docker, Kubernetes (kind), MySQL, Redis, RabbitMQ, MinIO, hosted services

## Cadastros & ERP

- [cadastros.md](cadastros.md) — Empresa, Cliente, Fornecedor, Funcionário, Produto, ViaCEP
- [financeiro.md](financeiro.md) — Despesa, Receita, ContaPagar/Receber, FluxoDeCaixa, ConciliacaoBancaria, PlanoDeContas
- [estoque.md](estoque.md) — Estoque multi-empresa, EntradaProdutoEstoque, SaídaProduto, Inventário, custo FIFO
- [compras.md](compras.md) — SolicitacaoCompra → PedidoCompra → RecebimentoCompra
- [vendas.md](vendas.md) — Orcamento → PedidoVenda → Faturamento (NF-e) → DevolucaoVenda, ComissãoVendedor

## Fiscal

- [fiscal-nfe.md](fiscal-nfe.md) — NF-e v4.00, SEFAZ client próprio, contingência SVRS, ICP-Brasil A1, NumeradorNFe

## RH (programa-rh-folha-esocial)

- [rh-fundacao-w1.md](rh-fundacao-w1.md) — Jornada, Cargo, Lotação, Departamento, Benefício, Dependente, CBO
- [rh-ponto-interno-w2.md](rh-ponto-interno-w2.md) — MarcacaoPonto, AjustePonto, EspelhoMensal, BancoHoras, FechamentoPonto
- [rh-mobile-w3.md](rh-mobile-w3.md) — App MAUI Android/iOS/Win/Mac, offline queue, dispositivos, push stub
- [rh-ponto-oficial-671-w4.md](rh-ponto-oficial-671-w4.md) — Portaria 671: NSR, ComprovantePonto, AFD, AEJ

## UI & Cliente

- [frontend-angular.md](frontend-angular.md) — Angular 17 standalone, signals, AuthStore, branding, shared/data-table, shared/crud
- [mobile-maui.md](mobile-maui.md) — Apenas o app MAUI (cliente) — espelha rh-mobile-w3 do lado cliente

## Como usar este RAG

1. Query semântica → casa com 1 arquivo principal + talvez 1-2 vizinhos.
2. Cada arquivo lista **paths concretos** (`src/...`, `documentacao/...`) — siga
   eles para detalhes.
3. Atualize o arquivo da funcionalidade junto com a PR que muda código.
4. Adicione novos arquivos quando criar uma nova capability — sempre registre
   neste índice.

## Convenção de cada arquivo

```
# <Título da funcionalidade>

## Propósito
1-2 parágrafos: o que faz, por que existe, quando o usuário/dev encosta nisso.

## Entidades principais
- Lista de classes Domain com namespace + 1 linha de descrição.

## Endpoints REST
- Lista `VERB /api/v1/...` com 1 linha + permissão exigida.

## Services / Handlers chave
- Componentes Services + paths.

## Decisões / Convenções
- Decisões não-óbvias documentadas (com o "porquê").

## Arquivos para consultar
- Paths concretos `src/...` que materializam tudo.

## Follow-ups conhecidos
- TODOs e PRs nominais.
```
