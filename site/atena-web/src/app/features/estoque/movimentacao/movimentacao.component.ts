import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudListComponent } from '@shared/crud/crud-list.component';
import { ColunaTabela } from '@shared/data-table/data-table.component';
import { MovimentacaoEstoque, MovimentacaoEstoqueService } from '../estoque.services';

@Component({
  selector: 'app-movimentacao',
  standalone: true,
  imports: [CrudListComponent],
  template: `<app-crud-list [titulo]="'Movimentação de Estoque'" [colunas]="colunas" [servico]="servico" [rotaForm]="'/estoque/movimentacao'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class MovimentacaoComponent {
  readonly servico = inject(MovimentacaoEstoqueService);
  readonly colunas: ColunaTabela<MovimentacaoEstoque>[] = [
    { campo: 'data', titulo: 'Data' },
    { campo: 'produto', titulo: 'Produto' },
    { campo: 'tipo', titulo: 'Tipo' },
    { campo: 'quantidade', titulo: 'Quantidade' },
    { campo: 'motivo', titulo: 'Motivo' },
  ];
}
