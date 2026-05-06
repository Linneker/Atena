import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudFormComponent, CampoFormulario } from '@shared/crud/crud-form.component';
import { RecebimentoCompraService } from '../compras.services';

@Component({
  selector: 'app-recebimento-form',
  standalone: true,
  imports: [CrudFormComponent],
  template: `<app-crud-form [titulo]="'Recebimento de Compra'" [campos]="campos" [servico]="servico" [rotaLista]="'/compras/recebimentos'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RecebimentoFormComponent {
  readonly servico = inject(RecebimentoCompraService);
  readonly campos: CampoFormulario[] = [
    { nome: 'pedidoNumero', rotulo: 'Número do Pedido', obrigatorio: true },
    { nome: 'data', rotulo: 'Data', tipo: 'date', obrigatorio: true },
    { nome: 'tipo', rotulo: 'Tipo (Total/Parcial/Divergencia)', obrigatorio: true },
    { nome: 'status', rotulo: 'Status' },
  ];
}
