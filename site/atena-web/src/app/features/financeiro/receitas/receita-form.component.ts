import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { map } from 'rxjs';
import { CrudFormComponent, CampoFormulario } from '@shared/crud/crud-form.component';
import { CentroCustoService } from '@features/cadastros/cadastros.services';
import { ReceitaService } from '../financeiro.services';

@Component({
  selector: 'app-receita-form',
  standalone: true,
  imports: [CrudFormComponent],
  template: `<app-crud-form [titulo]="'Receita'" [campos]="campos" [servico]="servico" [rotaLista]="'/financeiro/receitas'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ReceitaFormComponent {
  readonly servico = inject(ReceitaService);
  private readonly centros = inject(CentroCustoService);

  readonly campos: CampoFormulario[] = [
    { nome: 'nome', rotulo: 'Nome', obrigatorio: true },
    { nome: 'descricao', rotulo: 'Descrição' },
    { nome: 'valor', rotulo: 'Valor', tipo: 'number', obrigatorio: true },
    { nome: 'dataPrevistaRecebimento', rotulo: 'Recebimento previsto', tipo: 'date', obrigatorio: true },
    {
      nome: 'receitaFixa',
      rotulo: 'Receita fixa (recorrente)',
      tipo: 'checkbox',
      ajuda: 'Marque se esta receita se repete todo mês (mensalidade, contrato, assinatura etc.).',
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
