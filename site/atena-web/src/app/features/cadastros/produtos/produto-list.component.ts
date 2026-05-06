import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudListComponent } from '@shared/crud/crud-list.component';
import { ColunaTabela } from '@shared/data-table/data-table.component';
import { Produto, ProdutoService } from '../cadastros.services';

@Component({
  selector: 'app-produto-list',
  standalone: true,
  imports: [CrudListComponent],
  template: `<app-crud-list [titulo]="'Produtos'" [colunas]="colunas" [servico]="servico" [rotaForm]="'/cadastros/produtos'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProdutoListComponent {
  readonly servico = inject(ProdutoService);
  readonly colunas: ColunaTabela<Produto>[] = [
    { campo: 'codigo', titulo: 'Código' },
    { campo: 'descricao', titulo: 'Descrição' },
    { campo: 'unidade', titulo: 'Un.' },
    { campo: 'precoVenda', titulo: 'Preço', formato: (l) => l.precoVenda.toFixed(2) },
  ];
}
