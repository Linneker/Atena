## ADDED Requirements

### Requirement: Isolamento de dados por tenant
O sistema SHALL garantir que todos os dados de negócio (financeiro, estoque, vendas, cadastros) sejam isolados por tenant, sendo impossível um tenant acessar dados de outro mesmo que autenticado.

#### Scenario: Consulta retorna apenas dados do tenant autenticado
- **WHEN** um usuário autenticado realiza qualquer consulta
- **THEN** o sistema filtra automaticamente por `tenant_id` extraído do JWT
- **THEN** registros de outros tenants nunca aparecem no resultado

#### Scenario: Tentativa de acesso cross-tenant via ID direto
- **WHEN** um usuário tenta acessar um recurso pelo ID de outro tenant
- **THEN** o sistema retorna HTTP 404 (não 403, para não revelar existência)

### Requirement: Cadastro e gestão de tenants
O sistema SHALL permitir o cadastro de tenants com dados da empresa, plano de assinatura e status ativo/inativo.

#### Scenario: Criação de novo tenant
- **WHEN** um administrador do sistema cria um novo tenant com CNPJ, razão social e plano
- **THEN** o tenant é criado com status ativo e UUID único como `tenant_id`
- **THEN** o tenant recebe um usuário administrador padrão

#### Scenario: Inativação de tenant
- **WHEN** um tenant é inativado
- **THEN** todos os usuários daquele tenant recebem HTTP 401 nas requisições
- **THEN** os dados permanecem no banco mas inacessíveis via API

### Requirement: Propagação automática do tenant_id
O sistema SHALL injetar `tenant_id` automaticamente em todos os registros criados, sem necessidade de o cliente informar.

#### Scenario: Criação de registro com tenant_id automático
- **WHEN** um usuário cria qualquer entidade (despesa, produto, venda, etc.)
- **THEN** o campo `tenant_id` é preenchido automaticamente a partir do JWT
- **THEN** o cliente não pode sobrescrever `tenant_id` via payload da requisição

### Requirement: Configurações e branding por tenant
Cada tenant SHALL poder configurar logo, cores primárias, nome de exibição e fuso horário independentemente.

#### Scenario: Configuração de branding
- **WHEN** o administrador do tenant atualiza as configurações de branding
- **THEN** o frontend carrega as configurações do tenant na inicialização da sessão
- **THEN** as configurações se aplicam apenas a usuários daquele tenant

### Requirement: Plano de assinatura e limites
O sistema SHALL controlar limites por plano (número de usuários, volume de NF-e/mês, espaço de armazenamento).

#### Scenario: Limite de usuários atingido
- **WHEN** um tenant tenta criar um usuário além do limite do seu plano
- **THEN** o sistema retorna HTTP 402 com mensagem indicando necessidade de upgrade

#### Scenario: Limite de NF-e atingido
- **WHEN** um tenant tenta emitir uma NF-e além do limite mensal do seu plano
- **THEN** a emissão é bloqueada com mensagem clara de limite atingido
