import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudFormComponent, CampoFormulario } from '@shared/crud/crud-form.component';
import { UsuarioService } from '../configuracao.services';

@Component({
  selector: 'app-usuario-form',
  standalone: true,
  imports: [CrudFormComponent],
  template: `<app-crud-form [titulo]="'Usuário'" [campos]="campos" [servico]="servico" [rotaLista]="'/configuracao/usuarios'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class UsuarioFormComponent {
  readonly servico = inject(UsuarioService);
  readonly campos: CampoFormulario[] = [
    { nome: 'nome', rotulo: 'Nome', obrigatorio: true },
    { nome: 'email', rotulo: 'E-mail', tipo: 'email', obrigatorio: true },
    { nome: 'ativo', rotulo: 'Ativo (true/false)' },
  ];
}
