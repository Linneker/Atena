import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ExportacaoResponse, Oficial671Service } from './oficial-671.service';

@Component({
  standalone: true,
  selector: 'atena-exportar-afd-aej',
  imports: [CommonModule, FormsModule],
  template: `
    <h2>Exportar AFD / AEJ (Portaria 671)</h2>
    <label>Empresa ID <input [(ngModel)]="empresaId" /></label>
    <label>Início <input type="date" [(ngModel)]="inicio" /></label>
    <label>Fim <input type="date" [(ngModel)]="fim" /></label>
    <div class="botoes">
      <button (click)="exportar('afd')" [disabled]="!pronto() || ocupado()">Exportar AFD</button>
      <button (click)="exportar('aej')" [disabled]="!pronto() || ocupado()">Exportar AEJ</button>
    </div>

    @if (ultimoAfd(); as r) {
      <div>
        <p>AFD: <strong>{{ r.status }}</strong> — hash {{ r.hashSha256 }}</p>
        <a [href]="api.downloadAfdUrl(r.exportacaoId)" target="_blank">Baixar AFD</a>
      </div>
    }
    @if (ultimoAej(); as r) {
      <div>
        <p>AEJ: <strong>{{ r.status }}</strong> — hash {{ r.hashSha256 }}</p>
        <a [href]="api.downloadAejUrl(r.exportacaoId, 'json')" target="_blank">Baixar AEJ (JSON)</a>
        |
        <a [href]="api.downloadAejUrl(r.exportacaoId, 'jws')" target="_blank">Baixar JWS (assinatura)</a>
      </div>
    }
  `,
})
export class ExportarAfdAejComponent {
  readonly api = inject(Oficial671Service);
  empresaId = '';
  inicio = '';
  fim = '';
  ocupado = signal(false);
  ultimoAfd = signal<ExportacaoResponse | null>(null);
  ultimoAej = signal<ExportacaoResponse | null>(null);

  pronto(): boolean { return !!this.empresaId && !!this.inicio && !!this.fim; }

  exportar(tipo: 'afd' | 'aej'): void {
    this.ocupado.set(true);
    const payload = { empresaId: this.empresaId, periodoInicio: this.inicio, periodoFim: this.fim };
    const obs = tipo === 'afd' ? this.api.exportarAfd(payload) : this.api.exportarAej(payload);
    obs.subscribe({
      next: (r) => { (tipo === 'afd' ? this.ultimoAfd : this.ultimoAej).set(r); this.ocupado.set(false); },
      error: () => this.ocupado.set(false),
    });
  }
}
