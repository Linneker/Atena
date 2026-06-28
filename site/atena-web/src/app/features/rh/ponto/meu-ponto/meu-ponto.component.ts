import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PontoService, MarcacaoPonto, TipoMarcacao } from '../ponto.services';

/**
 * Tela "Meu Ponto" — visualização da semana + botão grande "Bater".
 * Tipo da batida é inferido pelo backend (alterna E/SA/VA/S pela última do dia).
 */
@Component({
  selector: 'app-meu-ponto',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <h3>Meu ponto</h3>
    <div class="row g-3 mb-3">
      <div class="col-md-3"><label>Data início</label>
        <input type="date" class="form-control" [(ngModel)]="dataInicio" (change)="carregar()" /></div>
      <div class="col-md-3"><label>Data fim</label>
        <input type="date" class="form-control" [(ngModel)]="dataFim" (change)="carregar()" /></div>
      <div class="col-md-6 d-flex align-items-end justify-content-end">
        <button class="btn btn-success btn-lg" (click)="bater()" [disabled]="batendo()">
          {{ batendo() ? 'Batendo...' : '⏰ BATER PONTO AGORA' }}
        </button>
      </div>
    </div>

    @if (erro()) { <div class="alert alert-danger">{{ erro() }}</div> }
    @if (mensagemSucesso()) { <div class="alert alert-success">{{ mensagemSucesso() }}</div> }

    <table class="table table-sm table-striped">
      <thead><tr><th>Data / Hora</th><th>Tipo</th><th>Origem</th><th>Status</th><th>Hash</th></tr></thead>
      <tbody>
        <tr *ngFor="let m of marcacoes()">
          <td>{{ m.dataHora | date:'dd/MM/yyyy HH:mm:ss' }}</td>
          <td>{{ m.tipo }}</td>
          <td>{{ m.origem }}</td>
          <td>{{ m.status }}</td>
          <td class="text-muted small">{{ m.hashIntegridade.substring(0, 12) }}…</td>
        </tr>
        <tr *ngIf="!marcacoes().length">
          <td colspan="5" class="text-center text-muted">Nenhuma marcação no período.</td>
        </tr>
      </tbody>
    </table>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MeuPontoComponent {
  private readonly svc = inject(PontoService);

  readonly marcacoes = signal<MarcacaoPonto[]>([]);
  readonly batendo = signal(false);
  readonly erro = signal<string | null>(null);
  readonly mensagemSucesso = signal<string | null>(null);

  dataInicio = new Date(Date.now() - 7 * 86400000).toISOString().slice(0, 10);
  dataFim = new Date().toISOString().slice(0, 10);

  constructor() { this.carregar(); }

  carregar(): void {
    this.svc.listarProprio(this.dataInicio, this.dataFim).subscribe({
      next: (r) => this.marcacoes.set(r.items),
      error: (e) => this.erro.set(e?.error?.message ?? 'Erro ao carregar marcações.'),
    });
  }

  bater(): void {
    this.batendo.set(true);
    this.erro.set(null);
    this.svc.baterPonto({ tipo: null }).subscribe({
      next: (r) => {
        this.batendo.set(false);
        this.mensagemSucesso.set(`Ponto batido: ${r.tipo} às ${new Date(r.dataHora).toLocaleTimeString('pt-BR')}`);
        this.carregar();
        setTimeout(() => this.mensagemSucesso.set(null), 5000);
      },
      error: (e) => {
        this.batendo.set(false);
        this.erro.set(e?.error?.message ?? 'Falha ao bater ponto.');
      },
    });
  }
}
