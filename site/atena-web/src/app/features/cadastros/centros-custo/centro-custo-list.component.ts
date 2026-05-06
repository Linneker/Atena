import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudListComponent } from '@shared/crud/crud-list.component';
import { ColunaTabela } from '@shared/data-table/data-table.component';
import { CentroCusto, CentroCustoService } from '../cadastros.services';

@Component({
  selector: 'app-centro-custo-list',
  standalone: true,
  imports: [CrudListComponent],
  template: `<app-crud-list [titulo]="'Centros de Custo'" [colunas]="colunas" [servico]="servico" [rotaForm]="'/cadastros/centros-custo'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CentroCustoListComponent {
  readonly servico = inject(CentroCustoService);
  readonly colunas: ColunaTabela<CentroCusto>[] = [
    { campo: 'codigo', titulo: 'Código' },
    { campo: 'nome', titulo: 'Nome' },
  ];
}
