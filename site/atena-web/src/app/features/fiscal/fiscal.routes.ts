import { Routes } from '@angular/router';

export const FISCAL_ROUTES: Routes = [
  { path: '', redirectTo: 'nfe', pathMatch: 'full' },
  { path: 'configuracao', loadComponent: () => import('./configuracao/configuracao-fiscal.component').then((m) => m.ConfiguracaoFiscalComponent) },
  { path: 'nfe', loadComponent: () => import('./nfe/nfe-list.component').then((m) => m.NFeListComponent) },
  { path: 'nfe/:id', loadComponent: () => import('./nfe/nfe-detalhe.component').then((m) => m.NFeDetalheComponent) },
];
