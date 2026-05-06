import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudFormComponent, CampoFormulario } from '@shared/crud/crud-form.component';
import { ClienteService } from '../cadastros.services';

@Component({
  selector: 'app-cliente-form',
  standalone: true,
  imports: [CrudFormComponent],
  template: `<app-crud-form [titulo]="'Cliente'" [campos]="campos" [servico]="servico" [rotaLista]="'/cadastros/clientes'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ClienteFormComponent {
  readonly servico = inject(ClienteService);
  readonly campos: CampoFormulario[] = [
    { nome: 'nome', rotulo: 'Nome', obrigatorio: true },
    { nome: 'cpfCnpj', rotulo: 'CPF/CNPJ', obrigatorio: true },
    { nome: 'email', rotulo: 'E-mail', tipo: 'email' },
    { nome: 'telefone', rotulo: 'Telefone' },
  ];
}
