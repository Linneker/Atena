import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CampoFormulario, CrudFormComponent } from '@shared/crud/crud-form.component';
import { LotacaoService } from '../rh.services';

@Component({
  selector: 'app-lotacao-form',
  standalone: true,
  imports: [CrudFormComponent],
  template: `<app-crud-form [titulo]="'Lotação'" [campos]="campos" [servico]="servico" [rotaLista]="'/rh/lotacoes'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LotacaoFormComponent {
  readonly servico = inject(LotacaoService);
  readonly campos: CampoFormulario[] = [
    { nome: 'nome', rotulo: 'Nome', obrigatorio: true },
    { nome: 'cnpj', rotulo: 'CNPJ (somente dígitos, 14)' },
    { nome: 'enderecoJson', rotulo: 'Endereço (JSON)',
      ajuda: 'Ex.: {"cep":"01001000","logradouro":"...","cidade":"São Paulo","uf":"SP"}' },
    { nome: 'ativo', rotulo: 'Ativa', tipo: 'checkbox' },
  ];
}
