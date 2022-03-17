import { ValorProduto } from "../../response/product/price/valor-produto";

export class ProdutoCadastroRequest {
  public nome:string;
  public descricao: string;
  public codigo: number;
  public validade: Date;
  public tipoProdutoId: string;

  public  valorProdutos:ValorProduto[]=[];


}
