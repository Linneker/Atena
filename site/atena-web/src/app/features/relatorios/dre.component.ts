import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { LinhaDre, RelatoriosService } from './relatorios.service';

@Component({
  selector: 'app-dre',
  standalone: true,
  imports: [FormsModule],
  template: `
    <h3>DRE</h3>
    <div class="d-flex gap-2 mb-3">
      <input type="month" class="form-control form-control-sm" [(ngModel)]="competencia" />
      <button class="btn btn-sm btn-primary" (click)="carregar()">Gerar</button>
    </div>
    <table class="table table-sm">
      <thead><tr><th>Conta</th><th class="text-end">Valor</th></tr></thead>
      <tbody>
        @for (l of linhas(); track l.conta) {
          <tr><td>{{ l.conta }}</td><td class="text-end">{{ l.valor.toFixed(2) }}</td></tr>
        }
      </tbody>
    </table>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DreComponent implements OnInit {
  private readonly rel = inject(RelatoriosService);
  readonly linhas = signal<LinhaDre[]>([]);
  competencia = new Date().toISOString().slice(0, 7);

  ngOnInit(): void { this.carregar(); }
  carregar(): void { this.rel.dre(this.competencia).subscribe((r) => this.linhas.set(r)); }
}
