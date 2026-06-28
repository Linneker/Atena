import { AfterViewInit, ChangeDetectionStrategy, Component, ElementRef, OnInit, ViewChild, effect, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Chart, registerables } from 'chart.js';
import { DashboardKpis, EvolucaoMes, RelatoriosService } from './relatorios.service';

Chart.register(...registerables);

@Component({
  selector: 'app-dashboard-relatorios',
  standalone: true,
  imports: [CommonModule],
  template: `
    <h3 class="mb-3">Indicadores</h3>
    @if (kpis(); as k) {
      <div class="row g-3 mb-4">
        <div class="col-md-2">
          <div class="card p-3 text-center">
            <small class="text-muted">Receita</small>
            <h4 class="text-success m-0">R$ {{ k.receita.toFixed(2) }}</h4>
          </div>
        </div>
        <div class="col-md-2">
          <div class="card p-3 text-center">
            <small class="text-muted">Despesa</small>
            <h4 class="text-danger m-0">R$ {{ k.despesa.toFixed(2) }}</h4>
          </div>
        </div>
        <div class="col-md-2">
          <div class="card p-3 text-center">
            <small class="text-muted">Resultado</small>
            <h4 class="m-0" [class.text-success]="k.resultado >= 0" [class.text-danger]="k.resultado < 0">
              R$ {{ k.resultado.toFixed(2) }}
            </h4>
          </div>
        </div>
        <div class="col-md-2">
          <div class="card p-3 text-center">
            <small class="text-muted">Vendas abertas</small>
            <h4 class="m-0">{{ k.vendasAbertas }}</h4>
          </div>
        </div>
        <div class="col-md-2">
          <div class="card p-3 text-center">
            <small class="text-muted">Vencimentos 7d</small>
            <h4 class="m-0 text-info">{{ k.vencimentos }}</h4>
          </div>
        </div>
        <div class="col-md-2">
          <div class="card p-3 text-center">
            <small class="text-muted">Estoque crítico</small>
            <h4 class="text-warning m-0">{{ k.estoqueCritico }}</h4>
          </div>
        </div>
      </div>
    }

    <h5 class="mt-4">Evolução financeira (12 meses)</h5>
    <div class="card p-3" style="height: 400px">
      <canvas #grafico></canvas>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardRelatoriosComponent implements OnInit, AfterViewInit {
  private readonly rel = inject(RelatoriosService);
  readonly kpis = signal<DashboardKpis | null>(null);
  readonly evolucao = signal<EvolucaoMes[]>([]);

  @ViewChild('grafico') canvas?: ElementRef<HTMLCanvasElement>;
  private chart?: Chart;

  constructor() {
    effect(() => {
      const dados = this.evolucao();
      if (dados.length === 0) return;
      this.atualizarGrafico(dados);
    });
  }

  ngOnInit(): void {
    this.rel.kpis().subscribe((k) => this.kpis.set(k));
    this.rel.evolucao().subscribe((e) => this.evolucao.set(e));
  }

  ngAfterViewInit(): void {
    const dados = this.evolucao();
    if (dados.length > 0) this.atualizarGrafico(dados);
  }

  private atualizarGrafico(dados: EvolucaoMes[]): void {
    if (!this.canvas) return;
    if (this.chart) this.chart.destroy();

    const labels = dados.map((d) => d.mes);
    const receitas = dados.map((d) => d.receita);
    const despesas = dados.map((d) => d.despesa);
    const resultado = dados.map((d) => d.receita - d.despesa);

    this.chart = new Chart(this.canvas.nativeElement, {
      type: 'line',
      data: {
        labels,
        datasets: [
          {
            label: 'Receitas',
            data: receitas,
            borderColor: '#198754',
            backgroundColor: 'rgba(25, 135, 84, 0.15)',
            tension: 0.3,
            fill: true,
          },
          {
            label: 'Despesas',
            data: despesas,
            borderColor: '#dc3545',
            backgroundColor: 'rgba(220, 53, 69, 0.15)',
            tension: 0.3,
            fill: true,
          },
          {
            label: 'Resultado',
            data: resultado,
            borderColor: '#0d6efd',
            borderDash: [6, 4],
            tension: 0.3,
            fill: false,
          },
        ],
      },
      options: {
        responsive: true,
        maintainAspectRatio: false,
        plugins: {
          legend: { position: 'bottom' },
          tooltip: {
            callbacks: {
              label: (ctx) => `${ctx.dataset.label}: R$ ${Number(ctx.parsed.y).toFixed(2)}`,
            },
          },
        },
        scales: {
          y: {
            beginAtZero: true,
            ticks: { callback: (v) => 'R$ ' + Number(v).toFixed(0) },
          },
        },
      },
    });
  }
}
