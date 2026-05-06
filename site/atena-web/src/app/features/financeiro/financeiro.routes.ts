import { Routes } from '@angular/router';

export const FINANCEIRO_ROUTES: Routes = [
  { path: '', redirectTo: 'despesas', pathMatch: 'full' },
  {
    path: 'despesas',
    loadComponent: () => import('./despesas/despesa-list.component').then((m) => m.DespesaListComponent),
  },
  {
    path: 'despesas/:id',
    loadComponent: () => import('./despesas/despesa-form.component').then((m) => m.DespesaFormComponent),
  },
  {
    path: 'receitas',
    loadComponent: () => import('./receitas/receita-list.component').then((m) => m.ReceitaListComponent),
  },
  {
    path: 'receitas/:id',
    loadComponent: () => import('./receitas/receita-form.component').then((m) => m.ReceitaFormComponent),
  },
  {
    path: 'fluxo-caixa',
    loadComponent: () => import('./fluxo-caixa/fluxo-caixa.component').then((m) => m.FluxoCaixaComponent),
  },
  {
    path: 'contas-pagar',
    loadComponent: () => import('./contas-pagar/contas-pagar-list.component').then((m) => m.ContasPagarListComponent),
  },
  {
    path: 'contas-receber',
    loadComponent: () => import('./contas-receber/contas-receber-list.component').then((m) => m.ContasReceberListComponent),
  },
  {
    path: 'conciliacao',
    loadComponent: () => import('./conciliacao/conciliacao.component').then((m) => m.ConciliacaoComponent),
  },
];
