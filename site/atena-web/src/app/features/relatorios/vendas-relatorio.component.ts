import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RelatoriosService, VendaRelatorio } from './relatorios.service';

@Component({
  selector: 'app-vendas-relatorio',
  standalone: true,
  imports: [FormsModule],
  template: `
    <h3>Relatório de Vendas</h3>
    <div class="d-flex gap-2 mb-3">
      <input type="date" class="form-control form-control-sm" [(ngModel)]="inicio" />
      <input type="date" class="form-control form-control-sm" [(ngModel)]="fim" />
      <button class="btn btn-sm btn-primary" (click)="carregar()">Gerar</button>
    </div>
    <table class="table table-sm">
      <thead><tr><th>Data</th><th>Vendedor</th><th>Cliente</th><th>Produto</th><th class="text-end">Valor</th></tr></thead>
      <tbody>
        @for (v of vendas(); track v) {
          <tr>
            <td>{{ v.data }}</td><td>{{ v.vendedor }}</td><td>{{ v.cliente }}</td>
            <td>{{ v.produto }}</td><td class="text-end">{{ v.valor.toFixed(2) }}</td>
          </tr>
        }
      </tbody>
    </table>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class VendasRelatorioComponent implements OnInit {
  private readonly rel = inject(RelatoriosService);
  readonly vendas = signal<VendaRelatorio[]>([]);
  inicio = new Date(new Date().getFullYear(), new Date().getMonth(), 1).toISOString().slice(0, 10);
  fim = new Date().toISOString().slice(0, 10);

  ngOnInit(): void { this.carregar(); }
  carregar(): void { this.rel.vendas(this.inicio, this.fim).subscribe((r) => this.vendas.set(r)); }
}
