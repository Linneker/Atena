# Estoque

## Propósito

Gestão de estoque multi-empresa: posições por produto, entradas (compras +
ajustes), saídas (vendas + perdas + transferências), inventário cíclico/total,
custo médio via **FIFO** (First In, First Out).

## Entidades principais

| Entidade | Path | Highlights |
|----------|------|-----------|
| `Estoque` | `Domain/Entities/Estoque/Estoque.cs` | Almoxarifado por empresa (matriz, filial, depósito) |
| `EstoqueProduto` | `Domain/Entities/Estoque/EstoqueProduto.cs` | Saldo atual por (estoque, produto) — denormalizado |
| `EntradaProdutoEstoque` | `Domain/Entities/Estoque/EntradaProdutoEstoque.cs` | Entrada via compra, devolução, ajuste; carrega custo unitário |
| `SaidaProdutoEstoque` | `Domain/Entities/Estoque/SaidaProdutoEstoque.cs` | Saída via venda, perda, transferência; consome FIFO |
| `Inventario` | `Domain/Entities/Estoque/Inventario.cs` | Contagem física vs sistema; ajustes resultantes |

## FIFO — `FifoCustoCalculator`

Path: `Services/V1/Estoque/Services/FifoCustoCalculator.cs`.

- Cada `EntradaProdutoEstoque` é uma **camada** com `quantidade` + `custoUnitario` + `dataEntrada`.
- `SaidaProdutoEstoque` consome camadas em ordem de entrada (mais antiga primeiro).
- Quando uma camada se esgota, próxima é consumida; quando não há saldo
  suficiente, retorna `EstoqueInsuficienteException` (ou status SaldoNegativo
  se config permite).
- Custo médio ponderado da saída é calculado e gravado em `SaidaProdutoEstoque.CustoMedio`.

## Endpoints REST

| Método | Rota | Permissão |
|--------|------|-----------|
| GET/POST/PUT/DEL | `/api/v1/estoque` | `estoque:*` |
| GET | `/api/v1/estoque/{id}/saldo` | `estoque:ler` |
| POST | `/api/v1/estoque/entradas` | `estoque:editar` |
| POST | `/api/v1/estoque/saidas` | `estoque:editar` |
| POST | `/api/v1/estoque/transferencias` | `estoque:editar` |
| GET/POST | `/api/v1/estoque/inventarios` | `inventario:*` |
| POST | `/api/v1/estoque/inventarios/{id}/finalizar` | `inventario:aprovar` |

## Integrações

- **Compras**: `RecebimentoCompra` finalizado → cria `EntradaProdutoEstoque`.
- **Vendas**: `Faturamento` confirmado → cria `SaidaProdutoEstoque`.
- **Devolução de venda**: cria `EntradaProdutoEstoque` com flag `Devolucao`.

## Frontend

- `site/atena-web/src/app/features/estoque/` com telas: estoques (almoxarifados),
  saldos, entradas, saídas, inventários.

## Decisões

- `EstoqueProduto` é **cache denormalizado** atualizado em transação junto da
  entrada/saída. Reconciliação periódica não implementada (fica TODO).
- Custo FIFO é cravado na saída — preço corrente do produto NÃO afeta saídas
  passadas.
- Transferência entre estoques é par `Saída(origem) + Entrada(destino)` com
  mesmo `loteId` para auditoria.

## Arquivos para consultar

- `src/Service/Acme.Sistemas.Domain/Entities/Estoque/`
- `src/Service/Acme.Sistemas.Services/V1/Estoque/`
- `src/Service/Acme.Sistemas.Services/V1/Estoque/Services/FifoCustoCalculator.cs`
- `src/Api/Acme.Sistemas.Atena.Api/Endpoints/V1/Estoque/`
- `site/atena-web/src/app/features/estoque/`

## Follow-ups conhecidos

- LIFO e custo médio móvel como opções.
- Lotes/séries (lote produto perecível, número de série bens).
- Reserva (reservar quantidade no pedido, baixar no faturamento).
