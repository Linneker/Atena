import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudListComponent } from '@shared/crud/crud-list.component';
import { ColunaTabela } from '@shared/data-table/data-table.component';
import { PedidoCompra, PedidoCompraService } from '../compras.services';

@Component({
  selector: 'app-pedido-list',
  standalone: true,
  imports: [CrudListComponent],
  template: `<app-crud-list [titulo]="'Pedidos de Compra'" [colunas]="colunas" [servico]="servico" [rotaForm]="'/compras/pedidos'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PedidoListComponent {
  readonly servico = inject(PedidoCompraService);
  readonly colunas: ColunaTabela<PedidoCompra>[] = [
    { campo: 'numero', titulo: 'Número' },
    { campo: 'fornecedor', titulo: 'Fornecedor' },
    { campo: 'status', titulo: 'Status' },
    { campo: 'total', titulo: 'Total', formato: (l) => l.total.toFixed(2) },
    { campo: 'emitidoEm', titulo: 'Emitido em' },
  ];
}
