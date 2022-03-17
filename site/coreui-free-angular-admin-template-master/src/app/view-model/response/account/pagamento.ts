import { PagamentoVenda } from './pagamento-venda';
import { UsuarioResponse } from '../security/usuario';
import { AbstractEntityCompetencia } from "../abstract-entity-competencia";
import { EnumTipoPagamento } from "../enum/enum-tipo-pagamento";
import { Cliente } from "../person/cliente";
import { Fornecedor } from "../person/fornecedor";
import { Empresa } from '../person/empresa';
import { PagamentoFormaPagamento } from './pagamento-forma-pagamento';

export class Pagamento extends AbstractEntityCompetencia
{
  public parcelado : boolean;
  public numeroDaParcela : number;
  public dataPagamento: Date;
  public dataCredito: Date;
  public valorPago: number;
  public enumTipoPagamento: EnumTipoPagamento;
  public usuarioId: string;
  public fornecedorId : string;
  public clienteId: string;
  public empresaId: string;
  public pagamentoVendaId: string;

  public  Cliente: Cliente;
  public  Fornecedor: Fornecedor;
  public  Usuario : UsuarioResponse;
  public  Empresa : Empresa;
  public  PagamentoVenda: PagamentoVenda;

  public PagamentosFormasPagamentos:PagamentoFormaPagamento[];

}
