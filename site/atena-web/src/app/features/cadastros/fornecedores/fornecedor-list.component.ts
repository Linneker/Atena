import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudListComponent } from '@shared/crud/crud-list.component';
import { ColunaTabela } from '@shared/data-table/data-table.component';
import { Fornecedor, FornecedorService } from '../cadastros.services';

@Component({
  selector: 'app-fornecedor-list',
  standalone: true,
  imports: [CrudListComponent],
  template: `<app-crud-list [titulo]="'Fornecedores'" [colunas]="colunas" [servico]="servico" [rotaForm]="'/cadastros/fornecedores'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FornecedorListComponent {
  readonly servico = inject(FornecedorService);
  readonly colunas: ColunaTabela<Fornecedor>[] = [
    { campo: 'nome', titulo: 'Nome / Razão Social' },
    { campo: 'documento', titulo: 'CPF/CNPJ' },
    { campo: 'email', titulo: 'E-mail' },
    { campo: 'telefone', titulo: 'Telefone' },
  ];
}
