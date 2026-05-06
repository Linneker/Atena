## ADDED Requirements

### Requirement: Frontend Único Angular/CoreUI
O sistema SHALL ter um único projeto Angular como interface principal, consolidando todos os módulos ERP — eliminando cashflow/, cashflow2/ e o MVC site.

#### Scenario: Acesso a qualquer módulo pelo frontend único
- **WHEN** um usuário autenticado acessa o sistema
- **THEN** o menu lateral exibe apenas os módulos que o usuário tem permissão de acessar
- **THEN** todos os módulos (financeiro, vendas, compras, estoque, cadastros, fiscal, relatórios) estão disponíveis na mesma aplicação

#### Scenario: Lazy loading de módulos Angular
- **WHEN** o usuário navega para um módulo pela primeira vez
- **THEN** o bundle desse módulo é carregado sob demanda (lazy loading)
- **THEN** o carregamento inicial da aplicação não ultrapassa 200KB gzipped

### Requirement: Layout Responsivo
O sistema SHALL funcionar corretamente em desktop (1366px+), tablet (768px) e mobile (360px).

#### Scenario: Adaptação do menu em mobile
- **WHEN** o usuário acessa em dispositivo mobile
- **THEN** o menu lateral é substituído por um menu hamburguer
- **THEN** todas as tabelas de listagem são substituídas por cards/listas adaptadas

### Requirement: Controle de Acesso no Frontend
O sistema SHALL esconder elementos de UI (botões, menus, ações) baseado nas permissões do usuário autenticado — sem depender exclusivamente do backend.

#### Scenario: Botão de ação oculto sem permissão
- **WHEN** o usuário não tem permissão de `excluir` em um recurso
- **THEN** o botão "Excluir" não aparece na interface
- **THEN** se acessado diretamente via URL, a API retorna 403

### Requirement: Identidade Visual por Tenant (Branding)
O sistema SHALL aplicar logo e cores primárias do tenant carregadas após o login.

#### Scenario: Carregamento de branding do tenant
- **WHEN** o usuário faz login com sucesso
- **THEN** o frontend busca as configurações de branding do tenant
- **THEN** aplica a logo no sidebar e cores primárias via CSS custom properties

### Requirement: Notificações em Tempo Real
O sistema SHALL exibir notificações em tempo real para eventos relevantes: vencimentos, aprovações pendentes, alertas de estoque.

#### Scenario: Notificação de aprovação pendente
- **WHEN** um pedido de compra requer aprovação do usuário logado
- **THEN** o sino de notificações exibe o badge com o número de pendências
- **THEN** clicar na notificação navega para o pedido

### Requirement: Tabelas com Paginação, Filtro e Ordenação
Todas as listagens de entidades SHALL usar paginação server-side, filtros dinâmicos e ordenação por coluna.

#### Scenario: Paginação de listagem
- **WHEN** o usuário acessa uma listagem com mais de 20 registros
- **THEN** o sistema exibe 20 por página com controles de página anterior/próxima
- **THEN** o total de registros é exibido

#### Scenario: Filtro em tempo real
- **WHEN** o usuário digita no campo de busca da listagem
- **THEN** o sistema dispara a busca após 300ms de debounce
- **THEN** a URL é atualizada com os parâmetros de filtro (deep linking)

### Requirement: Exportação de Listagens
O sistema SHALL permitir exportar qualquer listagem para Excel diretamente do frontend.

#### Scenario: Exportação de listagem filtrada
- **WHEN** o usuário aplica filtros e clica em "Exportar Excel"
- **THEN** o sistema exporta todos os registros do filtro (não apenas a página atual) para XLSX
