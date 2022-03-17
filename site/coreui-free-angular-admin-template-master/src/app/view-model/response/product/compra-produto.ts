import { DevolucaoCompra } from './devolucao-compra';
import { Produto } from './produto';
import { AbstractEntityCompetencia } from "../abstract-entity-competencia";
import { Compra } from "./compra";

export class CompraProduto extends AbstractEntityCompetencia{

  public valor: number;
  public quantidadeComprada: number;
  public dataPagamento: Date;
  public isRecebido: boolean;

  public produtoId :string;
  public compraId: string;

  public produto: Produto;
  public compra: Compra;
  public devolucaoCompras: DevolucaoCompra[];


}
