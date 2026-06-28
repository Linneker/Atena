import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BancoHorasService, MovimentoBancoHoras, SaldoBancoHoras } from '../ponto.services';

@Component({
  selector: 'app-banco-horas',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <h3>Banco de horas</h3>
    <div class="row g-3 mb-3">
      <div class="col-md-5"><label>Funcionário (UUID)</label>
        <input class="form-control" [(ngModel)]="funcionarioId" /></div>
      <div class="col-md-3"><label>Competência</label>
        <input class="form-control" [(ngModel)]="competencia" /></div>
      <div class="col-md-4 d-flex align-items-end">
        <button class="btn btn-primary" (click)="carregar()">Carregar</button>
      </div>
    </div>

    @if (saldo(); as s) {
      <div class="card mb-3 p-3">
        <h5>Saldo da competência {{ s.competencia }}</h5>
        <p>
          Horas devidas: {{ s.horasDevidas }} | Realizadas: {{ s.horasRealizadas }} |
          Saldo: <strong [class]="s.saldoMinutos >= 0 ? 'text-success' : 'text-danger'">
            {{ formatMinSign(s.saldoMinutos) }}
          </strong>
        </p>
      </div>
    }

    <h5>Movimentos</h5>
    <table class="table table-sm">
      <thead><tr><th>Data</th><th>Origem</th><th>Minutos</th><th>Observação</th></tr></thead>
      <tbody>
        <tr *ngFor="let m of movimentos()">
          <td>{{ m.data | date:'dd/MM/yyyy' }}</td>
          <td>{{ m.origem }}</td>
          <td [class]="m.minutos >= 0 ? 'text-success' : 'text-danger'">
            {{ formatMinSign(m.minutos) }}
          </td>
          <td class="small text-muted">{{ m.observacao }}</td>
        </tr>
      </tbody>
    </table>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BancoHorasComponent {
  private readonly svc = inject(BancoHorasService);
  readonly saldo = signal<SaldoBancoHoras | null>(null);
  readonly movimentos = signal<MovimentoBancoHoras[]>([]);

  funcionarioId = '';
  competencia = new Date().toISOString().slice(0, 7);

  carregar(): void {
    if (!this.funcionarioId) return;
    this.svc.obterSaldo(this.funcionarioId, this.competencia).subscribe((r) => this.saldo.set(r));
    this.svc.listarMovimentos(this.funcionarioId, this.competencia).subscribe((r) => this.movimentos.set(r.items));
  }

  formatMinSign(m: number): string {
    if (m === 0) return '0h00';
    const abs = Math.abs(m);
    const fmt = `${Math.floor(abs / 60).toString().padStart(2, '0')}h${(abs % 60).toString().padStart(2, '0')}`;
    return (m > 0 ? '+' : '-') + fmt;
  }
}
