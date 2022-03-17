import { Produto } from "./produto";
import { AbstractEntityCompetencia } from "../abstract-entity-competencia";
import { Venda } from "./venda";
import { DevolucaoVenda } from "./devolucao-venda";

export class VendaProduto extends AbstractEntityCompetencia {
  public valor: number;
  public quantidadeVedida: number;
  public dataPagamento: Date;

  public enviado: boolean;
  public vendaId: string;
  public produtoId: string;

  public venda: Venda;
  public produto: Produto;
  public devolucoesVenda: DevolucaoVenda[];
}
