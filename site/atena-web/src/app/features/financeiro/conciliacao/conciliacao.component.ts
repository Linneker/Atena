import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '@env/environment';

interface ResultadoConciliacao { processados: number; conciliados: number; pendentes: number; }

@Component({
  selector: 'app-conciliacao',
  standalone: true,
  template: `
    <h3>Conciliação Bancária</h3>
    <p class="text-muted">Importe um extrato OFX/CSV para conciliação automática.</p>
    <input type="file" class="form-control mb-3" accept=".ofx,.csv" (change)="importar($event)" />
    @if (resultado(); as r) {
      <div class="alert alert-info">
        Processados: {{ r.processados }} | Conciliados: {{ r.conciliados }} | Pendentes: {{ r.pendentes }}
      </div>
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ConciliacaoComponent {
  private readonly http = inject(HttpClient);
  readonly resultado = signal<ResultadoConciliacao | null>(null);

  importar(ev: Event): void {
    const file = (ev.target as HTMLInputElement).files?.[0];
    if (!file) return;
    const fd = new FormData();
    fd.append('arquivo', file);
    this.http
      .post<ResultadoConciliacao>(`${environment.apiUrl}/${environment.apiVersion}/conciliacao/importar`, fd)
      .subscribe((r) => this.resultado.set(r));
  }
}
