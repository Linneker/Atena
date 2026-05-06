import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudListComponent } from '@shared/crud/crud-list.component';
import { ColunaTabela } from '@shared/data-table/data-table.component';
import { Orcamento, OrcamentoService } from '../vendas.services';

@Component({
  selector: 'app-orcamento-list',
  standalone: true,
  imports: [CrudListComponent],
  template: `<app-crud-list [titulo]="'Orçamentos'" [colunas]="colunas" [servico]="servico" [rotaForm]="'/vendas/orcamentos'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class OrcamentoListComponent {
  readonly servico = inject(OrcamentoService);
  readonly colunas: ColunaTabela<Orcamento>[] = [
    { campo: 'numero', titulo: 'Número' },
    { campo: 'cliente', titulo: 'Cliente' },
    { campo: 'validade', titulo: 'Validade' },
    { campo: 'total', titulo: 'Total', formato: (l) => l.total.toFixed(2) },
    { campo: 'status', titulo: 'Status' },
  ];
}
