import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output, signal } from '@angular/core';
import { DatePipe } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Subject, debounceTime, distinctUntilChanged } from 'rxjs';

export interface ColunaTabela<T> {
  campo: keyof T & string;
  titulo: string;
  ordenavel?: boolean;
  /** Renderização customizada. Tem precedência sobre `tipo`. */
  formato?: (linha: T) => string;
  /**
   * Hint de tipo para renderização padrão. Quando definido e `formato` não está,
   * a célula formata o valor conforme o tipo.
   *  - 'data'       → dd/MM/yyyy
   *  - 'dataHora'   → dd/MM/yyyy HH:mm
   *  - 'moeda'      → valor com 2 decimais
   */
  tipo?: 'data' | 'dataHora' | 'moeda';
}

export interface OrdenacaoTabela {
  campo: string;
  direcao: 'asc' | 'desc';
}

/** Ação extra por linha (botão na coluna de ações, ao lado de Editar). */
export interface AcaoLinha<T> {
  rotulo: string;
  /** Classe Bootstrap do botão. Default: 'btn-link'. */
  classe?: string;
  /** Mostra ou esconde o botão por linha. Default: sempre visível. */
  visivel?: (linha: T) => boolean;
  executar: (linha: T) => void;
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
  providers: [DatePipe],
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
                <td>{{ renderCelula(c, linha) }}</td>
              }
              <td class="text-end">
                @for (a of acoes; track a.rotulo) {
                  @if (!a.visivel || a.visivel(linha)) {
                    <button class="btn btn-sm {{ a.classe ?? 'btn-link' }}" (click)="a.executar(linha)">{{ a.rotulo }}</button>
                  }
                }
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
  @Input() acoes: AcaoLinha<T>[] = [];
  @Output() readonly buscaChange = new EventEmitter<string>();
  @Output() readonly paginaChange = new EventEmitter<number>();
  @Output() readonly ordenacaoChange = new EventEmitter<OrdenacaoTabela>();
  @Output() readonly editar = new EventEmitter<T>();
  @Output() readonly exportarExcel = new EventEmitter<void>();

  termoBusca = '';
  readonly ordenacao = signal<OrdenacaoTabela | null>(null);
  private readonly busca$ = new Subject<string>();

  constructor(private readonly datePipe: DatePipe) {
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

  renderCelula(coluna: ColunaTabela<T>, linha: T): string {
    if (coluna.formato) return coluna.formato(linha);
    const valor = linha[coluna.campo];
    if (valor === null || valor === undefined || valor === '') return '';
    switch (coluna.tipo) {
      case 'data':
        return this.datePipe.transform(valor as string, 'dd/MM/yyyy') ?? '';
      case 'dataHora':
        return this.datePipe.transform(valor as string, 'dd/MM/yyyy HH:mm') ?? '';
      case 'moeda':
        return (Number(valor) || 0).toFixed(2);
      default:
        return String(valor);
    }
  }
}
