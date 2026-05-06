## ADDED Requirements

### Requirement: Roles (Perfis de Acesso)
O sistema SHALL permitir criar perfis de acesso (roles) por tenant com nome, descrição e conjunto de permissões.

#### Scenario: Criação de role por tenant
- **WHEN** o administrador do tenant cria uma role "Vendedor" com permissões específicas
- **THEN** a role fica disponível para atribuição a usuários do tenant
- **THEN** a role não é visível para outros tenants

#### Scenario: Role padrão "Administrador"
- **WHEN** um novo tenant é criado
- **THEN** o sistema cria automaticamente a role "Administrador" com todas as permissões
- **THEN** o primeiro usuário do tenant recebe essa role

### Requirement: Permissões Granulares por Recurso e Ação
O sistema SHALL controlar acesso por recurso (ex: `vendas`, `financeiro`, `estoque`) e ação (`ler`, `criar`, `editar`, `excluir`, `aprovar`).

#### Scenario: Verificação de permissão em endpoint
- **WHEN** um usuário acessa um endpoint da API
- **THEN** o sistema verifica se o usuário possui a permissão correspondente ao recurso e ação
- **THEN** retorna HTTP 403 com mensagem descritiva se não tiver permissão

#### Scenario: Permissão de aprovação separada de criação
- **WHEN** um usuário tem permissão de `criar` pedido de compra mas não de `aprovar`
- **THEN** o usuário pode criar solicitações mas não pode aprovar pedidos de outros
- **THEN** a ação "Aprovar" no frontend é invisível para esse usuário

### Requirement: Atribuição de Roles a Usuários
O sistema SHALL permitir atribuir uma ou mais roles a cada usuário, com data de vigência opcional.

#### Scenario: Usuário com múltiplas roles
- **WHEN** um usuário tem as roles "Vendedor" e "Analista Financeiro"
- **THEN** o sistema une as permissões de ambas as roles (permissão aditiva)
- **THEN** o JWT inclui todas as permissões consolidadas

#### Scenario: Role com expiração
- **WHEN** uma role atribuída a um usuário expira
- **THEN** as permissões dessa role são removidas na próxima autenticação
- **THEN** o usuário recebe notificação de que seu acesso foi alterado

### Requirement: API Keys para Integrações
O sistema SHALL gerar e gerenciar API Keys por tenant para autenticação de integrações sem usuário humano.

#### Scenario: Geração de API Key
- **WHEN** o administrador gera uma API Key com nome, permissões e expiração
- **THEN** o sistema retorna o token apenas uma vez (não pode ser recuperado depois)
- **THEN** requisições com essa API Key operam no contexto do tenant com as permissões definidas

#### Scenario: Revogação de API Key
- **WHEN** o administrador revoga uma API Key
- **THEN** requisições com essa key passam a receber HTTP 401 imediatamente

### Requirement: Refresh Token e Renovação de Sessão
O sistema SHALL emitir refresh tokens de longa duração para renovar o JWT de acesso sem novo login.

#### Scenario: Renovação de JWT expirado
- **WHEN** o cliente envia um refresh token válido
- **THEN** o sistema emite novo JWT de acesso com novo refresh token (rotação)
- **THEN** o refresh token anterior é invalidado (adicionado à blacklist)

#### Scenario: Refresh token revogado
- **WHEN** o usuário faz logout
- **THEN** o refresh token atual é adicionado à blacklist
- **THEN** tentativas de uso do token revogado retornam HTTP 401

### Requirement: Log de Acesso e Tentativas de Login
O sistema SHALL registrar todas as tentativas de autenticação com sucesso ou falha, IP de origem e dispositivo.

#### Scenario: Bloqueio por tentativas consecutivas
- **WHEN** um usuário falha ao autenticar 5 vezes consecutivas (configurável)
- **THEN** a conta é bloqueada temporariamente por 15 minutos
- **THEN** o administrador do tenant é notificado
