## ADDED Requirements

### Requirement: Solicitação de Compra
O sistema SHALL permitir que qualquer usuário autorizado abra uma solicitação de compra que segue fluxo de aprovação antes de virar pedido.

#### Scenario: Criação de solicitação
- **WHEN** um usuário cria uma solicitação de compra com produto, quantidade e justificativa
- **THEN** a solicitação vai para fila de aprovação do responsável configurado
- **THEN** o solicitante recebe notificação de criação

#### Scenario: Aprovação da solicitação
- **WHEN** o aprovador aprova a solicitação
- **THEN** a solicitação pode ser convertida em pedido de compra
- **THEN** o solicitante é notificado da aprovação

### Requirement: Pedido de Compra
O sistema SHALL gerar pedido de compra com dados do fornecedor, itens, quantidades, preços, condições de pagamento e prazo de entrega.

#### Scenario: Criação de pedido a partir de solicitação aprovada
- **WHEN** o comprador converte uma solicitação aprovada em pedido
- **THEN** o pedido é criado com status "Aguardando Confirmação do Fornecedor"
- **THEN** o pedido pode ser enviado ao fornecedor por e-mail em PDF

#### Scenario: Pedido com múltiplos itens
- **WHEN** o comprador adiciona múltiplos produtos ao pedido
- **THEN** o sistema calcula totais por item (quantidade × preço) e total geral com frete e descontos

### Requirement: Recebimento de Mercadoria
O sistema SHALL registrar o recebimento físico de mercadorias de um pedido de compra com conferência de quantidade e qualidade.

#### Scenario: Recebimento total do pedido
- **WHEN** o usuário confirma o recebimento total do pedido
- **THEN** os itens entram no estoque automaticamente
- **THEN** o pedido muda para status "Recebido"
- **THEN** uma conta a pagar é gerada conforme condição de pagamento do pedido

#### Scenario: Recebimento parcial
- **WHEN** o usuário recebe apenas parte do pedido
- **THEN** os itens recebidos entram no estoque
- **THEN** o pedido permanece "Parcialmente Recebido" com saldo pendente

#### Scenario: Divergência de quantidade no recebimento
- **WHEN** a quantidade recebida difere da pedida
- **THEN** o sistema registra a divergência e notifica o comprador
- **THEN** o usuário pode aceitar a quantidade recebida ou registrar devolução parcial

### Requirement: Nota Fiscal de Entrada (Escrituração)
O sistema SHALL registrar a nota fiscal de entrada vinculada ao recebimento, com chave de acesso NF-e, CFOP e tributos.

#### Scenario: Vinculação de NF-e de entrada
- **WHEN** o usuário informa a chave de acesso da NF-e de entrada
- **THEN** o sistema consulta a SEFAZ para validar a NF-e
- **THEN** os dados fiscais são preenchidos automaticamente

### Requirement: Aprovação com Alçada
O sistema SHALL suportar alçadas de aprovação por valor — pedidos acima de um limite requerem aprovação de nível superior.

#### Scenario: Pedido acima da alçada
- **WHEN** um pedido ultrapassa o valor da alçada do aprovador atual
- **THEN** o pedido é roteado automaticamente para o aprovador do nível superior
- **THEN** ambos os aprovadores recebem notificação
