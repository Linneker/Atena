import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudListComponent } from '@shared/crud/crud-list.component';
import { ColunaTabela } from '@shared/data-table/data-table.component';
import { Inventario, InventarioService } from '../estoque.services';

@Component({
  selector: 'app-inventario-list',
  standalone: true,
  imports: [CrudListComponent],
  template: `<app-crud-list [titulo]="'Inventários'" [colunas]="colunas" [servico]="servico" [rotaForm]="'/estoque/inventario'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class InventarioListComponent {
  readonly servico = inject(InventarioService);
  readonly colunas: ColunaTabela<Inventario>[] = [
    { campo: 'descricao', titulo: 'Descrição' },
    { campo: 'status', titulo: 'Status' },
    { campo: 'abertoEm', titulo: 'Aberto em' },
    { campo: 'fechadoEm', titulo: 'Fechado em' },
  ];
}
