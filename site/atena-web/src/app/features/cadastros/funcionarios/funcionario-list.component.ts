import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudListComponent } from '@shared/crud/crud-list.component';
import { ColunaTabela } from '@shared/data-table/data-table.component';
import { Funcionario, FuncionarioService } from '../cadastros.services';

@Component({
  selector: 'app-funcionario-list',
  standalone: true,
  imports: [CrudListComponent],
  template: `<app-crud-list [titulo]="'Funcionários'" [colunas]="colunas" [servico]="servico" [rotaForm]="'/cadastros/funcionarios'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FuncionarioListComponent {
  readonly servico = inject(FuncionarioService);
  readonly colunas: ColunaTabela<Funcionario>[] = [
    { campo: 'nomeCompleto', titulo: 'Nome' },
    { campo: 'cpf', titulo: 'CPF' },
    { campo: 'cargo', titulo: 'Cargo' },
    { campo: 'departamento', titulo: 'Departamento' },
    { campo: 'centroDeCustoNome', titulo: 'Centro de Custo', formato: (l) => l.centroDeCustoNome ?? '—' },
    { campo: 'dataAdmissao', titulo: 'Admissão', tipo: 'data' },
  ];
}
