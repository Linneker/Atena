# Compras

## Propósito

Fluxo de procurement: usuário **Solicita** compra → comprador transforma em
**Pedido** ao fornecedor → mercadoria chega e é **Recebida** → entra no estoque
e gera ContaPagar.

## Entidades principais

| Entidade | Path | Highlights |
|----------|------|-----------|
| `SolicitacaoCompra` | `Domain/Entities/Compras/SolicitacaoCompra.cs` | Requisitante, justificativa, status (Pendente/Aprovada/Rejeitada/Convertida) |
| `SolicitacaoCompraItem` | `Domain/Entities/Compras/SolicitacaoCompraItem.cs` | Produto, qtd, observação |
| `PedidoCompra` | `Domain/Entities/Compras/PedidoCompra.cs` | Fornecedor, status (Aberto/Confirmado/Recebido/Cancelado), prazo, condições |
| `PedidoCompraItem` | `Domain/Entities/Compras/PedidoCompraItem.cs` | Produto, qtd, valor unitário, desconto |
| `RecebimentoCompra` | `Domain/Entities/Compras/RecebimentoCompra.cs` | Data, NF do fornecedor, itens recebidos (qtd parcial ou total) |

## Fluxo

```
Solicitação(Pendente)
        │  aprovar
        ▼
Solicitação(Aprovada)
        │  converter em pedido
        ▼
PedidoCompra(Aberto)
        │  enviar/confirmar fornecedor
        ▼
PedidoCompra(Confirmado)
        │  registrar recebimento (parcial ou total)
        ▼
PedidoCompra(Recebido)
        ├─→ EntradaProdutoEstoque (1 por item) — custo FIFO atualizado
        └─→ ContaPagar (status Aberta, vencimento conforme condição)
```

## Endpoints REST

| Método | Rota | Permissão |
|--------|------|-----------|
| GET/POST | `/api/v1/compras/solicitacoes` | `solicitacao-compra:*` |
| POST | `/api/v1/compras/solicitacoes/{id}/aprovar` | `solicitacao-compra:aprovar` |
| POST | `/api/v1/compras/solicitacoes/{id}/converter` | `pedido-compra:criar` |
| GET/POST | `/api/v1/compras/pedidos` | `pedido-compra:*` |
| POST | `/api/v1/compras/pedidos/{id}/confirmar` | `pedido-compra:editar` |
| POST | `/api/v1/compras/pedidos/{id}/cancelar` | `pedido-compra:cancelar` |
| POST | `/api/v1/compras/recebimentos` | `pedido-compra:editar` |

## Decisões

- Aprovação de Solicitação é **opcional** (configurável por tenant — flag
  `Compras:RequerAprovacao`).
- Recebimento parcial mantém pedido em status `Confirmado`; vira `Recebido`
  só quando 100% dos itens recebidos.
- ContaPagar é gerada no **recebimento**, não no pedido (princípio: passivo só
  surge quando mercadoria recebida).

## Frontend

- `site/atena-web/src/app/features/compras/` — solicitações, pedidos,
  recebimentos.

## Arquivos para consultar

- `src/Service/Acme.Sistemas.Domain/Entities/Compras/`
- `src/Service/Acme.Sistemas.Services/V1/Compras/`
- `src/Api/Acme.Sistemas.Atena.Api/Endpoints/V1/Compras/`
- `site/atena-web/src/app/features/compras/`

## Follow-ups conhecidos

- Cotação multi-fornecedor (RFQ).
- E-mail automático ao fornecedor na confirmação do pedido.
- Importação de XML de NF-e do fornecedor para pré-preencher recebimento.
