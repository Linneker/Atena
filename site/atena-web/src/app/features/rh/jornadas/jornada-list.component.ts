import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudListComponent } from '@shared/crud/crud-list.component';
import { ColunaTabela } from '@shared/data-table/data-table.component';
import { Jornada, JornadaService } from '../rh.services';

@Component({
  selector: 'app-jornada-list',
  standalone: true,
  imports: [CrudListComponent],
  template: `<app-crud-list [titulo]="'Jornadas de trabalho'" [colunas]="colunas" [servico]="servico" [rotaForm]="'/rh/jornadas'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class JornadaListComponent {
  readonly servico = inject(JornadaService);
  readonly colunas: ColunaTabela<Jornada>[] = [
    { campo: 'nome', titulo: 'Nome' },
    { campo: 'tipo', titulo: 'Tipo' },
    { campo: 'cargaSemanalHoras', titulo: 'Carga semanal (h)' },
    { campo: 'toleranciaMinutos', titulo: 'Tolerância (min)' },
    { campo: 'ativo', titulo: 'Ativa', formato: (j) => (j.ativo ? 'Sim' : 'Não') },
  ];
}
