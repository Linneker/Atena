import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthStore } from '@core/auth/auth.store';
import { TemPermissaoDirective } from '@core/permissions/tem-permissao.directive';
import { TenantBrandingService } from '@core/branding/tenant-branding.service';
import { NotificacaoService } from '@core/notifications/notificacao.service';
import { NotificacaoBellComponent } from '@core/notifications/notificacao-bell.component';

@Component({
  selector: 'app-default-layout',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, TemPermissaoDirective, NotificacaoBellComponent],
  template: `
    <div class="app-shell d-flex" style="min-height:100vh">
      <aside class="sidebar bg-dark text-white p-3" [class.open]="menuOpen()" style="width:240px">
        <div class="text-center mb-3">
          @if (branding.branding()?.logoUrl; as logo) {
            <img [src]="logo" alt="logo" class="tenant-logo" />
          } @else {
            <strong>{{ branding.branding()?.razaoSocial ?? 'Atena ERP' }}</strong>
          }
        </div>
        <nav class="nav flex-column">
          <a class="nav-link text-white" routerLink="/dashboard" routerLinkActive="active">Dashboard</a>
          <a *temPermissao="'financeiro.visualizar'" class="nav-link text-white"
             routerLink="/financeiro" routerLinkActive="active">Financeiro</a>
          <a *temPermissao="'cadastros.visualizar'" class="nav-link text-white"
             routerLink="/cadastros" routerLinkActive="active">Cadastros</a>
          <a *temPermissao="'estoque.visualizar'" class="nav-link text-white"
             routerLink="/estoque" routerLinkActive="active">Estoque</a>
          <a *temPermissao="'compras.visualizar'" class="nav-link text-white"
             routerLink="/compras" routerLinkActive="active">Compras</a>
          <a *temPermissao="'vendas.visualizar'" class="nav-link text-white"
             routerLink="/vendas" routerLinkActive="active">Vendas</a>
          <a *temPermissao="'fiscal.visualizar'" class="nav-link text-white"
             routerLink="/fiscal" routerLinkActive="active">Fiscal</a>
          <a *temPermissao="'relatorios.visualizar'" class="nav-link text-white"
             routerLink="/relatorios" routerLinkActive="active">Relatórios</a>
          <a *temPermissao="'configuracao.visualizar'" class="nav-link text-white"
             routerLink="/configuracao" routerLinkActive="active">Configuração</a>
        </nav>
      </aside>
      <div class="flex-grow-1 d-flex flex-column">
        <header class="d-flex align-items-center px-3 py-2 bg-white border-bottom">
          <button type="button" class="btn btn-sm btn-outline-secondary d-md-none me-2"
                  (click)="menuOpen.set(!menuOpen())">☰</button>
          <h5 class="m-0 flex-grow-1">{{ branding.branding()?.razaoSocial ?? 'Atena ERP' }}</h5>
          <app-notificacao-bell class="me-3" />
          <span class="me-3">{{ auth.user()?.nome }}</span>
          <button class="btn btn-sm btn-outline-danger" (click)="auth.logout()">Sair</button>
        </header>
        <main class="p-3 flex-grow-1">
          <router-outlet />
        </main>
      </div>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DefaultLayoutComponent implements OnInit {
  readonly auth = inject(AuthStore);
  readonly branding = inject(TenantBrandingService);
  private readonly notificacoes = inject(NotificacaoService);
  readonly menuOpen = signal(false);

  ngOnInit(): void {
    this.notificacoes.iniciarPolling();
  }
}
