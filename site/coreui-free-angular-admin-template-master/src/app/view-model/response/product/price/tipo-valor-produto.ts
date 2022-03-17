import { AbstractEntity } from "../../abstract-entity";
import { ValorProduto } from "./valor-produto";

export class TipoValorProduto extends AbstractEntity{
  public nome :string;

  public valorProdutos: ValorProduto[];
}
