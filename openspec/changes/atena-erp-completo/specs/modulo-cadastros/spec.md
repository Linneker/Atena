## ADDED Requirements

### Requirement: Cadastro de Clientes
O sistema SHALL manter cadastro completo de clientes com dados fiscais (CPF/CNPJ), endereços, contatos e histórico de compras.

#### Scenario: Cadastro com validação de CPF/CNPJ
- **WHEN** o usuário cadastra um cliente informando CPF ou CNPJ
- **THEN** o sistema valida o dígito verificador
- **THEN** rejeita documentos inválidos com mensagem descritiva

#### Scenario: Preenchimento automático por CEP
- **WHEN** o usuário informa o CEP do endereço
- **THEN** o sistema consulta a API ViaCEP e preenche logradouro, bairro, cidade e UF automaticamente

#### Scenario: Bloqueio de cliente inadimplente em nova venda
- **WHEN** uma nova venda é iniciada para um cliente marcado como inadimplente
- **THEN** o sistema exibe alerta e bloqueia a continuação (se a regra estiver ativa no tenant)

### Requirement: Cadastro de Fornecedores
O sistema SHALL manter cadastro de fornecedores com dados fiscais, endereços, condições de pagamento padrão e produtos fornecidos.

#### Scenario: Vínculo de produto a fornecedor
- **WHEN** o usuário associa um produto a um fornecedor com preço de custo
- **THEN** o sistema usa esse preço como custo padrão em pedidos de compra para aquele fornecedor

#### Scenario: Histórico de compras por fornecedor
- **WHEN** o usuário visualiza o cadastro de um fornecedor
- **THEN** o sistema exibe todos os pedidos de compra associados com valores e datas

### Requirement: Cadastro de Funcionários
O sistema SHALL manter cadastro básico de funcionários com cargo, departamento, dados pessoais e vínculo empregatício.

#### Scenario: Cadastro de funcionário com departamento
- **WHEN** o usuário cadastra um funcionário e informa departamento e cargo
- **THEN** o funcionário fica disponível para seleção como responsável em centros de custo e aprovações

### Requirement: Centro de Custo
O sistema SHALL permitir criar centros de custo e vincular despesas, receitas e funcionários a eles para análise de resultado por área.

#### Scenario: Vínculo de despesa a centro de custo
- **WHEN** o usuário registra uma despesa e seleciona um centro de custo
- **THEN** a despesa é contabilizada no DRE de forma segregada por centro de custo

#### Scenario: Relatório de resultado por centro de custo
- **WHEN** o usuário solicita o relatório de um centro de custo em um período
- **THEN** o sistema exibe todas as despesas e receitas vinculadas com saldo resultado

### Requirement: Plano de Contas
O sistema SHALL manter o plano de contas contábil do tenant com estrutura hierárquica (grupo, subgrupo, conta) e vínculos com categorias de despesa/receita.

#### Scenario: Criação de conta no plano
- **WHEN** o usuário cria uma nova conta com código hierárquico e tipo (ativo/passivo/receita/despesa)
- **THEN** a conta fica disponível para vínculo em lançamentos financeiros

#### Scenario: Validação de hierarquia
- **WHEN** o usuário tenta criar uma conta filha de um código inexistente
- **THEN** o sistema rejeita com mensagem indicando que a conta pai não existe

### Requirement: Catálogo de Produtos
O sistema SHALL manter catálogo de produtos com tipo, unidade de medida, tabela de preços por tipo de cliente e código de barras/SKU.

#### Scenario: Produto com múltiplos preços
- **WHEN** o usuário configura preços por tipo de cliente (varejo, atacado, distribuidor)
- **THEN** no pedido de venda, o preço é preenchido automaticamente conforme o tipo do cliente

#### Scenario: Código de barras duplicado
- **WHEN** o usuário tenta cadastrar um produto com código de barras já existente no tenant
- **THEN** o sistema rejeita com mensagem de duplicidade
