import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudListComponent } from '@shared/crud/crud-list.component';
import { ColunaTabela } from '@shared/data-table/data-table.component';
import { Cliente, ClienteService } from '../cadastros.services';

@Component({
  selector: 'app-cliente-list',
  standalone: true,
  imports: [CrudListComponent],
  template: `<app-crud-list [titulo]="'Clientes'" [colunas]="colunas" [servico]="servico" [rotaForm]="'/cadastros/clientes'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ClienteListComponent {
  readonly servico = inject(ClienteService);
  readonly colunas: ColunaTabela<Cliente>[] = [
    { campo: 'nome', titulo: 'Nome / Razão Social' },
    { campo: 'documento', titulo: 'CPF/CNPJ' },
    { campo: 'email', titulo: 'E-mail' },
    { campo: 'telefone', titulo: 'Telefone' },
    { campo: 'status', titulo: 'Status' },
  ];
}
