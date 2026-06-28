import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { map } from 'rxjs';
import { CrudFormComponent, CampoFormulario } from '@shared/crud/crud-form.component';
import { ClienteService } from '@features/cadastros/cadastros.services';
import { ContaReceberService } from '../financeiro.services';

@Component({
  selector: 'app-contas-receber-form',
  standalone: true,
  imports: [CrudFormComponent],
  template: `<app-crud-form [titulo]="'Conta a Receber'" [campos]="campos" [servico]="servico" [rotaLista]="'/financeiro/contas-receber'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ContasReceberFormComponent {
  readonly servico = inject(ContaReceberService);
  private readonly clientes = inject(ClienteService);

  readonly campos: CampoFormulario[] = [
    { nome: 'descricao', rotulo: 'Descrição', obrigatorio: true },
    {
      nome: 'clienteId',
      rotulo: 'Cliente',
      tipo: 'select',
      placeholderSelect: '— sem cliente —',
      opcoes: () => this.clientes.listar({ pagina: 1, tamanhoPagina: 100 }).pipe(
        map((p) => p.itens.map((c) => ({ value: c.id ?? '', label: c.nome }))),
      ),
    },
    { nome: 'valorOriginal', rotulo: 'Valor', tipo: 'number', obrigatorio: true },
    { nome: 'dataVencimento', rotulo: 'Vencimento', tipo: 'date', obrigatorio: true },
    { nome: 'observacaoRecebimento', rotulo: 'Observação' },
  ];
}
