import { Routes } from '@angular/router';
import { authGuard } from '@core/auth/auth.guard';
import { permissaoGuard } from '@core/permissions/permissao.guard';

export const routes: Routes = [
  {
    path: 'login',
    loadComponent: () => import('@core/auth/login.component').then((m) => m.LoginComponent),
  },
  {
    path: '',
    canActivate: [authGuard],
    loadComponent: () => import('./layout/default-layout.component').then((m) => m.DefaultLayoutComponent),
    children: [
      { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
      {
        path: 'dashboard',
        loadComponent: () => import('./layout/dashboard.component').then((m) => m.DashboardComponent),
      },
      // Permissões = arrays de "recurso:acao" reais do catálogo Acme.
      // permissaoGuard usa hasAnyPermission — basta uma para liberar a área.
      {
        path: 'financeiro',
        canActivate: [permissaoGuard],
        data: { permissao: ['despesa:ler', 'receita:ler', 'conta-pagar:ler', 'conta-receber:ler',
                            'fluxo-de-caixa:ler', 'conciliacao-bancaria:ler', 'plano-de-contas:ler', 'centro-de-custo:ler'] },
        loadChildren: () => import('@features/financeiro/financeiro.routes').then((m) => m.FINANCEIRO_ROUTES),
      },
      {
        path: 'cadastros',
        canActivate: [permissaoGuard],
        data: { permissao: ['cliente:ler', 'fornecedor:ler', 'funcionario:ler', 'empresa:ler'] },
        loadChildren: () => import('@features/cadastros/cadastros.routes').then((m) => m.CADASTROS_ROUTES),
      },
      {
        path: 'rh',
        canActivate: [permissaoGuard],
        data: { permissao: ['rh-funcionario:ler', 'rh-jornada:ler', 'rh-cargo:ler',
                            'rh-lotacao:ler', 'rh-departamento:ler', 'rh-beneficio:ler'] },
        loadChildren: () => import('@features/rh/rh.routes').then((m) => m.RH_ROUTES),
      },
      {
        path: 'estoque',
        canActivate: [permissaoGuard],
        data: { permissao: ['produto:ler', 'tipo-produto:ler', 'estoque:ler', 'inventario:ler'] },
        loadChildren: () => import('@features/estoque/estoque.routes').then((m) => m.ESTOQUE_ROUTES),
      },
      {
        path: 'compras',
        canActivate: [permissaoGuard],
        data: { permissao: ['solicitacao-compra:ler', 'pedido-compra:ler', 'recebimento:ler'] },
        loadChildren: () => import('@features/compras/compras.routes').then((m) => m.COMPRAS_ROUTES),
      },
      {
        path: 'vendas',
        canActivate: [permissaoGuard],
        data: { permissao: ['orcamento:ler', 'pedido-venda:ler', 'faturamento:ler', 'devolucao:ler'] },
        loadChildren: () => import('@features/vendas/vendas.routes').then((m) => m.VENDAS_ROUTES),
      },
      {
        path: 'fiscal',
        canActivate: [permissaoGuard],
        data: { permissao: ['nfe:ler', 'configuracao-fiscal:ler'] },
        loadChildren: () => import('@features/fiscal/fiscal.routes').then((m) => m.FISCAL_ROUTES),
      },
      {
        path: 'relatorios',
        canActivate: [permissaoGuard],
        data: { permissao: ['relatorio:ler'] },
        loadChildren: () => import('@features/relatorios/relatorios.routes').then((m) => m.RELATORIOS_ROUTES),
      },
      {
        path: 'configuracao',
        canActivate: [permissaoGuard],
        data: { permissao: ['tenant:ler', 'usuario:ler', 'role:ler', 'permissao:ler',
                            'feature-flags:ler', 'api-key:ler', 'auditoria:ler'] },
        loadChildren: () => import('@features/configuracao/configuracao.routes').then((m) => m.CONFIGURACAO_ROUTES),
      },
    ],
  },
  { path: '**', redirectTo: '' },
];
