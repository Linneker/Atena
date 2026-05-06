import { Routes } from '@angular/router';

export const VENDAS_ROUTES: Routes = [
  { path: '', redirectTo: 'orcamentos', pathMatch: 'full' },
  { path: 'orcamentos', loadComponent: () => import('./orcamentos/orcamento-list.component').then((m) => m.OrcamentoListComponent) },
  { path: 'orcamentos/:id', loadComponent: () => import('./orcamentos/orcamento-form.component').then((m) => m.OrcamentoFormComponent) },
  { path: 'pedidos', loadComponent: () => import('./pedidos/pedido-venda-list.component').then((m) => m.PedidoVendaListComponent) },
  { path: 'pedidos/:id', loadComponent: () => import('./pedidos/pedido-venda-form.component').then((m) => m.PedidoVendaFormComponent) },
  { path: 'faturamentos', loadComponent: () => import('./faturamentos/faturamento-list.component').then((m) => m.FaturamentoListComponent) },
  { path: 'faturamentos/:id', loadComponent: () => import('./faturamentos/faturamento-form.component').then((m) => m.FaturamentoFormComponent) },
  { path: 'devolucoes', loadComponent: () => import('./devolucoes/devolucao-list.component').then((m) => m.DevolucaoListComponent) },
  { path: 'devolucoes/:id', loadComponent: () => import('./devolucoes/devolucao-form.component').then((m) => m.DevolucaoFormComponent) },
];
