import { Estoque } from './estoque';
import { Produto } from './../product/produto';
import { AbstractEntityCompetencia } from "../abstract-entity-competencia";

export class EntradaProdutoEstoque extends AbstractEntityCompetencia{
  public produtoId:string ;
  public estoqueId: string ;
  public dataEntrada: Date ;

  public estoque: Estoque;
  public produto: Produto;
}
