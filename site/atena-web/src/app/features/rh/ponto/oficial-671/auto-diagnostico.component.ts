import { CommonModule } from '@angular/common';
import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Oficial671Service, ValidacaoRep } from './oficial-671.service';

@Component({
  standalone: true,
  selector: 'atena-auto-diagnostico-rep',
  imports: [CommonModule, FormsModule],
  template: `
    <h2>Auto-diagnóstico do REP</h2>
    <p>Confere se a empresa está apta a emitir comprovantes 671 (configuração + certificado).</p>
    <label>Empresa ID <input [(ngModel)]="empresaId" /></label>
    <button (click)="rodar()" [disabled]="!empresaId || rodando()">Validar</button>

    @if (resultado(); as r) {
      <h3 [style.color]="r.apto ? 'green' : 'red'">{{ r.apto ? '✓ Apto' : '✗ Inapto' }}</h3>
      <ul>
        @for (c of r.checagens; track c.item) {
          <li>
            <strong [style.color]="c.ok ? 'green' : 'red'">{{ c.ok ? '✓' : '✗' }} {{ c.item }}</strong>
            @if (c.mensagem) { — <small>{{ c.mensagem }}</small> }
          </li>
        }
      </ul>
    }
  `,
})
export class AutoDiagnosticoRepComponent {
  private readonly api = inject(Oficial671Service);
  empresaId = '';
  rodando = signal(false);
  resultado = signal<ValidacaoRep | null>(null);

  rodar(): void {
    this.rodando.set(true);
    this.resultado.set(null);
    this.api.validar(this.empresaId).subscribe({
      next: (r) => { this.resultado.set(r); this.rodando.set(false); },
      error: () => { this.rodando.set(false); },
    });
  }
}
