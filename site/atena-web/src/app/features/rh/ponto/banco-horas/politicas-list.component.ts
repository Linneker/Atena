import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { BancoHorasService, PoliticaBancoHoras } from '../ponto.services';

@Component({
  selector: 'app-politicas-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <h3>Políticas de banco de horas</h3>
    <table class="table table-sm">
      <thead><tr>
        <th>Nome</th><th>Limite (h)</th><th>Compensa em (dias)</th>
        <th>Permite pagar?</th><th>Fator</th><th>Ativa</th>
      </tr></thead>
      <tbody>
        <tr *ngFor="let p of politicas()">
          <td>{{ p.nome }}</td>
          <td>{{ p.limiteHorasAcumular }}</td>
          <td>{{ p.prazoCompensacaoDias }}</td>
          <td>{{ p.permitePagarExcedente ? 'Sim' : 'Não' }}</td>
          <td>{{ p.fatorPagamento }}</td>
          <td>{{ p.ativo ? 'Sim' : 'Não' }}</td>
        </tr>
      </tbody>
    </table>

    <div class="card p-3 mt-3">
      <h5>Nova política</h5>
      <div class="row g-2">
        <div class="col-md-3"><input class="form-control" placeholder="Nome" [(ngModel)]="nova.nome" /></div>
        <div class="col-md-2"><input type="date" class="form-control" [(ngModel)]="nova.vigenciaInicio" /></div>
        <div class="col-md-2"><input type="number" class="form-control" placeholder="Limite (h)" [(ngModel)]="nova.limiteHorasAcumular" /></div>
        <div class="col-md-2"><input type="number" class="form-control" placeholder="Prazo (dias)" [(ngModel)]="nova.prazoCompensacaoDias" /></div>
        <div class="col-md-1"><input type="number" step="0.1" class="form-control" placeholder="Fator" [(ngModel)]="nova.fatorPagamento" /></div>
        <div class="col-md-2 d-flex align-items-end">
          <button class="btn btn-success" (click)="criar()">Criar</button>
        </div>
      </div>
      <div class="form-check mt-2">
        <input type="checkbox" class="form-check-input" id="ppe" [(ngModel)]="nova.permitePagarExcedente" />
        <label class="form-check-label" for="ppe">Permite pagar excedente</label>
      </div>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PoliticasListComponent {
  private readonly svc = inject(BancoHorasService);
  readonly politicas = signal<PoliticaBancoHoras[]>([]);

  nova = {
    nome: '',
    vigenciaInicio: new Date().toISOString().slice(0, 10),
    vigenciaFim: null as string | null,
    limiteHorasAcumular: 40,
    prazoCompensacaoDias: 180,
    permitePagarExcedente: true,
    fatorPagamento: 1.0,
  };

  constructor() { this.carregar(); }

  carregar(): void {
    this.svc.listarPoliticas().subscribe((r) => this.politicas.set(r.items));
  }

  criar(): void {
    this.svc.criarPolitica(this.nova).subscribe(() => this.carregar());
  }
}
