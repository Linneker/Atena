import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ParametroService, ParametroSistema } from '../configuracao.services';

@Component({
  selector: 'app-parametros',
  standalone: true,
  imports: [FormsModule],
  template: `
    <h3>Parâmetros do Sistema</h3>
    <table class="table table-sm">
      <thead><tr><th>Chave</th><th>Valor</th><th>Descrição</th><th></th></tr></thead>
      <tbody>
        @for (p of parametros(); track p.chave) {
          <tr>
            <td><code>{{ p.chave }}</code></td>
            <td><input class="form-control form-control-sm" [(ngModel)]="p.valor" /></td>
            <td>{{ p.descricao }}</td>
            <td><button class="btn btn-sm btn-primary" (click)="salvar(p)">Salvar</button></td>
          </tr>
        }
      </tbody>
    </table>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ParametrosComponent implements OnInit {
  private readonly servico = inject(ParametroService);
  readonly parametros = signal<ParametroSistema[]>([]);

  ngOnInit(): void { this.servico.listar().subscribe((r) => this.parametros.set(r)); }
  salvar(p: ParametroSistema): void { this.servico.salvar(p).subscribe(); }
}
