import { Routes } from '@angular/router';

export const RH_ROUTES: Routes = [
  { path: '', redirectTo: 'funcionarios', pathMatch: 'full' },

  { path: 'jornadas', loadComponent: () => import('./jornadas/jornada-list.component').then((m) => m.JornadaListComponent) },
  { path: 'jornadas/:id', loadComponent: () => import('./jornadas/jornada-form.component').then((m) => m.JornadaFormComponent) },

  { path: 'cargos', loadComponent: () => import('./cargos/cargo-list.component').then((m) => m.CargoListComponent) },
  { path: 'cargos/:id', loadComponent: () => import('./cargos/cargo-form.component').then((m) => m.CargoFormComponent) },

  { path: 'lotacoes', loadComponent: () => import('./lotacoes/lotacao-list.component').then((m) => m.LotacaoListComponent) },
  { path: 'lotacoes/:id', loadComponent: () => import('./lotacoes/lotacao-form.component').then((m) => m.LotacaoFormComponent) },

  { path: 'departamentos', loadComponent: () => import('./departamentos/departamento-list.component').then((m) => m.DepartamentoListComponent) },
  { path: 'departamentos/:id', loadComponent: () => import('./departamentos/departamento-form.component').then((m) => m.DepartamentoFormComponent) },

  { path: 'beneficios', loadComponent: () => import('./beneficios/beneficio-list.component').then((m) => m.BeneficioListComponent) },
  { path: 'beneficios/:id', loadComponent: () => import('./beneficios/beneficio-form.component').then((m) => m.BeneficioFormComponent) },

  { path: 'funcionarios', loadComponent: () => import('./funcionarios/funcionario-list.component').then((m) => m.FuncionarioRhListComponent) },
  { path: 'funcionarios/novo', loadComponent: () => import('./funcionarios/funcionario-wizard.component').then((m) => m.FuncionarioWizardComponent) },
  { path: 'funcionarios/:id', loadComponent: () => import('./funcionarios/funcionario-ficha.component').then((m) => m.FuncionarioFichaComponent) },

  // W2 — rh-ponto-interno
  { path: 'ponto', loadChildren: () => import('./ponto/ponto.routes').then((m) => m.PONTO_ROUTES) },
];
