import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import {
  AcaoLinha, ColunaTabela, DataTableComponent, OrdenacaoTabela, PaginaResultado,
} from '@shared/data-table/data-table.component';
import { PedidoVenda, PedidoVendaService } from '../vendas.services';

@Component({
  selector: 'app-pedido-venda-list',
  standalone: true,
  imports: [DataTableComponent],
  template: `
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h3 class="m-0">Pedidos de Venda</h3>
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
export class PedidoVendaListComponent implements OnInit {
  private readonly servico = inject(PedidoVendaService);
  private readonly router = inject(Router);

  readonly pagina = signal<PaginaResultado<PedidoVenda> | null>(null);
  private busca = '';
  private numeroPagina = 1;
  private ordenacao: OrdenacaoTabela | null = null;

  readonly colunas: ColunaTabela<PedidoVenda>[] = [
    { campo: 'numero', titulo: 'Número' },
    { campo: 'clienteNome', titulo: 'Cliente', formato: (l) => l.clienteNome ?? '—' },
    { campo: 'vendedorNome', titulo: 'Vendedor', formato: (l) => l.vendedorNome ?? '—' },
    { campo: 'dataEmissao', titulo: 'Emitido em', tipo: 'data' },
    { campo: 'valorTotal', titulo: 'Total', tipo: 'moeda' },
    { campo: 'status', titulo: 'Status' },
  ];

  readonly acoes: AcaoLinha<PedidoVenda>[] = [
    {
      rotulo: 'Confirmar',
      classe: 'btn-link text-primary',
      visivel: (l) => l.status === 'Rascunho',
      executar: (l) => this.confirmar(l),
    },
    {
      rotulo: 'Faturar',
      classe: 'btn-link text-success',
      visivel: (l) => l.status === 'Confirmado' || l.status === 'FaturamentoParcial',
      executar: (l) => this.router.navigateByUrl(`/vendas/pedidos/${l.id}/faturar`),
    },
  ];

  ngOnInit(): void { this.recarregar(); }

  onBusca(t: string): void { this.busca = t; this.numeroPagina = 1; this.recarregar(); }
  onPagina(n: number): void { this.numeroPagina = n; this.recarregar(); }
  onOrdenacao(o: OrdenacaoTabela): void { this.ordenacao = o; this.recarregar(); }

  novo(): void { this.router.navigateByUrl('/vendas/pedidos/novo'); }
  editar(p: PedidoVenda): void { this.router.navigateByUrl(`/vendas/pedidos/${p.id}`); }

  confirmar(p: PedidoVenda): void {
    if (!p.id) return;
    if (!confirm(`Confirmar o pedido ${p.numero}? Após confirmado o estoque será reservado e o pedido poderá ser faturado.`)) return;
    this.servico.confirmar(p.id).subscribe({
      next: () => this.recarregar(),
      error: (e) => alert(e?.error?.message ?? 'Falha ao confirmar pedido.'),
    });
  }

  private recarregar(): void {
    this.servico
      .listar({ pagina: this.numeroPagina, tamanhoPagina: 20, busca: this.busca, ordenacao: this.ordenacao })
      .subscribe((p) => this.pagina.set(p));
  }
}
