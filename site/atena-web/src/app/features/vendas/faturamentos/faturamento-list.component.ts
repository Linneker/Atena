import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import {
  AcaoLinha, ColunaTabela, DataTableComponent, OrdenacaoTabela, PaginaResultado,
} from '@shared/data-table/data-table.component';
import { Faturamento, FaturamentoService } from '../vendas.services';

@Component({
  selector: 'app-faturamento-list',
  standalone: true,
  imports: [DataTableComponent],
  template: `
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h3 class="m-0">Faturamentos</h3>
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
export class FaturamentoListComponent implements OnInit {
  private readonly servico = inject(FaturamentoService);
  private readonly router = inject(Router);

  readonly pagina = signal<PaginaResultado<Faturamento> | null>(null);
  private busca = '';
  private numeroPagina = 1;
  private ordenacao: OrdenacaoTabela | null = null;

  readonly colunas: ColunaTabela<Faturamento>[] = [
    { campo: 'numero', titulo: 'Número' },
    { campo: 'pedidoVendaId', titulo: 'Pedido (ID)' },
    { campo: 'dataFaturamento', titulo: 'Data', tipo: 'data' },
    { campo: 'tipo', titulo: 'Tipo' },
    { campo: 'valorTotal', titulo: 'Valor', tipo: 'moeda' },
  ];

  readonly acoes: AcaoLinha<Faturamento>[] = [
    {
      rotulo: 'Devolver',
      classe: 'btn-link text-warning',
      executar: (f) => this.router.navigateByUrl(`/vendas/devolucoes/registrar/${f.id}`),
    },
  ];

  ngOnInit(): void { this.recarregar(); }
  onBusca(t: string): void { this.busca = t; this.numeroPagina = 1; this.recarregar(); }
  onPagina(n: number): void { this.numeroPagina = n; this.recarregar(); }
  onOrdenacao(o: OrdenacaoTabela): void { this.ordenacao = o; this.recarregar(); }
  editar(f: Faturamento): void { this.router.navigateByUrl(`/vendas/faturamentos/${f.id}`); }

  private recarregar(): void {
    this.servico
      .listar({ pagina: this.numeroPagina, tamanhoPagina: 20, busca: this.busca, ordenacao: this.ordenacao })
      .subscribe((p) => this.pagina.set(p));
  }
}
