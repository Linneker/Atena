import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { AgingFaixa, RelatoriosService } from './relatorios.service';

@Component({
  selector: 'app-aging-receber',
  standalone: true,
  template: `
    <h3>Aging Contas a Receber</h3>
    <table class="table table-sm">
      <thead><tr><th>Faixa</th><th class="text-end">Quantidade</th><th class="text-end">Valor</th></tr></thead>
      <tbody>
        @for (f of faixas(); track f.faixa) {
          <tr><td>{{ f.faixa }}</td><td class="text-end">{{ f.quantidade }}</td><td class="text-end">{{ f.valor.toFixed(2) }}</td></tr>
        }
      </tbody>
    </table>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AgingReceberComponent implements OnInit {
  private readonly rel = inject(RelatoriosService);
  readonly faixas = signal<AgingFaixa[]>([]);
  ngOnInit(): void { this.rel.agingReceber().subscribe((r) => this.faixas.set(r)); }
}
