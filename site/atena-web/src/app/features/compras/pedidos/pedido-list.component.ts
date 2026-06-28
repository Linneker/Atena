import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import {
  AcaoLinha, ColunaTabela, DataTableComponent, OrdenacaoTabela, PaginaResultado,
} from '@shared/data-table/data-table.component';
import { PedidoCompra, PedidoCompraService } from '../compras.services';

@Component({
  selector: 'app-pedido-list',
  standalone: true,
  imports: [DataTableComponent],
  template: `
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h3 class="m-0">Pedidos de Compra</h3>
      <button class="btn btn-primary btn-sm" (click)="novo()">Novo</button>
    </div>
    <app-data-table
      [colunas]="colunas"
      [pagina]="pagina()"
      [acoes]="acoes"
      (paginaChange)="onPagina($event)"
      (buscaChange)="onBusca($event)"
      (ordenacaoChange)="onOrdenacao($event)"
      (editar)="editar($event)" />
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PedidoListComponent implements OnInit {
  private readonly servico = inject(PedidoCompraService);
  private readonly router = inject(Router);

  readonly pagina = signal<PaginaResultado<PedidoCompra> | null>(null);
  private busca = '';
  private numeroPagina = 1;
  private ordenacao: OrdenacaoTabela | null = null;

  readonly colunas: ColunaTabela<PedidoCompra>[] = [
    { campo: 'numero', titulo: 'Número' },
    { campo: 'fornecedorNome', titulo: 'Fornecedor', formato: (l) => l.fornecedorNome ?? '—' },
    { campo: 'dataEmissao', titulo: 'Emitido em', tipo: 'data' },
    { campo: 'previsaoEntrega', titulo: 'Previsão entrega', tipo: 'data' },
    { campo: 'valorTotal', titulo: 'Total', tipo: 'moeda' },
    { campo: 'status', titulo: 'Status' },
  ];

  readonly acoes: AcaoLinha<PedidoCompra>[] = [
    {
      rotulo: 'Enviar fornecedor',
      classe: 'btn-link text-primary',
      visivel: (l) => l.status === 'Rascunho',
      executar: (l) => this.enviar(l),
    },
    {
      rotulo: 'Receber',
      classe: 'btn-link text-success',
      visivel: (l) => l.status === 'EnviadoFornecedor' || l.status === 'ConfirmadoFornecedor' || l.status === 'RecebimentoParcial',
      executar: (l) => this.router.navigateByUrl(`/compras/recebimentos/registrar/${l.id}`),
    },
  ];

  ngOnInit(): void { this.recarregar(); }

  onBusca(t: string): void { this.busca = t; this.numeroPagina = 1; this.recarregar(); }
  onPagina(n: number): void { this.numeroPagina = n; this.recarregar(); }
  onOrdenacao(o: OrdenacaoTabela): void { this.ordenacao = o; this.recarregar(); }

  novo(): void { this.router.navigateByUrl('/compras/pedidos/novo'); }
  editar(p: PedidoCompra): void { this.router.navigateByUrl(`/compras/pedidos/${p.id}`); }

  enviar(p: PedidoCompra): void {
    if (!p.id) return;
    const email = prompt('E-mail destino (opcional, deixe em branco para usar o cadastrado no fornecedor):', '');
    this.servico.enviarFornecedor(p.id, email || null).subscribe({
      next: () => { alert(`Pedido ${p.numero} enviado ao fornecedor.`); this.recarregar(); },
      error: (e) => alert(e?.error?.message ?? 'Falha ao enviar pedido.'),
    });
  }

  private recarregar(): void {
    this.servico
      .listar({ pagina: this.numeroPagina, tamanhoPagina: 20, busca: this.busca, ordenacao: this.ordenacao })
      .subscribe((p) => this.pagina.set(p));
  }
}
