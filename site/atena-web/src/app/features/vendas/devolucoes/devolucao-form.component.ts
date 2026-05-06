import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudFormComponent, CampoFormulario } from '@shared/crud/crud-form.component';
import { DevolucaoVendaService } from '../vendas.services';

@Component({
  selector: 'app-devolucao-form',
  standalone: true,
  imports: [CrudFormComponent],
  template: `<app-crud-form [titulo]="'Devolução'" [campos]="campos" [servico]="servico" [rotaLista]="'/vendas/devolucoes'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DevolucaoFormComponent {
  readonly servico = inject(DevolucaoVendaService);
  readonly campos: CampoFormulario[] = [
    { nome: 'faturamentoId', rotulo: 'ID Faturamento', obrigatorio: true },
    { nome: 'motivo', rotulo: 'Motivo', obrigatorio: true },
    { nome: 'valor', rotulo: 'Valor', tipo: 'number' },
    { nome: 'data', rotulo: 'Data', tipo: 'date' },
    { nome: 'status', rotulo: 'Status' },
  ];
}
