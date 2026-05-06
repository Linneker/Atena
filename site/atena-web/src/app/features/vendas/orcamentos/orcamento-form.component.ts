import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudFormComponent, CampoFormulario } from '@shared/crud/crud-form.component';
import { OrcamentoService } from '../vendas.services';

@Component({
  selector: 'app-orcamento-form',
  standalone: true,
  imports: [CrudFormComponent],
  template: `<app-crud-form [titulo]="'Orçamento'" [campos]="campos" [servico]="servico" [rotaLista]="'/vendas/orcamentos'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class OrcamentoFormComponent {
  readonly servico = inject(OrcamentoService);
  readonly campos: CampoFormulario[] = [
    { nome: 'numero', rotulo: 'Número' },
    { nome: 'cliente', rotulo: 'Cliente', obrigatorio: true },
    { nome: 'validade', rotulo: 'Validade', tipo: 'date' },
    { nome: 'total', rotulo: 'Total', tipo: 'number' },
    { nome: 'status', rotulo: 'Status' },
  ];
}
