import { PagamentoVenda } from '../account/pagamento-venda';
import { VendaProduto } from './venda-produto';
import { UsuarioResponse } from './../security/usuario';
import { AbstractEntityCompetencia } from "../abstract-entity-competencia";
import { Cliente } from "../person/cliente";
import { Empresa } from "../person/empresa";

export class Venda extends AbstractEntityCompetencia{
  public dataVenda: Date ;
  public valorTotal:number;
  public valorPago: number;

  public usuarioId: string;
  public clienteId: string;
  public empresaId:string;

  public  usuario: UsuarioResponse ;
  public  cliente: Cliente ;
  public  empresa: Empresa ;

  public  vendasProdutos:VendaProduto[] ;
  public  pagamentosVendas:PagamentoVenda[] ;
}
