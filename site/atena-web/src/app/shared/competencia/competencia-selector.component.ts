import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormsModule } from '@angular/forms';

export interface Competencia {
  ano: number;
  mes: number;
  /** Primeiro dia do mês (yyyy-mm-dd). */
  inicio: string;
  /** Último dia do mês (yyyy-mm-dd). */
  fim: string;
  /** Rótulo amigável (ex: "Maio/2026"). */
  rotulo: string;
}

const MESES = [
  'Jan', 'Fev', 'Mar', 'Abr', 'Mai', 'Jun',
  'Jul', 'Ago', 'Set', 'Out', 'Nov', 'Dez',
];

const MESES_LONGOS = [
  'Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho',
  'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro',
];

export function construirCompetencia(ano: number, mes: number): Competencia {
  const inicio = new Date(ano, mes - 1, 1);
  const fim = new Date(ano, mes, 0);
  const fmt = (d: Date) => d.toISOString().slice(0, 10);
  return {
    ano, mes,
    inicio: fmt(inicio),
    fim: fmt(fim),
    rotulo: `${MESES_LONGOS[mes - 1]}/${ano}`,
  };
}

export function competenciaAtual(): Competencia {
  const agora = new Date();
  return construirCompetencia(agora.getFullYear(), agora.getMonth() + 1);
}

@Component({
  selector: 'app-competencia-selector',
  standalone: true,
  imports: [FormsModule],
  template: `
    <div class="d-flex gap-2 align-items-center">
      <strong class="me-2">Competência:</strong>
      <button class="btn btn-sm btn-outline-secondary" type="button" (click)="anterior()">&laquo;</button>
      <select class="form-select form-select-sm" style="width:auto" [(ngModel)]="mesSelecionado" (ngModelChange)="emitir()">
        @for (m of meses; track $index) {
          <option [value]="$index + 1">{{ m }}</option>
        }
      </select>
      <input type="number" class="form-control form-control-sm" style="width:90px"
             [(ngModel)]="anoSelecionado" (ngModelChange)="emitir()" />
      <button class="btn btn-sm btn-outline-secondary" type="button" (click)="proximo()">&raquo;</button>
      <button class="btn btn-sm btn-link" type="button" (click)="hoje()">Hoje</button>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CompetenciaSelectorComponent implements OnInit {
  @Input() valorInicial?: Competencia;
  @Output() readonly mudou = new EventEmitter<Competencia>();

  readonly meses = MESES_LONGOS;
  mesSelecionado = 1;
  anoSelecionado = new Date().getFullYear();

  ngOnInit(): void {
    const c = this.valorInicial ?? competenciaAtual();
    this.mesSelecionado = c.mes;
    this.anoSelecionado = c.ano;
    this.emitir();
  }

  anterior(): void {
    let m = this.mesSelecionado - 1;
    let a = this.anoSelecionado;
    if (m < 1) { m = 12; a--; }
    this.mesSelecionado = m;
    this.anoSelecionado = a;
    this.emitir();
  }

  proximo(): void {
    let m = this.mesSelecionado + 1;
    let a = this.anoSelecionado;
    if (m > 12) { m = 1; a++; }
    this.mesSelecionado = m;
    this.anoSelecionado = a;
    this.emitir();
  }

  hoje(): void {
    const c = competenciaAtual();
    this.mesSelecionado = c.mes;
    this.anoSelecionado = c.ano;
    this.emitir();
  }

  emitir(): void {
    this.mudou.emit(construirCompetencia(this.anoSelecionado, this.mesSelecionado));
  }
}
