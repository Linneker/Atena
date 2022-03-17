import { Produto } from './produto';
import { AbstractEntityCompetencia } from "../abstract-entity-competencia";
import { CompraProduto } from "./compra-produto";

export class DevolucaoCompra extends AbstractEntityCompetencia{
  public  motivo:string;
  public  compraProdutoId:string;
  public  produtoId:string;
  public  isParcial:boolean;

  public  compraProduto: CompraProduto;
  public  produto: Produto;
}
