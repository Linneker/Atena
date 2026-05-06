## ADDED Requirements

### Requirement: Gestão de Despesas
O sistema SHALL permitir registro, categorização e consulta de despesas por competência, categoria, centro de custo e status de pagamento.

#### Scenario: Registro de despesa com vencimento futuro
- **WHEN** o usuário registra uma despesa com data de vencimento futura
- **THEN** a despesa é criada com status "Pendente"
- **THEN** aparece em Contas a Pagar na data de vencimento

#### Scenario: Baixa de despesa paga
- **WHEN** o usuário registra o pagamento de uma despesa pendente
- **THEN** o status muda para "Pago" com data e forma de pagamento registradas
- **THEN** o FluxoDeCaixa do período é atualizado automaticamente

### Requirement: Gestão de Receitas
O sistema SHALL permitir registro, categorização e consulta de receitas por competência e origem (venda, avulsa, financeira).

#### Scenario: Receita gerada automaticamente por venda
- **WHEN** uma venda é faturada no módulo de Vendas
- **THEN** uma receita é criada automaticamente com referência à venda
- **THEN** a receita aparece no Fluxo de Caixa da data de faturamento

#### Scenario: Receita avulsa manual
- **WHEN** o usuário registra uma receita avulsa (ex: aluguel recebido)
- **THEN** a receita é criada e reflete imediatamente no Fluxo de Caixa

### Requirement: Contas a Pagar
O sistema SHALL gerenciar contas a pagar com vencimento, agrupamento por fornecedor, alertas de vencimento e baixa parcial ou total.

#### Scenario: Alerta de vencimento próximo
- **WHEN** uma conta a pagar vence em até 3 dias
- **THEN** o sistema gera uma notificação para o usuário responsável
- **THEN** a conta aparece destacada no dashboard

#### Scenario: Baixa parcial de conta
- **WHEN** o usuário registra um pagamento parcial de uma conta
- **THEN** o saldo restante é mantido como pendente com nova data de vencimento opcional
- **THEN** o pagamento parcial é registrado no histórico da conta

### Requirement: Contas a Receber
O sistema SHALL gerenciar contas a receber com controle de inadimplência, agrupamento por cliente e baixa.

#### Scenario: Controle de inadimplência
- **WHEN** uma conta a receber está vencida há mais de X dias (configurável por tenant)
- **THEN** o sistema classifica o cliente como inadimplente
- **THEN** o módulo de Vendas bloqueia novas vendas para esse cliente (configurável)

#### Scenario: Baixa de conta recebida
- **WHEN** o usuário confirma o recebimento de uma conta
- **THEN** o status muda para "Recebido" com data e conta bancária registradas
- **THEN** o Fluxo de Caixa é atualizado

### Requirement: Fluxo de Caixa com Fechamento
O sistema SHALL calcular o Fluxo de Caixa por período com saldo inicial, entradas, saídas e saldo final; permitir fechamento mensal imutável.

#### Scenario: Cálculo automático do fluxo
- **WHEN** o usuário abre o Fluxo de Caixa de um período
- **THEN** o sistema consolida todas as entradas (receitas, recebimentos) e saídas (despesas, pagamentos) do período
- **THEN** exibe saldo inicial, total de entradas, total de saídas e saldo final

#### Scenario: Fechamento de período
- **WHEN** o usuário executa o fechamento do mês
- **THEN** o saldo final torna-se o saldo inicial do próximo mês
- **THEN** nenhuma alteração pode ser feita em lançamentos do período fechado

### Requirement: Conciliação Bancária
O sistema SHALL permitir importar extrato bancário (OFX/CSV) e conciliar lançamentos automaticamente com as transações registradas.

#### Scenario: Importação de extrato OFX
- **WHEN** o usuário importa um arquivo OFX do banco
- **THEN** o sistema processa os lançamentos do extrato
- **THEN** tenta conciliar automaticamente por valor e data com transações existentes

#### Scenario: Conciliação manual de lançamento não casado
- **WHEN** um lançamento do extrato não encontra correspondência automática
- **THEN** o usuário pode associá-lo manualmente a uma transação existente ou criar uma nova

### Requirement: DRE e Balanço Patrimonial
O sistema SHALL gerar Demonstrativo de Resultado do Exercício (DRE) e Balanço Patrimonial por período, baseados no Plano de Contas do tenant.

#### Scenario: Geração do DRE mensal
- **WHEN** o usuário solicita o DRE de um período
- **THEN** o sistema consolida receitas, custo de produtos vendidos, despesas operacionais e resultado líquido
- **THEN** o relatório pode ser exportado em PDF e Excel

#### Scenario: Plano de Contas personalizado
- **WHEN** o tenant configura seu plano de contas
- **THEN** todas as despesas e receitas são associadas às contas correspondentes
- **THEN** o DRE e Balanço refletem a estrutura do plano de contas do tenant
