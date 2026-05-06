import { Routes } from '@angular/router';

export const ESTOQUE_ROUTES: Routes = [
  { path: '', redirectTo: 'saldo', pathMatch: 'full' },
  { path: 'saldo', loadComponent: () => import('./saldo/saldo.component').then((m) => m.SaldoComponent) },
  { path: 'movimentacao', loadComponent: () => import('./movimentacao/movimentacao.component').then((m) => m.MovimentacaoComponent) },
  { path: 'inventario', loadComponent: () => import('./inventario/inventario-list.component').then((m) => m.InventarioListComponent) },
  { path: 'inventario/:id', loadComponent: () => import('./inventario/inventario-form.component').then((m) => m.InventarioFormComponent) },
];
