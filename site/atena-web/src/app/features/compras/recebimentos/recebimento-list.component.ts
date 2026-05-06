import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudListComponent } from '@shared/crud/crud-list.component';
import { ColunaTabela } from '@shared/data-table/data-table.component';
import { RecebimentoCompra, RecebimentoCompraService } from '../compras.services';

@Component({
  selector: 'app-recebimento-list',
  standalone: true,
  imports: [CrudListComponent],
  template: `<app-crud-list [titulo]="'Recebimentos'" [colunas]="colunas" [servico]="servico" [rotaForm]="'/compras/recebimentos'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RecebimentoListComponent {
  readonly servico = inject(RecebimentoCompraService);
  readonly colunas: ColunaTabela<RecebimentoCompra>[] = [
    { campo: 'pedidoNumero', titulo: 'Pedido' },
    { campo: 'data', titulo: 'Data' },
    { campo: 'tipo', titulo: 'Tipo' },
    { campo: 'status', titulo: 'Status' },
  ];
}
