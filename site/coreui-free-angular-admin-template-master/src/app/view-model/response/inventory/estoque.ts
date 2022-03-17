import { SaidaProdutoEstoque } from './saida-produto-estoque';
import { EstoqueProduto } from './estoque-produto';
import { EntradaProdutoEstoque } from './entrada-produto-estoque';
import { AbstractEntity } from "../abstract-entity";
import { Empresa } from "../person/empresa";

export class Estoque extends AbstractEntity{
  public nome: string ;
  public isPrincipal: boolean ;
  public estoqueMaximo: number ;
  public estoqueMinimo:number ;
  public empresaId: string ;
  public empresa: Empresa;

  public entradaProdutoEstoques: EntradaProdutoEstoque[] ;
  public saidaProdutoEstoques:SaidaProdutoEstoque[] ;
  public estoqueProdutos: EstoqueProduto[];
}
