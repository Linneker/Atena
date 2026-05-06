import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudFormComponent, CampoFormulario } from '@shared/crud/crud-form.component';
import { FuncionarioService } from '../cadastros.services';

@Component({
  selector: 'app-funcionario-form',
  standalone: true,
  imports: [CrudFormComponent],
  template: `<app-crud-form [titulo]="'Funcionário'" [campos]="campos" [servico]="servico" [rotaLista]="'/cadastros/funcionarios'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FuncionarioFormComponent {
  readonly servico = inject(FuncionarioService);
  readonly campos: CampoFormulario[] = [
    { nome: 'nome', rotulo: 'Nome', obrigatorio: true },
    { nome: 'cpf', rotulo: 'CPF', obrigatorio: true },
    { nome: 'cargo', rotulo: 'Cargo' },
    { nome: 'departamento', rotulo: 'Departamento' },
  ];
}
