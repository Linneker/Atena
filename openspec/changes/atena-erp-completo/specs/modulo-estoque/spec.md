## ADDED Requirements

### Requirement: Saldo de Estoque por Produto
O sistema SHALL manter saldo atualizado em tempo real por produto e localização (depósito/almoxarifado).

#### Scenario: Consulta de saldo
- **WHEN** o usuário consulta o saldo de um produto
- **THEN** o sistema exibe quantidade disponível, reservada (pedidos em aberto) e total

#### Scenario: Saldo negativo bloqueado
- **WHEN** uma saída de estoque causaria saldo negativo
- **THEN** o sistema bloqueia a operação e exibe mensagem com saldo atual disponível
- **THEN** o bloqueio pode ser desativado por configuração do tenant (permitir estoque negativo)

### Requirement: Entrada de Estoque
O sistema SHALL registrar entradas de estoque com origem (compra, devolução de venda, ajuste, produção) e rastreabilidade de lote/série.

#### Scenario: Entrada via recebimento de compra
- **WHEN** um pedido de compra é recebido no módulo de Compras
- **THEN** a entrada de estoque é criada automaticamente com referência à nota fiscal de entrada

#### Scenario: Entrada manual por ajuste
- **WHEN** o usuário registra uma entrada manual com motivo "Ajuste de Inventário"
- **THEN** o sistema registra a entrada com usuário, data e motivo no histórico

### Requirement: Saída de Estoque
O sistema SHALL registrar saídas de estoque com origem (venda, devolução para fornecedor, perda, ajuste) e baixa automática por FIFO ou FEFO.

#### Scenario: Saída automática por faturamento de venda
- **WHEN** uma venda é faturada no módulo de Vendas
- **THEN** os itens da venda geram saídas de estoque automaticamente

#### Scenario: Método de custeio FIFO
- **WHEN** o tenant configura custeio FIFO e uma saída é registrada
- **THEN** o sistema usa o custo dos lotes mais antigos para calcular o CMV

### Requirement: Inventário (Contagem)
O sistema SHALL suportar inventário periódico com geração de planilha de contagem, registro de quantidades encontradas e ajuste automático das diferenças.

#### Scenario: Abertura de inventário
- **WHEN** o usuário abre um inventário para uma categoria ou todos os produtos
- **THEN** o sistema gera uma lista de contagem com produto, localização e saldo atual (opcional mostrar saldo)
- **THEN** movimentações de estoque dos produtos em inventário ficam bloqueadas durante a contagem

#### Scenario: Fechamento de inventário com diferença
- **WHEN** o usuário fecha o inventário com quantidades divergentes do sistema
- **THEN** o sistema gera ajustes automáticos (entrada ou saída) para cada produto com diferença
- **THEN** os ajustes ficam registrados com referência ao inventário no histórico

### Requirement: Relatório de Movimentação de Estoque
O sistema SHALL gerar relatório de movimentação por produto, período e tipo de operação com saldo inicial, entradas, saídas e saldo final.

#### Scenario: Relatório de movimentação por período
- **WHEN** o usuário solicita o relatório de movimentação de um produto em um período
- **THEN** o sistema exibe todas as entradas e saídas com data, origem, quantidade e custo unitário
- **THEN** o relatório pode ser exportado em Excel

### Requirement: Ponto de Pedido e Estoque Mínimo
O sistema SHALL alertar quando o saldo de um produto atingir o estoque mínimo configurado.

#### Scenario: Alerta de estoque mínimo
- **WHEN** o saldo de um produto cai abaixo do estoque mínimo configurado
- **THEN** o sistema gera uma notificação para o responsável pelo estoque
- **THEN** o produto aparece destacado no painel de estoque crítico
