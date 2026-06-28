import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { environment } from '@env/environment';
import {
  Competencia, CompetenciaSelectorComponent, competenciaAtual,
} from '@shared/competencia/competencia-selector.component';

interface FluxoMovimento {
  data: string;
  tipo: 'Receita' | 'Despesa';
  descricao: string;
  valor: number;
  status: string;
  realizado: boolean;
}

interface FluxoResposta {
  inicio: string;
  fim: string;
  totalReceitas: number;
  totalDespesas: number;
  resultado: number;
  somenteRealizados: boolean;
  periodoFechado: boolean;
  movimentos: FluxoMovimento[];
}

@Component({
  selector: 'app-fluxo-caixa',
  standalone: true,
  imports: [CommonModule, FormsModule, CompetenciaSelectorComponent],
  template: `
    <h3>Fluxo de Caixa</h3>

    <div class="d-flex gap-3 align-items-center mb-3 flex-wrap">
      <app-competencia-selector (mudou)="onCompetencia($event)" />
      <div class="form-check ms-3">
        <input class="form-check-input" type="checkbox" id="somenteRealizados"
               [(ngModel)]="somenteRealizados" (ngModelChange)="carregar()" />
        <label class="form-check-label" for="somenteRealizados">Somente realizados</label>
      </div>
    </div>

    @if (fluxo(); as f) {
      <div class="row g-3 mb-3">
        <div class="col-md-3">
          <div class="card p-3 text-center">
            <small class="text-muted">Total Receitas previstas</small>
            <h4 class="text-success m-0">R$ {{ f.totalReceitas.toFixed(2) }}</h4>
          </div>
        </div>
        <div class="col-md-3">
          <div class="card p-3 text-center">
            <small class="text-muted">Total Despesas previstas</small>
            <h4 class="text-danger m-0">R$ {{ f.totalDespesas.toFixed(2) }}</h4>
          </div>
        </div>
        <div class="col-md-3">
          <div class="card p-3 text-center">
            <small class="text-muted">Resultado do período</small>
            <h4 class="m-0" [class.text-success]="f.resultado >= 0" [class.text-danger]="f.resultado < 0">
              R$ {{ f.resultado.toFixed(2) }}
            </h4>
          </div>
        </div>
        <div class="col-md-3">
          <div class="card p-3 text-center">
            <small class="text-muted">{{ f.somenteRealizados ? 'Considerando apenas' : 'Inclui' }}</small>
            <h6 class="m-0">{{ f.somenteRealizados ? 'Realizados' : 'Previstos + Realizados' }}</h6>
            @if (f.periodoFechado) {
              <small class="text-warning">Período fechado</small>
            }
          </div>
        </div>
      </div>

      <h5 class="mt-4">Movimentos da competência ({{ f.movimentos.length }})</h5>
      <div class="table-responsive">
        <table class="table table-sm table-hover">
          <thead class="table-light">
            <tr>
              <th>Data</th>
              <th>Tipo</th>
              <th>Descrição</th>
              <th>Status</th>
              <th class="text-end">Valor</th>
              <th class="text-end">Saldo acumulado</th>
            </tr>
          </thead>
          <tbody>
            @for (m of comSaldoAcumulado(); track $index) {
              <tr>
                <td>{{ m.data | date:'dd/MM/yyyy' }}</td>
                <td>
                  <span class="badge"
                        [class.bg-success]="m.tipo === 'Receita'"
                        [class.bg-danger]="m.tipo === 'Despesa'">
                    {{ m.tipo }}
                  </span>
                </td>
                <td>{{ m.descricao }}</td>
                <td>
                  <span class="badge"
                        [class.bg-secondary]="!m.realizado"
                        [class.bg-success]="m.realizado">
                    {{ m.status }}
                  </span>
                </td>
                <td class="text-end" [class.text-success]="m.tipo === 'Receita'"
                                       [class.text-danger]="m.tipo === 'Despesa'">
                  {{ m.tipo === 'Receita' ? '+' : '-' }} {{ m.valor.toFixed(2) }}
                </td>
                <td class="text-end" [class.text-success]="m.saldoAcumulado >= 0"
                                       [class.text-danger]="m.saldoAcumulado < 0">
                  {{ m.saldoAcumulado.toFixed(2) }}
                </td>
              </tr>
            } @empty {
              <tr><td colspan="6" class="text-center text-muted">Sem movimentos no período.</td></tr>
            }
          </tbody>
          <tfoot class="table-light">
            <tr>
              <th colspan="4" class="text-end">Total Entradas:</th>
              <th class="text-end text-success">+ {{ totalEntradas().toFixed(2) }}</th>
              <th></th>
            </tr>
            <tr>
              <th colspan="4" class="text-end">Total Saídas:</th>
              <th class="text-end text-danger">- {{ totalSaidas().toFixed(2) }}</th>
              <th></th>
            </tr>
            <tr>
              <th colspan="4" class="text-end">Saldo do período:</th>
              <th colspan="2" class="text-end"
                  [class.text-success]="saldoPeriodo() >= 0"
                  [class.text-danger]="saldoPeriodo() < 0">
                R$ {{ saldoPeriodo().toFixed(2) }}
              </th>
            </tr>
          </tfoot>
        </table>
      </div>
    } @else {
      <p class="text-muted">Selecione uma competência.</p>
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FluxoCaixaComponent implements OnInit {
  private readonly http = inject(HttpClient);
  readonly fluxo = signal<FluxoResposta | null>(null);
  readonly carregando = signal(false);

  competencia: Competencia = competenciaAtual();
  somenteRealizados = false;

  ngOnInit(): void { /* competencia-selector dispara no init */ }

  onCompetencia(c: Competencia): void {
    this.competencia = c;
    this.carregar();
  }

  carregar(): void {
    this.carregando.set(true);
    const params = new HttpParams()
      .set('inicio', this.competencia.inicio)
      .set('fim', this.competencia.fim)
      .set('somenteRealizados', String(this.somenteRealizados));

    this.http
      .get<FluxoResposta>(`${environment.apiUrl}/${environment.apiVersion}/fluxo-de-caixa`, { params })
      .subscribe({
        next: (r) => { this.fluxo.set(r); this.carregando.set(false); },
        error: () => this.carregando.set(false),
      });
  }

  /** Calcula saldo acumulado linha a linha (Receitas somam, Despesas subtraem). */
  comSaldoAcumulado(): (FluxoMovimento & { saldoAcumulado: number })[] {
    const movs = this.fluxo()?.movimentos ?? [];
    let saldo = 0;
    return movs.map((m) => {
      saldo += m.tipo === 'Receita' ? m.valor : -m.valor;
      return { ...m, saldoAcumulado: saldo };
    });
  }

  totalEntradas(): number {
    return (this.fluxo()?.movimentos ?? [])
      .filter((m) => m.tipo === 'Receita')
      .reduce((a, m) => a + m.valor, 0);
  }

  totalSaidas(): number {
    return (this.fluxo()?.movimentos ?? [])
      .filter((m) => m.tipo === 'Despesa')
      .reduce((a, m) => a + m.valor, 0);
  }

  saldoPeriodo(): number {
    return this.totalEntradas() - this.totalSaidas();
  }
}
