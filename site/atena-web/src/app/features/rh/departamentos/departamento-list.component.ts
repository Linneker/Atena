import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudListComponent } from '@shared/crud/crud-list.component';
import { ColunaTabela } from '@shared/data-table/data-table.component';
import { Departamento, DepartamentoService } from '../rh.services';

@Component({
  selector: 'app-departamento-list',
  standalone: true,
  imports: [CrudListComponent],
  template: `<app-crud-list [titulo]="'Departamentos'" [colunas]="colunas" [servico]="servico" [rotaForm]="'/rh/departamentos'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DepartamentoListComponent {
  readonly servico = inject(DepartamentoService);
  readonly colunas: ColunaTabela<Departamento>[] = [
    { campo: 'codigo', titulo: 'Código' },
    { campo: 'nome', titulo: 'Nome' },
    { campo: 'ativo', titulo: 'Ativo', formato: (d) => (d.ativo ? 'Sim' : 'Não') },
  ];
}
