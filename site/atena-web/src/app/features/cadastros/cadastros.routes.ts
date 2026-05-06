import { Routes } from '@angular/router';

export const CADASTROS_ROUTES: Routes = [
  { path: '', redirectTo: 'clientes', pathMatch: 'full' },
  { path: 'clientes', loadComponent: () => import('./clientes/cliente-list.component').then((m) => m.ClienteListComponent) },
  { path: 'clientes/:id', loadComponent: () => import('./clientes/cliente-form.component').then((m) => m.ClienteFormComponent) },
  { path: 'fornecedores', loadComponent: () => import('./fornecedores/fornecedor-list.component').then((m) => m.FornecedorListComponent) },
  { path: 'fornecedores/:id', loadComponent: () => import('./fornecedores/fornecedor-form.component').then((m) => m.FornecedorFormComponent) },
  { path: 'funcionarios', loadComponent: () => import('./funcionarios/funcionario-list.component').then((m) => m.FuncionarioListComponent) },
  { path: 'funcionarios/:id', loadComponent: () => import('./funcionarios/funcionario-form.component').then((m) => m.FuncionarioFormComponent) },
  { path: 'produtos', loadComponent: () => import('./produtos/produto-list.component').then((m) => m.ProdutoListComponent) },
  { path: 'produtos/:id', loadComponent: () => import('./produtos/produto-form.component').then((m) => m.ProdutoFormComponent) },
  { path: 'centros-custo', loadComponent: () => import('./centros-custo/centro-custo-list.component').then((m) => m.CentroCustoListComponent) },
  { path: 'centros-custo/:id', loadComponent: () => import('./centros-custo/centro-custo-form.component').then((m) => m.CentroCustoFormComponent) },
  { path: 'plano-contas', loadComponent: () => import('./plano-contas/plano-contas-list.component').then((m) => m.PlanoContasListComponent) },
  { path: 'plano-contas/:id', loadComponent: () => import('./plano-contas/plano-contas-form.component').then((m) => m.PlanoContasFormComponent) },
];
