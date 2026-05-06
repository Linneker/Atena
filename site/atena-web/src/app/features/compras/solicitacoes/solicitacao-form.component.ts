import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudFormComponent, CampoFormulario } from '@shared/crud/crud-form.component';
import { SolicitacaoCompraService } from '../compras.services';

@Component({
  selector: 'app-solicitacao-form',
  standalone: true,
  imports: [CrudFormComponent],
  template: `<app-crud-form [titulo]="'Solicitação de Compra'" [campos]="campos" [servico]="servico" [rotaLista]="'/compras/solicitacoes'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SolicitacaoFormComponent {
  readonly servico = inject(SolicitacaoCompraService);
  readonly campos: CampoFormulario[] = [
    { nome: 'numero', rotulo: 'Número' },
    { nome: 'solicitante', rotulo: 'Solicitante', obrigatorio: true },
    { nome: 'status', rotulo: 'Status' },
    { nome: 'total', rotulo: 'Total', tipo: 'number' },
  ];
}
