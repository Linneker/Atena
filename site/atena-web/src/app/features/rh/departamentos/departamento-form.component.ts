import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { map } from 'rxjs';
import { CampoFormulario, CrudFormComponent } from '@shared/crud/crud-form.component';
import { CentroCustoService } from '@features/cadastros/cadastros.services';
import { DepartamentoService } from '../rh.services';

@Component({
  selector: 'app-departamento-form',
  standalone: true,
  imports: [CrudFormComponent],
  template: `<app-crud-form [titulo]="'Departamento'" [campos]="campos" [servico]="servico" [rotaLista]="'/rh/departamentos'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DepartamentoFormComponent {
  readonly servico = inject(DepartamentoService);
  private readonly centros = inject(CentroCustoService);
  readonly campos: CampoFormulario[] = [
    { nome: 'codigo', rotulo: 'Código' },
    { nome: 'nome', rotulo: 'Nome', obrigatorio: true },
    {
      nome: 'centroDeCustoId', rotulo: 'Centro de Custo', tipo: 'select',
      placeholderSelect: '— nenhum —',
      opcoes: () => this.centros.listar({ pagina: 1, tamanhoPagina: 100 }).pipe(
        map((p) => p.itens.map((c) => ({ value: c.id ?? '', label: `${c.codigo} — ${c.nome}` }))),
      ),
    },
    { nome: 'ativo', rotulo: 'Ativo', tipo: 'checkbox' },
  ];
}
