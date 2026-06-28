import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PontoService, StatusFechamentoPonto } from '../ponto.services';

interface FechamentoItem {
  funcionarioId: string;
  status: StatusFechamentoPonto;
  fechadoEm?: string | null;
}

@Component({
  selector: 'app-fechamento',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <h3>Fechamento de competência</h3>
    <div class="row g-3 mb-3">
      <div class="col-md-3"><label>Competência (YYYY-MM)</label>
        <input class="form-control" [(ngModel)]="competencia" /></div>
      <div class="col-md-3 d-flex align-items-end">
        <button class="btn btn-primary" (click)="carregar()">Listar status</button>
      </div>
    </div>

    @if (erro()) { <div class="alert alert-danger">{{ erro() }}</div> }
    @if (mensagem()) { <div class="alert alert-success">{{ mensagem() }}</div> }

    <table class="table table-sm">
      <thead><tr>
        <th>Funcionário</th><th>Status</th><th>Fechado em</th><th class="text-end">Ações</th>
      </tr></thead>
      <tbody>
        <tr *ngFor="let f of itens()">
          <td class="small">{{ f.funcionarioId.substring(0, 8) }}…</td>
          <td>{{ f.status }}</td>
          <td>{{ f.fechadoEm | date:'dd/MM/yyyy HH:mm' }}</td>
          <td class="text-end">
            <button class="btn btn-sm btn-success me-1" (click)="fechar(f)"
                    [disabled]="f.status === 'Fechado'">Fechar</button>
            <button class="btn btn-sm btn-outline-warning" (click)="reabrir(f)"
                    [disabled]="f.status !== 'Fechado'">Reabrir</button>
          </td>
        </tr>
        <tr *ngIf="!itens().length">
          <td colspan="4" class="text-center text-muted">Nenhum registro carregado.</td>
        </tr>
      </tbody>
    </table>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FechamentoComponent {
  private readonly svc = inject(PontoService);
  readonly itens = signal<FechamentoItem[]>([]);
  readonly erro = signal<string | null>(null);
  readonly mensagem = signal<string | null>(null);

  competencia = new Date().toISOString().slice(0, 7);

  carregar(): void {
    this.erro.set(null);
    this.svc.listarStatusFechamento(this.competencia).subscribe({
      next: (r) => this.itens.set(r.items),
      error: (e) => this.erro.set(e?.error?.message ?? 'Falha ao listar.'),
    });
  }

  fechar(f: FechamentoItem): void {
    const obs = prompt('Observações (opcional):') ?? undefined;
    this.svc.fecharCompetencia(f.funcionarioId, this.competencia, obs).subscribe({
      next: () => { this.mensagem.set(`Competência ${this.competencia} fechada.`); this.carregar(); },
      error: (e) => this.erro.set(e?.error?.message ?? 'Falha ao fechar.'),
    });
  }

  reabrir(f: FechamentoItem): void {
    const motivo = prompt('Motivo da reabertura (obrigatório):');
    if (!motivo) return;
    this.svc.reabrirCompetencia(f.funcionarioId, this.competencia, motivo).subscribe({
      next: () => { this.mensagem.set(`Competência ${this.competencia} reaberta.`); this.carregar(); },
      error: (e) => this.erro.set(e?.error?.message ?? 'Falha ao reabrir.'),
    });
  }
}
