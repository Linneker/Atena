import { Routes } from '@angular/router';

export const PONTO_ROUTES: Routes = [
  { path: '', redirectTo: 'meu-ponto', pathMatch: 'full' },
  { path: 'meu-ponto', loadComponent: () => import('./meu-ponto/meu-ponto.component').then((m) => m.MeuPontoComponent) },
  { path: 'espelho', loadComponent: () => import('./espelho/espelho-mensal.component').then((m) => m.EspelhoMensalComponent) },
  { path: 'aprovacoes', loadComponent: () => import('./aprovacoes/aprovacoes-pendentes.component').then((m) => m.AprovacoesPendentesComponent) },
  { path: 'banco-horas', loadComponent: () => import('./banco-horas/banco-horas.component').then((m) => m.BancoHorasComponent) },
  { path: 'politicas', loadComponent: () => import('./banco-horas/politicas-list.component').then((m) => m.PoliticasListComponent) },
  { path: 'fechamento', loadComponent: () => import('./fechamento/fechamento.component').then((m) => m.FechamentoComponent) },
  { path: '671/configuracao', loadComponent: () => import('./oficial-671/configuracao-rep.component').then((m) => m.ConfiguracaoRepComponent) },
  { path: '671/diagnostico', loadComponent: () => import('./oficial-671/auto-diagnostico.component').then((m) => m.AutoDiagnosticoRepComponent) },
  { path: '671/exportar', loadComponent: () => import('./oficial-671/exportar-afd-aej.component').then((m) => m.ExportarAfdAejComponent) },
];
