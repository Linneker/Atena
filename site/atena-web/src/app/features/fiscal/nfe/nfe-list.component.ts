import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudListComponent } from '@shared/crud/crud-list.component';
import { ColunaTabela } from '@shared/data-table/data-table.component';
import { NFe, NFeService } from '../fiscal.services';

@Component({
  selector: 'app-nfe-list',
  standalone: true,
  imports: [CrudListComponent],
  template: `<app-crud-list [titulo]="'NF-e'" [colunas]="colunas" [servico]="servico" [rotaForm]="'/fiscal/nfe'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NFeListComponent {
  readonly servico = inject(NFeService);
  readonly colunas: ColunaTabela<NFe>[] = [
    { campo: 'numero', titulo: 'Número' },
    { campo: 'chave', titulo: 'Chave' },
    { campo: 'cliente', titulo: 'Cliente' },
    { campo: 'valor', titulo: 'Valor', formato: (l) => l.valor.toFixed(2) },
    { campo: 'emissao', titulo: 'Emissão' },
    { campo: 'status', titulo: 'Status' },
  ];
}
