import { ChangeDetectionStrategy, Component, OnInit, computed, inject, signal } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthStore } from '@core/auth/auth.store';
import { TemPermissaoDirective } from '@core/permissions/tem-permissao.directive';
import { TenantBrandingService } from '@core/branding/tenant-branding.service';
import { NotificacaoService } from '@core/notifications/notificacao.service';
import { NotificacaoBellComponent } from '@core/notifications/notificacao-bell.component';

interface MenuItem {
  rotulo: string;
  rota: string;
}

interface MenuAreaDef {
  chave: string;
  rotulo: string;
  rotaBase: string;
  permissoes: string[];
  filhos: MenuItem[];
}

@Component({
  selector: 'app-default-layout',
  standalone: true,
  imports: [RouterOutlet, RouterLink, RouterLinkActive, TemPermissaoDirective, NotificacaoBellComponent],
  template: `
    <div class="app-shell d-flex" style="min-height:100vh">
      <aside class="sidebar bg-dark text-white p-3" [class.open]="menuOpen()" style="width:260px">
        <div class="text-center mb-3">
          @if (branding.branding()?.logoUrl; as logo) {
            <img [src]="logo" alt="logo" class="tenant-logo" />
          } @else {
            <strong>{{ branding.branding()?.razaoSocial ?? 'Atena ERP' }}</strong>
          }
        </div>
        <nav class="nav flex-column">
          <a class="nav-link text-white" routerLink="/dashboard" routerLinkActive="active">Dashboard</a>

          @for (area of areas; track area.chave) {
            <ng-container *temPermissao="area.permissoes">
              <button type="button"
                      class="nav-link text-white d-flex justify-content-between align-items-center bg-transparent border-0 text-start"
                      (click)="alternar(area.chave)">
                <span>{{ area.rotulo }}</span>
                <span class="small">{{ areaAberta() === area.chave ? '▾' : '▸' }}</span>
              </button>
              @if (areaAberta() === area.chave) {
                <div class="ms-3 d-flex flex-column border-start border-secondary ps-2">
                  @for (it of area.filhos; track it.rota) {
                    <a class="nav-link text-white-50 py-1"
                       [routerLink]="it.rota"
                       routerLinkActive="active text-white">{{ it.rotulo }}</a>
                  }
                </div>
              }
            </ng-container>
          }

          <a *temPermissao="permTenants" class="nav-link text-white"
             routerLink="/configuracao/tenants" routerLinkActive="active">Tenants</a>
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
  private readonly router = inject(Router);

  readonly menuOpen = signal(false);
  readonly areaAberta = signal<string | null>(null);

  readonly permTenants = ['tenant:criar', 'tenant:editar', 'tenant:excluir'];

  readonly areas: MenuAreaDef[] = [
    {
      chave: 'financeiro', rotulo: 'Financeiro', rotaBase: '/financeiro',
      permissoes: ['despesa:ler', 'receita:ler', 'conta-pagar:ler', 'conta-receber:ler',
                   'fluxo-de-caixa:ler', 'conciliacao-bancaria:ler', 'plano-de-contas:ler', 'centro-de-custo:ler'],
      filhos: [
        { rotulo: 'Despesas', rota: '/financeiro/despesas' },
        { rotulo: 'Receitas', rota: '/financeiro/receitas' },
        { rotulo: 'Contas a Pagar', rota: '/financeiro/contas-pagar' },
        { rotulo: 'Contas a Receber', rota: '/financeiro/contas-receber' },
        { rotulo: 'Fluxo de Caixa', rota: '/financeiro/fluxo-caixa' },
        { rotulo: 'Conciliação', rota: '/financeiro/conciliacao' },
      ],
    },
    {
      chave: 'cadastros', rotulo: 'Cadastros', rotaBase: '/cadastros',
      permissoes: ['cliente:ler', 'fornecedor:ler', 'funcionario:ler', 'empresa:ler', 'produto:ler'],
      filhos: [
        { rotulo: 'Clientes', rota: '/cadastros/clientes' },
        { rotulo: 'Fornecedores', rota: '/cadastros/fornecedores' },
        { rotulo: 'Funcionários', rota: '/cadastros/funcionarios' },
        { rotulo: 'Produtos', rota: '/cadastros/produtos' },
        { rotulo: 'Centros de Custo', rota: '/cadastros/centros-custo' },
        { rotulo: 'Plano de Contas', rota: '/cadastros/plano-contas' },
      ],
    },
    {
      chave: 'rh', rotulo: 'RH', rotaBase: '/rh',
      permissoes: ['rh-funcionario:ler', 'rh-jornada:ler', 'rh-cargo:ler',
                   'rh-lotacao:ler', 'rh-departamento:ler', 'rh-beneficio:ler'],
      filhos: [
        { rotulo: 'Funcionários', rota: '/rh/funcionarios' },
        { rotulo: 'Cargos', rota: '/rh/cargos' },
        { rotulo: 'Departamentos', rota: '/rh/departamentos' },
        { rotulo: 'Lotações', rota: '/rh/lotacoes' },
        { rotulo: 'Jornadas', rota: '/rh/jornadas' },
        { rotulo: 'Catálogo de Benefícios', rota: '/rh/beneficios' },
      ],
    },
    {
      chave: 'estoque', rotulo: 'Estoque', rotaBase: '/estoque',
      permissoes: ['produto:ler', 'tipo-produto:ler', 'estoque:ler', 'inventario:ler'],
      filhos: [
        { rotulo: 'Saldo', rota: '/estoque/saldo' },
        { rotulo: 'Movimentação', rota: '/estoque/movimentacao' },
        { rotulo: 'Inventário', rota: '/estoque/inventario' },
      ],
    },
    {
      chave: 'compras', rotulo: 'Compras', rotaBase: '/compras',
      permissoes: ['solicitacao-compra:ler', 'pedido-compra:ler', 'recebimento:ler'],
      filhos: [
        { rotulo: 'Solicitações', rota: '/compras/solicitacoes' },
        { rotulo: 'Pedidos', rota: '/compras/pedidos' },
        { rotulo: 'Recebimentos', rota: '/compras/recebimentos' },
      ],
    },
    {
      chave: 'vendas', rotulo: 'Vendas', rotaBase: '/vendas',
      permissoes: ['orcamento:ler', 'pedido-venda:ler', 'faturamento:ler', 'devolucao:ler'],
      filhos: [
        { rotulo: 'Orçamentos', rota: '/vendas/orcamentos' },
        { rotulo: 'Pedidos', rota: '/vendas/pedidos' },
        { rotulo: 'Faturamentos', rota: '/vendas/faturamentos' },
        { rotulo: 'Devoluções', rota: '/vendas/devolucoes' },
      ],
    },
    {
      chave: 'fiscal', rotulo: 'Fiscal', rotaBase: '/fiscal',
      permissoes: ['nfe:ler', 'configuracao-fiscal:ler'],
      filhos: [
        { rotulo: 'NF-e', rota: '/fiscal/nfe' },
        { rotulo: 'Configuração Fiscal', rota: '/fiscal/configuracao' },
      ],
    },
    {
      chave: 'relatorios', rotulo: 'Relatórios', rotaBase: '/relatorios',
      permissoes: ['relatorio:ler'],
      filhos: [
        { rotulo: 'Indicadores', rota: '/relatorios/dashboard' },
        { rotulo: 'DRE', rota: '/relatorios/dre' },
        { rotulo: 'Balanço', rota: '/relatorios/balanco' },
        { rotulo: 'Aging Contas a Pagar', rota: '/relatorios/aging-pagar' },
        { rotulo: 'Aging Contas a Receber', rota: '/relatorios/aging-receber' },
        { rotulo: 'Vendas (relatório)', rota: '/relatorios/vendas' },
        { rotulo: 'Estoque (relatório)', rota: '/relatorios/estoque' },
      ],
    },
    {
      chave: 'configuracao', rotulo: 'Configuração', rotaBase: '/configuracao',
      permissoes: ['tenant:ler', 'usuario:ler', 'role:ler', 'permissao:ler',
                   'feature-flags:ler', 'api-key:ler', 'auditoria:ler'],
      filhos: [
        { rotulo: 'Usuários', rota: '/configuracao/usuarios' },
        { rotulo: 'Roles', rota: '/configuracao/roles' },
        { rotulo: 'Permissões', rota: '/configuracao/permissoes' },
        { rotulo: 'Parâmetros', rota: '/configuracao/parametros' },
        { rotulo: 'Branding', rota: '/configuracao/branding' },
      ],
    },
  ];

  alternar(chave: string): void {
    const area = this.areas.find((a) => a.chave === chave);
    if (!area) return;
    const ja = this.areaAberta() === chave;
    this.areaAberta.set(ja ? null : chave);
    if (!ja) {
      this.router.navigateByUrl(area.rotaBase);
    }
  }

  ngOnInit(): void {
    this.notificacoes.iniciarPolling();
    const url = this.router.url;
    const match = this.areas.find((a) => url.startsWith(a.rotaBase));
    if (match) this.areaAberta.set(match.chave);
  }
}
