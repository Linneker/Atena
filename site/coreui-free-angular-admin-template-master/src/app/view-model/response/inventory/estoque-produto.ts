import { Produto } from './../product/produto';
import { Estoque } from './estoque';
import { AbstractEntity } from "../abstract-entity";

export class EstoqueProduto extends AbstractEntity{
  public produtoId: string ;
  public estoqueId: string ;
  public quantidadeMaxima: number;
  public quantidadeMinima:number;
  public quantidadeProduto:number;

  public estoque: Estoque ;
  public produto: Produto ;
}
