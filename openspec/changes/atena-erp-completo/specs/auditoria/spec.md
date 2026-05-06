## ADDED Requirements

### Requirement: Log Funcional de Mutações
O sistema SHALL registrar toda criação, alteração e exclusão de entidades de negócio com dados antes e depois da mudança.

#### Scenario: Alteração de registro com log
- **WHEN** um usuário altera qualquer entidade (ex: edita uma despesa)
- **THEN** o sistema registra: tenant_id, usuário, timestamp, entidade, operação, dados anteriores (JSON) e dados posteriores (JSON)
- **THEN** o log é imutável — não pode ser editado ou excluído via API

#### Scenario: Exclusão lógica com log
- **WHEN** um usuário exclui um registro
- **THEN** o sistema realiza soft delete (flag `deleted_at`) e registra a exclusão no log de auditoria
- **THEN** o registro permanece no banco mas invisível nas consultas normais

### Requirement: Log de Requisições HTTP da API
O sistema SHALL registrar todas as requisições HTTP com método, rota, usuário, IP, status de resposta e tempo de processamento.

#### Scenario: Registro de requisição
- **WHEN** qualquer requisição é recebida pela API
- **THEN** um registro é criado com: tenant_id, user_id, method, path, status_code, duration_ms, ip, user_agent
- **THEN** o registro é assíncrono para não impactar a performance da requisição

### Requirement: Consulta de Auditoria por Administrador
O sistema SHALL permitir que administradores do tenant consultem o histórico de auditoria com filtros por usuário, entidade, período e operação.

#### Scenario: Consulta de histórico de um registro
- **WHEN** o administrador consulta o histórico de uma despesa específica pelo ID
- **THEN** o sistema exibe todas as alterações da despesa em ordem cronológica com dados antes/depois

#### Scenario: Relatório de atividades por usuário
- **WHEN** o administrador filtra por usuário e período
- **THEN** o sistema lista todas as ações do usuário com timestamp e dados modificados

### Requirement: Retenção de Logs
O sistema SHALL reter logs de auditoria por no mínimo 5 anos e permitir exportação para fins legais.

#### Scenario: Exportação de logs para auditoria externa
- **WHEN** o administrador exporta os logs de um período
- **THEN** o sistema gera um arquivo JSON ou CSV com todos os registros do período
- **THEN** o arquivo inclui hash de integridade para verificação de não adulteração
