import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudFormComponent, CampoFormulario } from '@shared/crud/crud-form.component';
import { ReceitaService } from '../financeiro.services';

@Component({
  selector: 'app-receita-form',
  standalone: true,
  imports: [CrudFormComponent],
  template: `<app-crud-form [titulo]="'Receita'" [campos]="campos" [servico]="servico" [rotaLista]="'/financeiro/receitas'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ReceitaFormComponent {
  readonly servico = inject(ReceitaService);
  readonly campos: CampoFormulario[] = [
    { nome: 'descricao', rotulo: 'Descrição', obrigatorio: true },
    { nome: 'valor', rotulo: 'Valor', tipo: 'number', obrigatorio: true },
    { nome: 'vencimento', rotulo: 'Recebimento', tipo: 'date', obrigatorio: true },
    { nome: 'status', rotulo: 'Status' },
  ];
}
