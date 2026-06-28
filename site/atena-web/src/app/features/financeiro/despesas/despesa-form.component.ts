import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { map } from 'rxjs';
import { CrudFormComponent, CampoFormulario } from '@shared/crud/crud-form.component';
import { CentroCustoService } from '@features/cadastros/cadastros.services';
import { DespesaService } from '../financeiro.services';

@Component({
  selector: 'app-despesa-form',
  standalone: true,
  imports: [CrudFormComponent],
  template: `<app-crud-form [titulo]="'Despesa'" [campos]="campos" [servico]="servico" [rotaLista]="'/financeiro/despesas'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DespesaFormComponent {
  readonly servico = inject(DespesaService);
  private readonly centros = inject(CentroCustoService);

  readonly campos: CampoFormulario[] = [
    { nome: 'nome', rotulo: 'Nome', obrigatorio: true },
    { nome: 'descricao', rotulo: 'Descrição' },
    { nome: 'valor', rotulo: 'Valor', tipo: 'number', obrigatorio: true },
    { nome: 'dataVencimento', rotulo: 'Vencimento', tipo: 'date', obrigatorio: true },
    {
      nome: 'despesaFixa',
      rotulo: 'Despesa fixa (recorrente)',
      tipo: 'checkbox',
      ajuda: 'Marque se esta despesa se repete todo mês (aluguel, salário, internet etc.).',
    },
    {
      nome: 'centroDeCustoId',
      rotulo: 'Centro de Custo',
      tipo: 'select',
      placeholderSelect: '— sem centro de custo —',
      opcoes: () => this.centros.listar({ pagina: 1, tamanhoPagina: 100 }).pipe(
        map((p) => p.itens.map((c) => ({ value: c.id ?? '', label: `${c.codigo} — ${c.nome}` }))),
      ),
    },
  ];
}
