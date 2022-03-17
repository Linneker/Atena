import { Produto } from './produto';
import { AbstractEntity } from "../abstract-entity";

export class TipoProduto extends AbstractEntity{
  public nome:string ;
  public produtos: Produto[] ;
}
