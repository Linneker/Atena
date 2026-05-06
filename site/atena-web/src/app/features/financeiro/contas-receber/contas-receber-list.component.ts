import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudListComponent } from '@shared/crud/crud-list.component';
import { ColunaTabela } from '@shared/data-table/data-table.component';
import { ContaReceber, ContaReceberService } from '../financeiro.services';

@Component({
  selector: 'app-contas-receber-list',
  standalone: true,
  imports: [CrudListComponent],
  template: `<app-crud-list [titulo]="'Contas a Receber'" [colunas]="colunas" [servico]="servico" [rotaForm]="'/financeiro/contas-receber'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ContasReceberListComponent {
  readonly servico = inject(ContaReceberService);
  readonly colunas: ColunaTabela<ContaReceber>[] = [
    { campo: 'descricao', titulo: 'Descrição' },
    { campo: 'cliente', titulo: 'Cliente' },
    { campo: 'valor', titulo: 'Valor', formato: (l) => l.valor.toFixed(2) },
    { campo: 'vencimento', titulo: 'Vencimento' },
    { campo: 'status', titulo: 'Status' },
  ];
}
