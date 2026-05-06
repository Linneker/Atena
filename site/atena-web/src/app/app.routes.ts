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
      {
        path: 'financeiro',
        canActivate: [permissaoGuard],
        data: { permissao: 'financeiro.visualizar' },
        loadChildren: () => import('@features/financeiro/financeiro.routes').then((m) => m.FINANCEIRO_ROUTES),
      },
      {
        path: 'cadastros',
        canActivate: [permissaoGuard],
        data: { permissao: 'cadastros.visualizar' },
        loadChildren: () => import('@features/cadastros/cadastros.routes').then((m) => m.CADASTROS_ROUTES),
      },
      {
        path: 'estoque',
        canActivate: [permissaoGuard],
        data: { permissao: 'estoque.visualizar' },
        loadChildren: () => import('@features/estoque/estoque.routes').then((m) => m.ESTOQUE_ROUTES),
      },
      {
        path: 'compras',
        canActivate: [permissaoGuard],
        data: { permissao: 'compras.visualizar' },
        loadChildren: () => import('@features/compras/compras.routes').then((m) => m.COMPRAS_ROUTES),
      },
      {
        path: 'vendas',
        canActivate: [permissaoGuard],
        data: { permissao: 'vendas.visualizar' },
        loadChildren: () => import('@features/vendas/vendas.routes').then((m) => m.VENDAS_ROUTES),
      },
      {
        path: 'fiscal',
        canActivate: [permissaoGuard],
        data: { permissao: 'fiscal.visualizar' },
        loadChildren: () => import('@features/fiscal/fiscal.routes').then((m) => m.FISCAL_ROUTES),
      },
      {
        path: 'relatorios',
        canActivate: [permissaoGuard],
        data: { permissao: 'relatorios.visualizar' },
        loadChildren: () => import('@features/relatorios/relatorios.routes').then((m) => m.RELATORIOS_ROUTES),
      },
      {
        path: 'configuracao',
        canActivate: [permissaoGuard],
        data: { permissao: 'configuracao.visualizar' },
        loadChildren: () => import('@features/configuracao/configuracao.routes').then((m) => m.CONFIGURACAO_ROUTES),
      },
    ],
  },
  { path: '**', redirectTo: '' },
];
