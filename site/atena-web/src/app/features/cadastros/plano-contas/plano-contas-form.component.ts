import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudFormComponent, CampoFormulario } from '@shared/crud/crud-form.component';
import { PlanoContasService } from '../cadastros.services';

@Component({
  selector: 'app-plano-contas-form',
  standalone: true,
  imports: [CrudFormComponent],
  template: `<app-crud-form [titulo]="'Plano de Contas'" [campos]="campos" [servico]="servico" [rotaLista]="'/cadastros/plano-contas'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PlanoContasFormComponent {
  readonly servico = inject(PlanoContasService);
  readonly campos: CampoFormulario[] = [
    { nome: 'codigo', rotulo: 'Código', obrigatorio: true },
    { nome: 'descricao', rotulo: 'Descrição', obrigatorio: true },
    { nome: 'tipo', rotulo: 'Tipo (Receita/Despesa/Ativo/Passivo)', obrigatorio: true },
    { nome: 'paiId', rotulo: 'Conta pai (id)' },
  ];
}
