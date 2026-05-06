import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudListComponent } from '@shared/crud/crud-list.component';
import { ColunaTabela } from '@shared/data-table/data-table.component';
import { Usuario, UsuarioService } from '../configuracao.services';

@Component({
  selector: 'app-usuario-list',
  standalone: true,
  imports: [CrudListComponent],
  template: `<app-crud-list [titulo]="'Usuários'" [colunas]="colunas" [servico]="servico" [rotaForm]="'/configuracao/usuarios'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UsuarioListComponent {
  readonly servico = inject(UsuarioService);
  readonly colunas: ColunaTabela<Usuario>[] = [
    { campo: 'nome', titulo: 'Nome' },
    { campo: 'email', titulo: 'E-mail' },
    { campo: 'ativo', titulo: 'Ativo', formato: (l) => (l.ativo ? 'Sim' : 'Não') },
  ];
}
