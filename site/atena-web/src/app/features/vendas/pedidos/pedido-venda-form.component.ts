import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudFormComponent, CampoFormulario } from '@shared/crud/crud-form.component';
import { PedidoVendaService } from '../vendas.services';

@Component({
  selector: 'app-pedido-venda-form',
  standalone: true,
  imports: [CrudFormComponent],
  template: `<app-crud-form [titulo]="'Pedido de Venda'" [campos]="campos" [servico]="servico" [rotaLista]="'/vendas/pedidos'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PedidoVendaFormComponent {
  readonly servico = inject(PedidoVendaService);
  readonly campos: CampoFormulario[] = [
    { nome: 'numero', rotulo: 'Número' },
    { nome: 'cliente', rotulo: 'Cliente', obrigatorio: true },
    { nome: 'vendedor', rotulo: 'Vendedor' },
    { nome: 'total', rotulo: 'Total', tipo: 'number' },
    { nome: 'status', rotulo: 'Status' },
  ];
}
