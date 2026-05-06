import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudListComponent } from '@shared/crud/crud-list.component';
import { ColunaTabela } from '@shared/data-table/data-table.component';
import { Faturamento, FaturamentoService } from '../vendas.services';

@Component({
  selector: 'app-faturamento-list',
  standalone: true,
  imports: [CrudListComponent],
  template: `<app-crud-list [titulo]="'Faturamentos'" [colunas]="colunas" [servico]="servico" [rotaForm]="'/vendas/faturamentos'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FaturamentoListComponent {
  readonly servico = inject(FaturamentoService);
  readonly colunas: ColunaTabela<Faturamento>[] = [
    { campo: 'pedidoNumero', titulo: 'Pedido' },
    { campo: 'numeroNota', titulo: 'Nota' },
    { campo: 'valor', titulo: 'Valor', formato: (l) => l.valor.toFixed(2) },
    { campo: 'data', titulo: 'Data' },
    { campo: 'status', titulo: 'Status' },
  ];
}
