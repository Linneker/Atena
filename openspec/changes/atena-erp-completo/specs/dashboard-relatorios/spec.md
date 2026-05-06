## ADDED Requirements

### Requirement: Dashboard Executivo
O sistema SHALL exibir um painel inicial com KPIs do tenant em tempo real: receita do mês, despesas, resultado, vendas abertas, contas a vencer e estoque crítico.

#### Scenario: Carregamento do dashboard
- **WHEN** o usuário acessa o sistema após login
- **THEN** o dashboard exibe os KPIs do mês atual do tenant autenticado
- **THEN** os dados são carregados em menos de 2 segundos

#### Scenario: Filtro de período no dashboard
- **WHEN** o usuário seleciona um período diferente no dashboard
- **THEN** todos os KPIs são atualizados para o período selecionado sem recarregar a página

#### Scenario: Alertas no dashboard
- **WHEN** existem contas a vencer nos próximos 3 dias
- **THEN** o dashboard exibe o total e quantidade no widget de "Vencimentos Próximos"
- **THEN** clicar no widget redireciona para a lista filtrada de contas a vencer

### Requirement: Gráficos de Evolução Financeira
O sistema SHALL exibir gráficos de linha/barra com evolução mensal de receitas, despesas e resultado dos últimos 12 meses.

#### Scenario: Gráfico de receita vs despesa
- **WHEN** o usuário acessa o painel financeiro
- **THEN** o sistema exibe gráfico comparativo de receitas e despesas por mês
- **THEN** o gráfico é interativo (hover mostra valores, clique drilla para o mês)

### Requirement: Relatório de Vendas
O sistema SHALL gerar relatório de vendas por período com filtros por vendedor, cliente, produto, status e forma de pagamento.

#### Scenario: Relatório de vendas por vendedor
- **WHEN** o usuário gera o relatório com filtro de vendedor e período
- **THEN** o sistema exibe total de vendas, ticket médio e comissão do vendedor

#### Scenario: Exportação do relatório
- **WHEN** o usuário clica em "Exportar"
- **THEN** o sistema oferece opções de PDF e Excel
- **THEN** o arquivo é gerado em background e disponibilizado para download

### Requirement: Relatório de Estoque
O sistema SHALL gerar relatório de posição de estoque (saldo atual) e relatório de movimentação por período.

#### Scenario: Posição de estoque com valor
- **WHEN** o usuário solicita a posição de estoque
- **THEN** o sistema exibe cada produto com quantidade e valor total (custo médio)
- **THEN** exibe o valor total do estoque ao final

### Requirement: Relatório de Contas a Pagar e Receber
O sistema SHALL gerar relatório de aging (por faixa de vencimento) para contas a pagar e a receber.

#### Scenario: Aging de contas a pagar
- **WHEN** o usuário abre o relatório de aging
- **THEN** o sistema exibe contas agrupadas por faixa: "a vencer", "vence hoje", "1-7 dias vencido", "8-30 dias", "31-60 dias", "60+ dias"

### Requirement: Relatório de DRE
O sistema SHALL gerar o DRE do tenant por competência com estrutura baseada no plano de contas.

#### Scenario: DRE mensal em PDF
- **WHEN** o usuário solicita o DRE de uma competência
- **THEN** o sistema gera o relatório com estrutura hierárquica do plano de contas
- **THEN** exibe resultado bruto, despesas operacionais e resultado líquido
- **THEN** o PDF é gerado com identidade visual do tenant (logo e cores)
