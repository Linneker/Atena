import { Produto } from './../product/produto';
import { Estoque } from './estoque';
import { AbstractEntityCompetencia } from "../abstract-entity-competencia";

export class SaidaProdutoEstoque extends AbstractEntityCompetencia{
  public produtoId: string;
  public estoqueId: string;
  public quantidade: number;
  public dataSaida: Date;

  public  estoque: Estoque;
  public  produto: Produto;

}
