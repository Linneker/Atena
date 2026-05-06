import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudFormComponent, CampoFormulario } from '@shared/crud/crud-form.component';
import { RoleService } from '../configuracao.services';

@Component({
  selector: 'app-role-form',
  standalone: true,
  imports: [CrudFormComponent],
  template: `<app-crud-form [titulo]="'Perfil'" [campos]="campos" [servico]="servico" [rotaLista]="'/configuracao/roles'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RoleFormComponent {
  readonly servico = inject(RoleService);
  readonly campos: CampoFormulario[] = [
    { nome: 'nome', rotulo: 'Nome', obrigatorio: true },
    { nome: 'descricao', rotulo: 'Descrição' },
  ];
}
