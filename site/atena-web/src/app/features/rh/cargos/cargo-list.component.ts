import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudListComponent } from '@shared/crud/crud-list.component';
import { ColunaTabela } from '@shared/data-table/data-table.component';
import { Cargo, CargoService } from '../rh.services';

@Component({
  selector: 'app-cargo-list',
  standalone: true,
  imports: [CrudListComponent],
  template: `<app-crud-list [titulo]="'Cargos'" [colunas]="colunas" [servico]="servico" [rotaForm]="'/rh/cargos'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CargoListComponent {
  readonly servico = inject(CargoService);
  readonly colunas: ColunaTabela<Cargo>[] = [
    { campo: 'codigo', titulo: 'Código' },
    { campo: 'descricao', titulo: 'Descrição' },
    { campo: 'codigoCbo', titulo: 'CBO' },
    { campo: 'salarioBaseSugerido', titulo: 'Salário sugerido', tipo: 'moeda' },
    { campo: 'ativo', titulo: 'Ativo', formato: (c) => (c.ativo ? 'Sim' : 'Não') },
  ];
}
