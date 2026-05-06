import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudListComponent } from '@shared/crud/crud-list.component';
import { ColunaTabela } from '@shared/data-table/data-table.component';
import { SolicitacaoCompra, SolicitacaoCompraService } from '../compras.services';

@Component({
  selector: 'app-solicitacao-list',
  standalone: true,
  imports: [CrudListComponent],
  template: `<app-crud-list [titulo]="'Solicitações de Compra'" [colunas]="colunas" [servico]="servico" [rotaForm]="'/compras/solicitacoes'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SolicitacaoListComponent {
  readonly servico = inject(SolicitacaoCompraService);
  readonly colunas: ColunaTabela<SolicitacaoCompra>[] = [
    { campo: 'numero', titulo: 'Número' },
    { campo: 'solicitante', titulo: 'Solicitante' },
    { campo: 'status', titulo: 'Status' },
    { campo: 'total', titulo: 'Total', formato: (l) => l.total.toFixed(2) },
    { campo: 'criadaEm', titulo: 'Criada em' },
  ];
}
