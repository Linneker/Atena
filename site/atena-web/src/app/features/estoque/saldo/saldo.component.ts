import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { DataTableComponent, ColunaTabela, PaginaResultado } from '@shared/data-table/data-table.component';
import { SaldoEstoque, SaldoEstoqueService } from '../estoque.services';

@Component({
  selector: 'app-saldo',
  standalone: true,
  imports: [DataTableComponent],
  template: `
    <h3 class="mb-3">Saldo de Estoque</h3>
    <app-data-table [colunas]="colunas" [pagina]="pagina()"
                    (buscaChange)="onBusca($event)" (paginaChange)="onPagina($event)" />
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SaldoComponent implements OnInit {
  private readonly servico = inject(SaldoEstoqueService);
  readonly pagina = signal<PaginaResultado<SaldoEstoque> | null>(null);
  readonly colunas: ColunaTabela<SaldoEstoque>[] = [
    { campo: 'codigo', titulo: 'Código' },
    { campo: 'descricao', titulo: 'Descrição' },
    { campo: 'saldoTotal', titulo: 'Total' },
    { campo: 'saldoReservado', titulo: 'Reservado' },
    { campo: 'saldoDisponivel', titulo: 'Disponível' },
  ];
  private busca = '';
  private numeroPagina = 1;

  ngOnInit(): void { this.recarregar(); }
  onBusca(termo: string) { this.busca = termo; this.numeroPagina = 1; this.recarregar(); }
  onPagina(num: number) { this.numeroPagina = num; this.recarregar(); }
  private recarregar() { this.servico.consultar(this.busca, this.numeroPagina).subscribe((p) => this.pagina.set(p)); }
}
