import { Produto } from './../produto';
import { AbstractEntity } from "../../abstract-entity";
import { TipoValorProduto } from "./tipo-valor-produto";

export class ValorProduto extends AbstractEntity{
  public valor:number;
  public  produtoId : string;
  public  tipoValorProdutoId : string;

  public produto: Produto;
  public tipoValorProduto: TipoValorProduto;
}
