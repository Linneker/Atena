## ADDED Requirements

### Requirement: Orçamento de Venda
O sistema SHALL permitir criar orçamentos com validade configurável, que podem ser convertidos em pedidos de venda.

#### Scenario: Orçamento com validade expirada
- **WHEN** a data de validade de um orçamento é atingida sem conversão
- **THEN** o orçamento muda automaticamente para status "Expirado"
- **THEN** não pode mais ser convertido sem revalidação de preços

#### Scenario: Conversão de orçamento em pedido
- **WHEN** o cliente aprova o orçamento e o usuário converte
- **THEN** um pedido de venda é criado com todos os itens e preços do orçamento

### Requirement: Pedido de Venda
O sistema SHALL gerenciar pedidos de venda com itens, quantidades, preços, descontos, condições de pagamento e status de progresso.

#### Scenario: Pedido com verificação de estoque
- **WHEN** o usuário finaliza um pedido de venda
- **THEN** o sistema verifica disponibilidade em estoque para cada item
- **THEN** se houver indisponibilidade, exibe aviso com saldo atual e permite continuar (reserva) ou ajustar o pedido

#### Scenario: Desconto máximo por perfil
- **WHEN** o vendedor tenta aplicar desconto acima do seu limite autorizado
- **THEN** o sistema bloqueia e solicita aprovação de um supervisor

#### Scenario: Reserva de estoque ao confirmar pedido
- **WHEN** o pedido é confirmado
- **THEN** as quantidades dos itens são reservadas no estoque (quantidade_reservada)
- **THEN** o saldo disponível é reduzido imediatamente

### Requirement: Faturamento
O sistema SHALL faturar pedidos confirmados, gerando a NF-e, baixando o estoque e criando a conta a receber.

#### Scenario: Faturamento de pedido
- **WHEN** o usuário fatura um pedido confirmado
- **THEN** a NF-e é emitida no módulo fiscal
- **THEN** os itens do pedido baixam o estoque (saída definitiva, liberando a reserva)
- **THEN** uma conta a receber é gerada conforme as condições de pagamento do pedido

#### Scenario: Faturamento parcial
- **WHEN** o usuário fatura apenas parte dos itens do pedido
- **THEN** os itens faturados geram NF-e e baixa de estoque
- **THEN** o pedido permanece "Parcialmente Faturado" com itens pendentes

### Requirement: Devolução de Venda
O sistema SHALL registrar devoluções de venda com NF-e de devolução, estorno de conta a receber e retorno ao estoque.

#### Scenario: Devolução total com NF-e
- **WHEN** o usuário registra a devolução total de uma venda
- **THEN** o sistema gera NF-e de devolução referenciando a NF-e original
- **THEN** os itens retornam ao estoque
- **THEN** a conta a receber original é estornada ou um crédito é gerado para o cliente

#### Scenario: Devolução parcial
- **WHEN** o usuário registra devolução de apenas alguns itens
- **THEN** a NF-e de devolução contém apenas os itens devolvidos
- **THEN** o estorno de conta a receber é proporcional ao valor devolvido

### Requirement: Comissão de Vendedor
O sistema SHALL calcular comissão por vendedor por venda baseado em percentual configurado por produto, categoria ou regra geral.

#### Scenario: Cálculo de comissão ao faturar
- **WHEN** uma venda é faturada
- **THEN** o sistema calcula a comissão do vendedor com base nas regras configuradas
- **THEN** a comissão fica registrada e pendente até o pagamento (configurável: calcular sobre pedido ou sobre recebimento)
