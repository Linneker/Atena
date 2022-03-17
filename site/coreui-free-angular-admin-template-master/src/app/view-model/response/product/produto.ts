import { EntradaProdutoEstoque } from './../inventory/entrada-produto-estoque';
import { SaidaProdutoEstoque } from './../inventory/saida-produto-estoque';
import { EstoqueProduto } from './../inventory/estoque-produto';
import { VendaProduto } from './venda-produto';
import { TipoProduto } from './tipo-produto';
import { DevolucaoCompra } from './devolucao-compra';
import { AbstractEntity } from "../abstract-entity";
import { CompraProduto } from "./compra-produto";
import { ValorProduto } from './price/valor-produto';
import { DevolucaoVenda } from './devolucao-venda';

export class Produto extends AbstractEntity{
  public nome:string;
  public descricao: string;
  public codigo: number;
  public validade: Date;
  public tipoProdutoId: string;
  public tipoProduto: TipoProduto;

  public  ValorProdutos:ValorProduto[];
  public  VendasProdutos: VendaProduto[];
  public  ComprasProdutos:CompraProduto[];
  public  EntradaProdutoEstoques: EntradaProdutoEstoque[];
  public  SaidaProdutoEstoques: SaidaProdutoEstoque[];
  public  EstoqueProdutos:EstoqueProduto[];
  public  DevolucaoCompras : DevolucaoCompra[];
  public  DevolucoesVenda :DevolucaoVenda[];
}
