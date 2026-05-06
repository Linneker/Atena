import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudListComponent } from '@shared/crud/crud-list.component';
import { ColunaTabela } from '@shared/data-table/data-table.component';
import { PedidoVenda, PedidoVendaService } from '../vendas.services';

@Component({
  selector: 'app-pedido-venda-list',
  standalone: true,
  imports: [CrudListComponent],
  template: `<app-crud-list [titulo]="'Pedidos de Venda'" [colunas]="colunas" [servico]="servico" [rotaForm]="'/vendas/pedidos'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PedidoVendaListComponent {
  readonly servico = inject(PedidoVendaService);
  readonly colunas: ColunaTabela<PedidoVenda>[] = [
    { campo: 'numero', titulo: 'Número' },
    { campo: 'cliente', titulo: 'Cliente' },
    { campo: 'vendedor', titulo: 'Vendedor' },
    { campo: 'total', titulo: 'Total', formato: (l) => l.total.toFixed(2) },
    { campo: 'status', titulo: 'Status' },
  ];
}
