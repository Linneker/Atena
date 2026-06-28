import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { map } from 'rxjs';
import { CrudFormComponent, CampoFormulario } from '@shared/crud/crud-form.component';
import { FornecedorService, ProdutoService } from '../cadastros.services';

@Component({
  selector: 'app-produto-form',
  standalone: true,
  imports: [CrudFormComponent],
  template: `<app-crud-form [titulo]="'Produto'" [campos]="campos" [servico]="servico" [rotaLista]="'/cadastros/produtos'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ProdutoFormComponent {
  readonly servico = inject(ProdutoService);
  private readonly fornecedores = inject(FornecedorService);

  readonly campos: CampoFormulario[] = [
    { nome: 'codigo', rotulo: 'Código', obrigatorio: true },
    { nome: 'nome', rotulo: 'Nome', obrigatorio: true },
    { nome: 'descricao', rotulo: 'Descrição' },
    { nome: 'codigoBarras', rotulo: 'Código de barras' },
    { nome: 'unidadeMedida', rotulo: 'Unidade de medida', obrigatorio: true },
    { nome: 'custoMedio', rotulo: 'Custo médio', tipo: 'number' },
    { nome: 'estoqueMinimo', rotulo: 'Estoque mínimo', tipo: 'number' },
    {
      nome: 'fornecedorId',
      rotulo: 'Fornecedor principal',
      tipo: 'select',
      placeholderSelect: '— sem fornecedor —',
      opcoes: () => this.fornecedores.listar({ pagina: 1, tamanhoPagina: 100 }).pipe(
        map((p) => p.itens.map((f) => ({ value: f.id ?? '', label: f.nome }))),
      ),
    },
  ];
}
