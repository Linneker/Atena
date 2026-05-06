import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudListComponent } from '@shared/crud/crud-list.component';
import { ColunaTabela } from '@shared/data-table/data-table.component';
import { Despesa, DespesaService } from '../financeiro.services';

@Component({
  selector: 'app-despesa-list',
  standalone: true,
  imports: [CrudListComponent],
  template: `<app-crud-list [titulo]="'Despesas'" [colunas]="colunas" [servico]="servico" [rotaForm]="'/financeiro/despesas'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DespesaListComponent {
  readonly servico = inject(DespesaService);
  readonly colunas: ColunaTabela<Despesa>[] = [
    { campo: 'descricao', titulo: 'Descrição' },
    { campo: 'valor', titulo: 'Valor', formato: (l) => l.valor.toFixed(2) },
    { campo: 'vencimento', titulo: 'Vencimento' },
    { campo: 'status', titulo: 'Status' },
  ];
}
