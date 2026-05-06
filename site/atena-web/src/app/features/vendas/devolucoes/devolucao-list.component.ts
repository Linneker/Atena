import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudListComponent } from '@shared/crud/crud-list.component';
import { ColunaTabela } from '@shared/data-table/data-table.component';
import { DevolucaoVenda, DevolucaoVendaService } from '../vendas.services';

@Component({
  selector: 'app-devolucao-list',
  standalone: true,
  imports: [CrudListComponent],
  template: `<app-crud-list [titulo]="'Devoluções'" [colunas]="colunas" [servico]="servico" [rotaForm]="'/vendas/devolucoes'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DevolucaoListComponent {
  readonly servico = inject(DevolucaoVendaService);
  readonly colunas: ColunaTabela<DevolucaoVenda>[] = [
    { campo: 'faturamentoId', titulo: 'Faturamento' },
    { campo: 'motivo', titulo: 'Motivo' },
    { campo: 'valor', titulo: 'Valor', formato: (l) => l.valor.toFixed(2) },
    { campo: 'data', titulo: 'Data' },
    { campo: 'status', titulo: 'Status' },
  ];
}
