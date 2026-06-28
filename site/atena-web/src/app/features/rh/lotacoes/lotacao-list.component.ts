import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudListComponent } from '@shared/crud/crud-list.component';
import { ColunaTabela } from '@shared/data-table/data-table.component';
import { Lotacao, LotacaoService } from '../rh.services';

@Component({
  selector: 'app-lotacao-list',
  standalone: true,
  imports: [CrudListComponent],
  template: `<app-crud-list [titulo]="'Lotações'" [colunas]="colunas" [servico]="servico" [rotaForm]="'/rh/lotacoes'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LotacaoListComponent {
  readonly servico = inject(LotacaoService);
  readonly colunas: ColunaTabela<Lotacao>[] = [
    { campo: 'nome', titulo: 'Nome' },
    { campo: 'cnpj', titulo: 'CNPJ' },
    { campo: 'ativo', titulo: 'Ativa', formato: (l) => (l.ativo ? 'Sim' : 'Não') },
  ];
}
