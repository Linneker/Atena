import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Subject, debounceTime, distinctUntilChanged } from 'rxjs';

export interface ColunaTabela<T> {
  campo: keyof T & string;
  titulo: string;
  ordenavel?: boolean;
  formato?: (linha: T) => string;
}

export interface OrdenacaoTabela {
  campo: string;
  direcao: 'asc' | 'desc';
}

export interface PaginaResultado<T> {
  itens: T[];
  total: number;
  pagina: number;
  tamanhoPagina: number;
}

@Component({
  selector: 'app-data-table',
  standalone: true,
  imports: [FormsModule],
  template: `
    <div class="d-flex mb-2">
      <input class="form-control form-control-sm me-2" placeholder="Buscar..."
             [(ngModel)]="termoBusca" (ngModelChange)="onBusca($event)" />
      <button class="btn btn-sm btn-outline-secondary" (click)="exportarExcel.emit()">Exportar</button>
    </div>
    <div class="table-responsive data-table">
      <table class="table table-sm table-hover">
        <thead class="table-light">
          <tr>
            @for (c of colunas; track c.campo) {
              <th [class.sortable]="c.ordenavel !== false"
                  (click)="c.ordenavel !== false && toggleOrdenacao(c.campo)"
                  [style.cursor]="c.ordenavel !== false ? 'pointer' : 'default'">
                {{ c.titulo }}
                @if (ordenacao()?.campo === c.campo) {
                  <span>{{ ordenacao()!.direcao === 'asc' ? '▲' : '▼' }}</span>
                }
              </th>
            }
            <th></th>
          </tr>
        </thead>
        <tbody>
          @for (linha of pagina?.itens ?? []; track linha) {
            <tr>
              @for (c of colunas; track c.campo) {
                <td>{{ c.formato ? c.formato(linha) : (linha[c.campo] ?? '') }}</td>
              }
              <td class="text-end">
                <button class="btn btn-sm btn-link" (click)="editar.emit(linha)">Editar</button>
              </td>
            </tr>
          } @empty {
            <tr><td [attr.colspan]="colunas.length + 1" class="text-center text-muted">Nenhum registro</td></tr>
          }
        </tbody>
      </table>
    </div>
    <div class="d-flex justify-content-between align-items-center">
      <small>Total: {{ pagina?.total ?? 0 }}</small>
      <div>
        <button class="btn btn-sm btn-outline-secondary me-1" [disabled]="(pagina?.pagina ?? 1) <= 1"
                (click)="paginaChange.emit((pagina?.pagina ?? 1) - 1)">Anterior</button>
        <span class="mx-2">{{ pagina?.pagina ?? 1 }}</span>
        <button class="btn btn-sm btn-outline-secondary" [disabled]="!hasNext()"
                (click)="paginaChange.emit((pagina?.pagina ?? 1) + 1)">Próxima</button>
      </div>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DataTableComponent<T> {
  @Input({ required: true }) colunas!: ColunaTabela<T>[];
  @Input() pagina: PaginaResultado<T> | null = null;
  @Output() readonly buscaChange = new EventEmitter<string>();
  @Output() readonly paginaChange = new EventEmitter<number>();
  @Output() readonly ordenacaoChange = new EventEmitter<OrdenacaoTabela>();
  @Output() readonly editar = new EventEmitter<T>();
  @Output() readonly exportarExcel = new EventEmitter<void>();

  termoBusca = '';
  readonly ordenacao = signal<OrdenacaoTabela | null>(null);
  private readonly busca$ = new Subject<string>();

  constructor() {
    this.busca$.pipe(debounceTime(300), distinctUntilChanged()).subscribe((v) => this.buscaChange.emit(v));
  }

  onBusca(valor: string): void {
    this.busca$.next(valor);
  }

  toggleOrdenacao(campo: string): void {
    const atual = this.ordenacao();
    const proxima: OrdenacaoTabela =
      atual?.campo === campo
        ? { campo, direcao: atual.direcao === 'asc' ? 'desc' : 'asc' }
        : { campo, direcao: 'asc' };
    this.ordenacao.set(proxima);
    this.ordenacaoChange.emit(proxima);
  }

  hasNext(): boolean {
    if (!this.pagina) return false;
    return this.pagina.pagina * this.pagina.tamanhoPagina < this.pagina.total;
  }
}
