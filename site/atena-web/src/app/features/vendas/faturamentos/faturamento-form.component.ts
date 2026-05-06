import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudFormComponent, CampoFormulario } from '@shared/crud/crud-form.component';
import { FaturamentoService } from '../vendas.services';

@Component({
  selector: 'app-faturamento-form',
  standalone: true,
  imports: [CrudFormComponent],
  template: `<app-crud-form [titulo]="'Faturamento'" [campos]="campos" [servico]="servico" [rotaLista]="'/vendas/faturamentos'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FaturamentoFormComponent {
  readonly servico = inject(FaturamentoService);
  readonly campos: CampoFormulario[] = [
    { nome: 'pedidoNumero', rotulo: 'Número Pedido', obrigatorio: true },
    { nome: 'numeroNota', rotulo: 'Número Nota' },
    { nome: 'valor', rotulo: 'Valor', tipo: 'number' },
    { nome: 'data', rotulo: 'Data', tipo: 'date' },
    { nome: 'status', rotulo: 'Status' },
  ];
}
