import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudFormComponent, CampoFormulario } from '@shared/crud/crud-form.component';
import { ProdutoService } from '../cadastros.services';

@Component({
  selector: 'app-produto-form',
  standalone: true,
  imports: [CrudFormComponent],
  template: `<app-crud-form [titulo]="'Produto'" [campos]="campos" [servico]="servico" [rotaLista]="'/cadastros/produtos'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProdutoFormComponent {
  readonly servico = inject(ProdutoService);
  readonly campos: CampoFormulario[] = [
    { nome: 'codigo', rotulo: 'Código', obrigatorio: true },
    { nome: 'descricao', rotulo: 'Descrição', obrigatorio: true },
    { nome: 'unidade', rotulo: 'Unidade' },
    { nome: 'precoVenda', rotulo: 'Preço de Venda', tipo: 'number' },
  ];
}
