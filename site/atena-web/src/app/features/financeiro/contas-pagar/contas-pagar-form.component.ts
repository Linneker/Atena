import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { map } from 'rxjs';
import { CrudFormComponent, CampoFormulario } from '@shared/crud/crud-form.component';
import { FornecedorService } from '@features/cadastros/cadastros.services';
import { ContaPagarService } from '../financeiro.services';

@Component({
  selector: 'app-contas-pagar-form',
  standalone: true,
  imports: [CrudFormComponent],
  template: `<app-crud-form [titulo]="'Conta a Pagar'" [campos]="campos" [servico]="servico" [rotaLista]="'/financeiro/contas-pagar'"></app-crud-form>`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ContasPagarFormComponent {
  readonly servico = inject(ContaPagarService);
  private readonly fornecedores = inject(FornecedorService);

  readonly campos: CampoFormulario[] = [
    { nome: 'descricao', rotulo: 'Descrição', obrigatorio: true },
    {
      nome: 'fornecedorId',
      rotulo: 'Fornecedor',
      tipo: 'select',
      placeholderSelect: '— sem fornecedor —',
      opcoes: () => this.fornecedores.listar({ pagina: 1, tamanhoPagina: 100 }).pipe(
        map((p) => p.itens.map((f) => ({ value: f.id ?? '', label: f.nome }))),
      ),
    },
    { nome: 'valorOriginal', rotulo: 'Valor', tipo: 'number', obrigatorio: true },
    { nome: 'dataVencimento', rotulo: 'Vencimento', tipo: 'date', obrigatorio: true },
    { nome: 'observacao', rotulo: 'Observação' },
  ];
}
