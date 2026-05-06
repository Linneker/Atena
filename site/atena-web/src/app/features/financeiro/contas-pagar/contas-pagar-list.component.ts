import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudListComponent } from '@shared/crud/crud-list.component';
import { ColunaTabela } from '@shared/data-table/data-table.component';
import { ContaPagar, ContaPagarService } from '../financeiro.services';

@Component({
  selector: 'app-contas-pagar-list',
  standalone: true,
  imports: [CrudListComponent],
  template: `<app-crud-list [titulo]="'Contas a Pagar'" [colunas]="colunas" [servico]="servico" [rotaForm]="'/financeiro/contas-pagar'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ContasPagarListComponent {
  readonly servico = inject(ContaPagarService);
  readonly colunas: ColunaTabela<ContaPagar>[] = [
    { campo: 'descricao', titulo: 'Descrição' },
    { campo: 'fornecedor', titulo: 'Fornecedor' },
    { campo: 'valor', titulo: 'Valor', formato: (l) => l.valor.toFixed(2) },
    { campo: 'vencimento', titulo: 'Vencimento' },
    { campo: 'status', titulo: 'Status' },
  ];
}
