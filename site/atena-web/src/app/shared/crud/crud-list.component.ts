import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import * as XLSX from 'xlsx';
import { ColunaTabela, DataTableComponent, OrdenacaoTabela, PaginaResultado } from '../data-table/data-table.component';
import { CrudService } from './crud.service';

@Component({
  selector: 'app-crud-list',
  standalone: true,
  imports: [DataTableComponent],
  template: `
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h3 class="m-0">{{ titulo }}</h3>
      <button class="btn btn-primary btn-sm" (click)="novo()">Novo</button>
    </div>
    <app-data-table
      [colunas]="colunas"
      [pagina]="pagina()"
      (buscaChange)="onBusca($event)"
      (paginaChange)="onPagina($event)"
      (ordenacaoChange)="onOrdenacao($event)"
      (editar)="editar($event)"
      (exportarExcel)="exportar()" />
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CrudListComponent<T extends { id?: string }> implements OnInit {
  @Input({ required: true }) titulo = '';
  @Input({ required: true }) colunas: ColunaTabela<T>[] = [];
  @Input({ required: true }) servico!: CrudService<T>;
  @Input({ required: true }) rotaForm = '';
  @Output() readonly editarItem = new EventEmitter<T>();

  private readonly router = inject(Router);
  readonly pagina = signal<PaginaResultado<T> | null>(null);

  private busca = '';
  private numeroPagina = 1;
  private ordenacao: OrdenacaoTabela | null = null;

  ngOnInit(): void {
    this.recarregar();
  }

  onBusca(termo: string): void {
    this.busca = termo;
    this.numeroPagina = 1;
    this.recarregar();
  }

  onPagina(num: number): void {
    this.numeroPagina = num;
    this.recarregar();
  }

  onOrdenacao(o: OrdenacaoTabela): void {
    this.ordenacao = o;
    this.numeroPagina = 1;
    this.recarregar();
  }

  novo(): void {
    this.router.navigateByUrl(`${this.rotaForm}/novo`);
  }

  editar(linha: T): void {
    this.router.navigateByUrl(`${this.rotaForm}/${linha.id}`);
  }

  exportar(): void {
    const itens = this.pagina()?.itens ?? [];
    const ws = XLSX.utils.json_to_sheet(itens);
    const wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, 'Dados');
    XLSX.writeFile(wb, `${this.titulo}.xlsx`);
  }

  private recarregar(): void {
    this.servico
      .listar({ pagina: this.numeroPagina, tamanhoPagina: 20, busca: this.busca, ordenacao: this.ordenacao })
      .subscribe((p) => this.pagina.set(p));
  }
}
