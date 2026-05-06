import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { PosicaoEstoque, RelatoriosService } from './relatorios.service';

@Component({
  selector: 'app-estoque-relatorio',
  standalone: true,
  template: `
    <h3>Posição de Estoque</h3>
    <table class="table table-sm">
      <thead><tr><th>Produto</th><th class="text-end">Saldo</th><th class="text-end">Valor</th></tr></thead>
      <tbody>
        @for (p of posicoes(); track p.produto) {
          <tr><td>{{ p.produto }}</td><td class="text-end">{{ p.saldo }}</td><td class="text-end">{{ p.valor.toFixed(2) }}</td></tr>
        }
      </tbody>
    </table>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class EstoqueRelatorioComponent implements OnInit {
  private readonly rel = inject(RelatoriosService);
  readonly posicoes = signal<PosicaoEstoque[]>([]);
  ngOnInit(): void { this.rel.posicaoEstoque().subscribe((r) => this.posicoes.set(r)); }
}
