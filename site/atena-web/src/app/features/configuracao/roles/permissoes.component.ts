import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { Permissao, PermissaoService } from '../configuracao.services';

@Component({
  selector: 'app-permissoes',
  standalone: true,
  template: `
    <h3>Permissões disponíveis</h3>
    <table class="table table-sm">
      <thead><tr><th>Recurso</th><th>Ação</th><th>Chave</th><th>Descrição</th></tr></thead>
      <tbody>
        @for (p of permissoes(); track p.chave) {
          <tr><td>{{ p.recurso }}</td><td>{{ p.acao }}</td><td><code>{{ p.chave }}</code></td><td>{{ p.descricao }}</td></tr>
        }
      </tbody>
    </table>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PermissoesComponent implements OnInit {
  private readonly servico = inject(PermissaoService);
  readonly permissoes = signal<Permissao[]>([]);
  ngOnInit(): void { this.servico.listar().subscribe((r) => this.permissoes.set(r)); }
}
