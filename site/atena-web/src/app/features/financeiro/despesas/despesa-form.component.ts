import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudFormComponent, CampoFormulario } from '@shared/crud/crud-form.component';
import { DespesaService } from '../financeiro.services';

@Component({
  selector: 'app-despesa-form',
  standalone: true,
  imports: [CrudFormComponent],
  template: `<app-crud-form [titulo]="'Despesa'" [campos]="campos" [servico]="servico" [rotaLista]="'/financeiro/despesas'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DespesaFormComponent {
  readonly servico = inject(DespesaService);
  readonly campos: CampoFormulario[] = [
    { nome: 'descricao', rotulo: 'Descrição', obrigatorio: true },
    { nome: 'valor', rotulo: 'Valor', tipo: 'number', obrigatorio: true },
    { nome: 'vencimento', rotulo: 'Vencimento', tipo: 'date', obrigatorio: true },
    { nome: 'status', rotulo: 'Status' },
  ];
}
