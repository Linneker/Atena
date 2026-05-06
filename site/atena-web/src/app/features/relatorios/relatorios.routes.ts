import { Routes } from '@angular/router';

export const RELATORIOS_ROUTES: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'dashboard', loadComponent: () => import('./dashboard.component').then((m) => m.DashboardRelatoriosComponent) },
  { path: 'dre', loadComponent: () => import('./dre.component').then((m) => m.DreComponent) },
  { path: 'balanco', loadComponent: () => import('./balanco.component').then((m) => m.BalancoComponent) },
  { path: 'aging-pagar', loadComponent: () => import('./aging-pagar.component').then((m) => m.AgingPagarComponent) },
  { path: 'aging-receber', loadComponent: () => import('./aging-receber.component').then((m) => m.AgingReceberComponent) },
  { path: 'vendas', loadComponent: () => import('./vendas-relatorio.component').then((m) => m.VendasRelatorioComponent) },
  { path: 'estoque', loadComponent: () => import('./estoque-relatorio.component').then((m) => m.EstoqueRelatorioComponent) },
];
