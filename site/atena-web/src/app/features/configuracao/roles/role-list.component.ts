import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudListComponent } from '@shared/crud/crud-list.component';
import { ColunaTabela } from '@shared/data-table/data-table.component';
import { Role, RoleService } from '../configuracao.services';

@Component({
  selector: 'app-role-list',
  standalone: true,
  imports: [CrudListComponent],
  template: `<app-crud-list [titulo]="'Perfis (Roles)'" [colunas]="colunas" [servico]="servico" [rotaForm]="'/configuracao/roles'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RoleListComponent {
  readonly servico = inject(RoleService);
  readonly colunas: ColunaTabela<Role>[] = [
    { campo: 'nome', titulo: 'Nome' },
    { campo: 'descricao', titulo: 'Descrição' },
  ];
}
