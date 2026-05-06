import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { DashboardKpis, EvolucaoMes, RelatoriosService } from './relatorios.service';

@Component({
  selector: 'app-dashboard-relatorios',
  standalone: true,
  template: `
    <h3 class="mb-3">Dashboard</h3>
    @if (kpis(); as k) {
      <div class="row g-3 mb-4">
        <div class="col-md-2"><div class="card p-3"><small>Receita</small><h4 class="text-success">{{ k.receita.toFixed(2) }}</h4></div></div>
        <div class="col-md-2"><div class="card p-3"><small>Despesa</small><h4 class="text-danger">{{ k.despesa.toFixed(2) }}</h4></div></div>
        <div class="col-md-2"><div class="card p-3"><small>Resultado</small><h4>{{ k.resultado.toFixed(2) }}</h4></div></div>
        <div class="col-md-2"><div class="card p-3"><small>Vendas abertas</small><h4>{{ k.vendasAbertas }}</h4></div></div>
        <div class="col-md-2"><div class="card p-3"><small>Vencimentos</small><h4>{{ k.vencimentos }}</h4></div></div>
        <div class="col-md-2"><div class="card p-3"><small>Estoque crítico</small><h4 class="text-warning">{{ k.estoqueCritico }}</h4></div></div>
      </div>
    }
    @if (evolucao(); as ev) {
      <h5>Evolução (12 meses)</h5>
      <table class="table table-sm">
        <thead><tr><th>Mês</th><th class="text-end">Receita</th><th class="text-end">Despesa</th><th class="text-end">Resultado</th></tr></thead>
        <tbody>
          @for (m of ev; track m.mes) {
            <tr>
              <td>{{ m.mes }}</td>
              <td class="text-end text-success">{{ m.receita.toFixed(2) }}</td>
              <td class="text-end text-danger">{{ m.despesa.toFixed(2) }}</td>
              <td class="text-end">{{ (m.receita - m.despesa).toFixed(2) }}</td>
            </tr>
          }
        </tbody>
      </table>
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardRelatoriosComponent implements OnInit {
  private readonly rel = inject(RelatoriosService);
  readonly kpis = signal<DashboardKpis | null>(null);
  readonly evolucao = signal<EvolucaoMes[]>([]);

  ngOnInit(): void {
    this.rel.kpis().subscribe((k) => this.kpis.set(k));
    this.rel.evolucao().subscribe((e) => this.evolucao.set(e));
  }
}
