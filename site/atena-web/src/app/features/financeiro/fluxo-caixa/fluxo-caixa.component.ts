import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { environment } from '@env/environment';

interface FluxoLinha { data: string; descricao: string; entrada: number; saida: number; saldo: number; }
interface FluxoResposta { linhas: FluxoLinha[]; saldoInicial: number; saldoFinal: number; }

@Component({
  selector: 'app-fluxo-caixa',
  standalone: true,
  imports: [FormsModule],
  template: `
    <h3>Fluxo de Caixa</h3>
    <div class="d-flex gap-2 mb-3">
      <input type="date" class="form-control form-control-sm" [(ngModel)]="dataInicio" />
      <input type="date" class="form-control form-control-sm" [(ngModel)]="dataFim" />
      <button class="btn btn-sm btn-primary" (click)="carregar()">Consultar</button>
    </div>
    @if (fluxo(); as f) {
      <table class="table table-sm">
        <thead><tr><th>Data</th><th>Descrição</th><th class="text-end">Entrada</th><th class="text-end">Saída</th><th class="text-end">Saldo</th></tr></thead>
        <tbody>
          @for (l of f.linhas; track l) {
            <tr>
              <td>{{ l.data }}</td><td>{{ l.descricao }}</td>
              <td class="text-end text-success">{{ l.entrada.toFixed(2) }}</td>
              <td class="text-end text-danger">{{ l.saida.toFixed(2) }}</td>
              <td class="text-end">{{ l.saldo.toFixed(2) }}</td>
            </tr>
          }
        </tbody>
        <tfoot><tr><th colspan="4">Saldo final</th><th class="text-end">{{ f.saldoFinal.toFixed(2) }}</th></tr></tfoot>
      </table>
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FluxoCaixaComponent implements OnInit {
  private readonly http = inject(HttpClient);
  readonly fluxo = signal<FluxoResposta | null>(null);
  dataInicio = new Date(new Date().getFullYear(), new Date().getMonth(), 1).toISOString().slice(0, 10);
  dataFim = new Date().toISOString().slice(0, 10);

  ngOnInit(): void { this.carregar(); }

  carregar(): void {
    this.http
      .get<FluxoResposta>(`${environment.apiUrl}/${environment.apiVersion}/fluxo-caixa`, {
        params: { dataInicio: this.dataInicio, dataFim: this.dataFim },
      })
      .subscribe((r) => this.fluxo.set(r));
  }
}
