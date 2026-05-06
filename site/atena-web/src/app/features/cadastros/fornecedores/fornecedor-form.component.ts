import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudFormComponent, CampoFormulario } from '@shared/crud/crud-form.component';
import { FornecedorService } from '../cadastros.services';

@Component({
  selector: 'app-fornecedor-form',
  standalone: true,
  imports: [CrudFormComponent],
  template: `<app-crud-form [titulo]="'Fornecedor'" [campos]="campos" [servico]="servico" [rotaLista]="'/cadastros/fornecedores'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FornecedorFormComponent {
  readonly servico = inject(FornecedorService);
  readonly campos: CampoFormulario[] = [
    { nome: 'razaoSocial', rotulo: 'Razão Social', obrigatorio: true },
    { nome: 'cnpj', rotulo: 'CNPJ', obrigatorio: true },
    { nome: 'email', rotulo: 'E-mail', tipo: 'email' },
    { nome: 'telefone', rotulo: 'Telefone' },
  ];
}
