import { SaidaProdutoEstoque } from './../inventory/saida-produto-estoque';
import { EntradaProdutoEstoque } from './../inventory/entrada-produto-estoque';
import { PagamentoVenda } from '../account/pagamento-venda';
import { Pagamento } from '../account/pagamento';
import { Receita } from '../account/receita';
import { DevolucaoCompra } from "./../product/devolucao-compra";
import { Divida } from "../account/divida";
import { VendaProduto } from "./../product/venda-produto";
import { Despesa } from "../account/despesa";
import { AbstractEntity } from "../abstract-entity";
import { Compra } from "../product/compra";
import { CompraProduto } from "../product/compra-produto";
import { Venda } from "../product/venda";
import { FluxoDeCaixa } from "../account/fluxo-de-caixa";
import { DevolucaoVenda } from "../product/devolucao-venda";

export class Competencia extends AbstractEntity {
  ano: number;
  mes: number;

  public despesas: Despesa[];
  public fluxosDeCasas: FluxoDeCaixa[];
  public receitas: Receita[];

  public dividas: Divida[];
  public pagamentos: Pagamento[];
  public compras: Compra[];
  public vendas: Venda[];
  public vendasProdutos: VendaProduto[];
  public comprasProdutos: CompraProduto[];
  public devolucaoCompras: DevolucaoCompra[];
  public devolucoesVenda: DevolucaoVenda[];
  public entradaProdutoEstoques: EntradaProdutoEstoque[];
  public saidaProdutoEstoques: SaidaProdutoEstoque[];
  public pagamentosVendas: PagamentoVenda[];
}
