import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudFormComponent, CampoFormulario } from '@shared/crud/crud-form.component';
import { CentroCustoService } from '../cadastros.services';

@Component({
  selector: 'app-centro-custo-form',
  standalone: true,
  imports: [CrudFormComponent],
  template: `<app-crud-form [titulo]="'Centro de Custo'" [campos]="campos" [servico]="servico" [rotaLista]="'/cadastros/centros-custo'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CentroCustoFormComponent {
  readonly servico = inject(CentroCustoService);
  readonly campos: CampoFormulario[] = [
    { nome: 'codigo', rotulo: 'Código', obrigatorio: true },
    { nome: 'nome', rotulo: 'Nome', obrigatorio: true },
  ];
}
