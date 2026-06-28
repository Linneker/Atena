import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { PontoService, AjustePendente } from '../ponto.services';

@Component({
  selector: 'app-aprovacoes-pendentes',
  standalone: true,
  imports: [CommonModule],
  template: `
    <h3>Aprovações pendentes</h3>
    @if (erro()) { <div class="alert alert-danger">{{ erro() }}</div> }
    <table class="table table-sm">
      <thead><tr>
        <th>Solicitado</th><th>Funcionário</th><th>Tipo</th><th>Hora proposta</th>
        <th>Motivo</th><th class="text-end">Ações</th>
      </tr></thead>
      <tbody>
        <tr *ngFor="let a of ajustes()">
          <td>{{ a.solicitadoEm | date:'dd/MM HH:mm' }}</td>
          <td class="small">{{ a.funcionarioId.substring(0, 8) }}…</td>
          <td>{{ a.tipoAjuste }}</td>
          <td>{{ a.dataHoraProposta | date:'dd/MM HH:mm' }}</td>
          <td>{{ a.motivo }}</td>
          <td class="text-end">
            <button class="btn btn-sm btn-success me-2" (click)="aprovar(a)">Aprovar</button>
            <button class="btn btn-sm btn-outline-danger" (click)="rejeitar(a)">Rejeitar</button>
          </td>
        </tr>
        <tr *ngIf="!ajustes().length">
          <td colspan="6" class="text-center text-muted">Nenhum ajuste pendente.</td>
        </tr>
      </tbody>
    </table>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AprovacoesPendentesComponent {
  private readonly svc = inject(PontoService);
  readonly ajustes = signal<AjustePendente[]>([]);
  readonly erro = signal<string | null>(null);

  constructor() { this.carregar(); }

  carregar(): void {
    this.svc.listarAjustesPendentes().subscribe({
      next: (r) => this.ajustes.set(r.items),
      error: (e) => this.erro.set(e?.error?.message ?? 'Falha ao carregar pendentes.'),
    });
  }

  aprovar(a: AjustePendente): void {
    const just = prompt('Justificativa (opcional):') ?? undefined;
    this.svc.aprovarAjuste(a.id, just).subscribe(() => this.carregar());
  }

  rejeitar(a: AjustePendente): void {
    const just = prompt('Justificativa da rejeição (obrigatório):');
    if (!just) return;
    this.svc.rejeitarAjuste(a.id, just).subscribe(() => this.carregar());
  }
}
