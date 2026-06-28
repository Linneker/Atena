import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { map } from 'rxjs';
import { CampoFormulario, CrudFormComponent } from '@shared/crud/crud-form.component';
import { CargoService, CboService } from '../rh.services';

@Component({
  selector: 'app-cargo-form',
  standalone: true,
  imports: [CrudFormComponent],
  template: `<app-crud-form [titulo]="'Cargo'" [campos]="campos" [servico]="servico" [rotaLista]="'/rh/cargos'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CargoFormComponent {
  readonly servico = inject(CargoService);
  private readonly cbos = inject(CboService);
  readonly campos: CampoFormulario[] = [
    { nome: 'codigo', rotulo: 'Código' },
    { nome: 'descricao', rotulo: 'Descrição', obrigatorio: true },
    {
      nome: 'codigoCbo', rotulo: 'CBO', tipo: 'select', placeholderSelect: '— selecionar —',
      opcoes: () => this.cbos.listar().pipe(
        map((r) => r.items.map((c) => ({ value: c.codigo, label: `${c.codigo} — ${c.titulo}` }))),
      ),
    },
    { nome: 'salarioBaseSugerido', rotulo: 'Salário base sugerido', tipo: 'number' },
    { nome: 'ativo', rotulo: 'Ativo', tipo: 'checkbox' },
  ];
}
