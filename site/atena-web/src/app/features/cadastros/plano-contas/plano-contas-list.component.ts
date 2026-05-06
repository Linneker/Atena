import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudListComponent } from '@shared/crud/crud-list.component';
import { ColunaTabela } from '@shared/data-table/data-table.component';
import { PlanoContas, PlanoContasService } from '../cadastros.services';

@Component({
  selector: 'app-plano-contas-list',
  standalone: true,
  imports: [CrudListComponent],
  template: `<app-crud-list [titulo]="'Plano de Contas'" [colunas]="colunas" [servico]="servico" [rotaForm]="'/cadastros/plano-contas'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class PlanoContasListComponent {
  readonly servico = inject(PlanoContasService);
  readonly colunas: ColunaTabela<PlanoContas>[] = [
    { campo: 'codigo', titulo: 'Código' },
    { campo: 'descricao', titulo: 'Descrição' },
    { campo: 'tipo', titulo: 'Tipo' },
  ];
}
