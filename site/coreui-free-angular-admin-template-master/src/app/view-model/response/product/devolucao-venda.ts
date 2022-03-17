import { Produto } from './produto';
import { VendaProduto } from './venda-produto';
import { AbstractEntityCompetencia } from "../abstract-entity-competencia";

export class DevolucaoVenda extends AbstractEntityCompetencia{
  public motivo:string;
  public vendaProdutoId: string;
  public produtoId: string;
  public isParcial :boolean;

  public vendaProduto: VendaProduto;
  public produto: Produto;

}
