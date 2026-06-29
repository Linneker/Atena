# Vendas

## Propósito

Funil comercial: Orçamento → Pedido de Venda → Faturamento (que emite NF-e
quando aplicável) → opcional Devolução. Cálculo de Comissão por vendedor.

## Entidades principais

| Entidade | Path | Highlights |
|----------|------|-----------|
| `Orcamento` | `Domain/Entities/Vendas/Orcamento.cs` | Cliente, validade, status (Aberto/Aprovado/Expirado/Convertido) |
| `OrcamentoItem` | `Domain/Entities/Vendas/OrcamentoItem.cs` | Produto, qtd, unitário, desconto |
| `PedidoVenda` | `Domain/Entities/Vendas/PedidoVenda.cs` | Cliente, vendedor, status (Aberto/Confirmado/Faturado/Cancelado), forma pagamento |
| `PedidoVendaItem` | `Domain/Entities/Vendas/PedidoVendaItem.cs` | Produto, qtd, unitário, desconto, comissão% |
| `Faturamento` | `Domain/Entities/Vendas/Faturamento.cs` | Vinculado a PedidoVenda, gera NF-e + SaidaProdutoEstoque + ContaReceber |
| `DevolucaoVenda` | `Domain/Entities/Vendas/DevolucaoVenda.cs` | NF de devolução, retorna ao estoque, gera ContaPagar (ressarcimento) |
| `ComissaoVendedor` | `Domain/Entities/Vendas/ComissaoVendedor.cs` | Apuração por vendedor + período |

## Fluxo

```
Orcamento(Aberto)
        │  cliente aprova
        ▼
Orcamento(Aprovado)
        │  converter em pedido
        ▼
PedidoVenda(Aberto)
        │  confirmar
        ▼
PedidoVenda(Confirmado)
        │  faturar
        ▼
Faturamento  ─┬─→ NF-e (assíncrona via RabbitMQ + SEFAZ; fiscal-nfe.md)
              ├─→ SaidaProdutoEstoque (FIFO) — estoque baixa
              ├─→ ContaReceber (status Aberta)
              └─→ ComissaoVendedor (linha pendente)
PedidoVenda(Faturado)
```

## Comissão

`Services/V1/Vendas/ComissaoCalculator.cs` (a confirmar) aplica:
- % do item × valor da venda
- Override por vendedor (regra negociada por cliente/produto)
- Status: Pendente → Paga (após Faturamento confirmado + pago)

## Endpoints REST

| Método | Rota | Permissão |
|--------|------|-----------|
| GET/POST/PUT | `/api/v1/vendas/orcamentos` | `orcamento:*` |
| POST | `/api/v1/vendas/orcamentos/{id}/aprovar` | `orcamento:aprovar` |
| POST | `/api/v1/vendas/orcamentos/{id}/converter` | `pedido-venda:criar` |
| GET/POST/PUT | `/api/v1/vendas/pedidos` | `pedido-venda:*` |
| POST | `/api/v1/vendas/pedidos/{id}/confirmar` | `pedido-venda:editar` |
| POST | `/api/v1/vendas/pedidos/{id}/faturar` | `pedido-venda:faturar` |
| POST | `/api/v1/vendas/devolucoes` | `pedido-venda:cancelar` |
| GET | `/api/v1/vendas/comissoes` | `vendedor:ler` |

## Decisões

- Orçamento expira automaticamente (job ou check on-read) após
  `data_validade`.
- Faturamento é **atômico**: NF-e enfileirada + estoque baixado + ContaReceber
  criada na mesma transação. Se a SEFAZ rejeitar a NF-e depois, o faturamento
  fica em `Rejeitado` e o estoque é estornado.
- Cancelamento de PedidoVenda só permitido enquanto `Aberto` ou `Confirmado`.
  Faturado precisa de Devolução.

## Frontend

- `site/atena-web/src/app/features/vendas/` — orçamentos, pedidos,
  faturamentos, devoluções, comissões.

## Arquivos para consultar

- `src/Service/Acme.Sistemas.Domain/Entities/Vendas/`
- `src/Service/Acme.Sistemas.Services/V1/Vendas/`
- `src/Api/Acme.Sistemas.Atena.Api/Endpoints/V1/Vendas/`
- `site/atena-web/src/app/features/vendas/`
- Ver `fiscal-nfe.md` para emissão da NF-e.

## Follow-ups conhecidos

- CRM-lite: pipeline visual de orçamentos por estágio.
- Metas por vendedor + dashboard.
- Pricing rules (descontos automáticos por volume).
