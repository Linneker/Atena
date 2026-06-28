import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PontoService, EspelhoMensal } from '../ponto.services';

@Component({
  selector: 'app-espelho-mensal',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <h3>Espelho mensal</h3>
    <div class="row g-3 mb-3">
      <div class="col-md-4"><label>Funcionário (UUID)</label>
        <input class="form-control" [(ngModel)]="funcionarioId" /></div>
      <div class="col-md-3"><label>Competência (YYYY-MM)</label>
        <input class="form-control" [(ngModel)]="competencia" /></div>
      <div class="col-md-5 d-flex align-items-end gap-2">
        <button class="btn btn-primary" (click)="carregar()" [disabled]="carregando()">Carregar</button>
        <button class="btn btn-outline-secondary" (click)="baixarPdf()"
                [disabled]="!espelho()">Baixar PDF</button>
      </div>
    </div>

    @if (erro()) { <div class="alert alert-danger">{{ erro() }}</div> }
    @if (espelho(); as e) {
      <p>
        <strong>{{ e.funcionarioNome }}</strong> — Competência {{ e.competencia }} —
        Jornada {{ e.jornadaVigente.nome }} ({{ e.jornadaVigente.cargaSemanal }}h/sem)
      </p>
      <p>
        Trabalhado: <strong>{{ formatMin(e.totais.trabalhadoMinutos) }}</strong> |
        Esperado: {{ formatMin(e.totais.esperadoMinutos) }} |
        Saldo mês: <strong [class]="e.totais.saldoMesMinutos >= 0 ? 'text-success' : 'text-danger'">
          {{ formatMinSign(e.totais.saldoMesMinutos) }}
        </strong> |
        Banco acumulado: {{ formatMinSign(e.totais.saldoBancoAcumuladoMinutos) }}
      </p>

      <table class="table table-sm table-bordered">
        <thead><tr>
          <th>Data</th><th>Dia</th><th>Esperada</th><th>Batidas</th>
          <th>Trabalhado</th><th>Saldo</th><th>Anomalias</th>
        </tr></thead>
        <tbody>
          <tr *ngFor="let d of e.dias" [class.table-warning]="d.ehFeriado"
              [class.table-light]="!d.ehDiaUtil">
            <td>{{ d.data | date:'dd/MM' }}</td>
            <td>{{ d.diaSemana }} {{ d.ehFeriado ? '★' : '' }}</td>
            <td>{{ d.janelaEsperadaEntrada }} – {{ d.janelaEsperadaSaida }}</td>
            <td>{{ batidasStr(d) }}</td>
            <td>{{ formatMin(d.trabalhadoMinutos) }}</td>
            <td [class]="d.saldoMinutos >= 0 ? 'text-success' : 'text-danger'">
              {{ formatMinSign(d.saldoMinutos) }}
            </td>
            <td class="text-muted small">{{ (d.anomalias || []).join('; ') }}</td>
          </tr>
        </tbody>
      </table>
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EspelhoMensalComponent {
  private readonly svc = inject(PontoService);
  readonly espelho = signal<EspelhoMensal | null>(null);
  readonly carregando = signal(false);
  readonly erro = signal<string | null>(null);

  funcionarioId = '';
  competencia = new Date().toISOString().slice(0, 7);

  carregar(): void {
    if (!this.funcionarioId) { this.erro.set('Informe o funcionário.'); return; }
    this.carregando.set(true);
    this.erro.set(null);
    this.svc.obterEspelho(this.funcionarioId, this.competencia).subscribe({
      next: (r) => { this.espelho.set(r.espelho); this.carregando.set(false); },
      error: (e) => { this.erro.set(e?.error?.message ?? 'Falha ao carregar espelho.'); this.carregando.set(false); },
    });
  }

  baixarPdf(): void {
    this.svc.baixarEspelhoPdf(this.funcionarioId, this.competencia).subscribe({
      next: (blob) => {
        const url = URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = `espelho-${this.competencia}.pdf`;
        a.click();
        URL.revokeObjectURL(url);
      },
      error: (e) => this.erro.set(e?.error?.message ?? 'Falha ao baixar PDF.'),
    });
  }

  formatMin(m: number): string {
    if (!m) return '—';
    return `${Math.floor(m / 60).toString().padStart(2, '0')}h${(m % 60).toString().padStart(2, '0')}`;
  }
  formatMinSign(m: number): string {
    if (m === 0) return '0h00';
    return m > 0 ? '+' + this.formatMin(m) : '-' + this.formatMin(-m);
  }
  batidasStr(d: any): string {
    return d.batidas.map((b: any) => b.hora).join('  ');
  }
}
