import { Routes } from '@angular/router';

export const COMPRAS_ROUTES: Routes = [
  { path: '', redirectTo: 'solicitacoes', pathMatch: 'full' },
  { path: 'solicitacoes', loadComponent: () => import('./solicitacoes/solicitacao-list.component').then((m) => m.SolicitacaoListComponent) },
  { path: 'solicitacoes/:id', loadComponent: () => import('./solicitacoes/solicitacao-form.component').then((m) => m.SolicitacaoFormComponent) },
  { path: 'pedidos', loadComponent: () => import('./pedidos/pedido-list.component').then((m) => m.PedidoListComponent) },
  { path: 'pedidos/:id', loadComponent: () => import('./pedidos/pedido-form.component').then((m) => m.PedidoFormComponent) },
  { path: 'recebimentos', loadComponent: () => import('./recebimentos/recebimento-list.component').then((m) => m.RecebimentoListComponent) },
  { path: 'recebimentos/:id', loadComponent: () => import('./recebimentos/recebimento-form.component').then((m) => m.RecebimentoFormComponent) },
];
