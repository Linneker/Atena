import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudFormComponent, CampoFormulario } from '@shared/crud/crud-form.component';
import { PedidoCompraService } from '../compras.services';

@Component({
  selector: 'app-pedido-form',
  standalone: true,
  imports: [CrudFormComponent],
  template: `<app-crud-form [titulo]="'Pedido de Compra'" [campos]="campos" [servico]="servico" [rotaLista]="'/compras/pedidos'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PedidoFormComponent {
  readonly servico = inject(PedidoCompraService);
  readonly campos: CampoFormulario[] = [
    { nome: 'numero', rotulo: 'Número' },
    { nome: 'fornecedor', rotulo: 'Fornecedor', obrigatorio: true },
    { nome: 'status', rotulo: 'Status' },
    { nome: 'total', rotulo: 'Total', tipo: 'number' },
  ];
}
