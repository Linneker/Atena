import { Routes } from '@angular/router';

export const CONFIGURACAO_ROUTES: Routes = [
  { path: '', redirectTo: 'usuarios', pathMatch: 'full' },
  { path: 'usuarios', loadComponent: () => import('./usuarios/usuario-list.component').then((m) => m.UsuarioListComponent) },
  { path: 'usuarios/:id', loadComponent: () => import('./usuarios/usuario-form.component').then((m) => m.UsuarioFormComponent) },
  { path: 'roles', loadComponent: () => import('./roles/role-list.component').then((m) => m.RoleListComponent) },
  { path: 'roles/:id', loadComponent: () => import('./roles/role-form.component').then((m) => m.RoleFormComponent) },
  { path: 'permissoes', loadComponent: () => import('./roles/permissoes.component').then((m) => m.PermissoesComponent) },
  { path: 'parametros', loadComponent: () => import('./parametros/parametros.component').then((m) => m.ParametrosComponent) },
  { path: 'branding', loadComponent: () => import('./branding.component').then((m) => m.BrandingComponent) },
  { path: 'tenants', loadComponent: () => import('./tenants/tenant-list.component').then((m) => m.TenantListComponent) },
  { path: 'tenants/:id', loadComponent: () => import('./tenants/tenant-form.component').then((m) => m.TenantFormComponent) },
];
