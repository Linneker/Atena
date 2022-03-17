import { PagamentoVendaFormaPagamento } from './pagamento-venda-forma-pagamento';
import { Pagamento } from './pagamento';
import { AbstractEntityCompetencia } from "../abstract-entity-competencia";
import { Venda } from "../product/venda";
import { EnumStatusPagamento } from '../enum/enum-status-pagamento';

export class PagamentoVenda extends AbstractEntityCompetencia{
  public statusPagamento: EnumStatusPagamento;
  public parcelado: boolean ;
  public numeroDeParcela: number;
  public diaVencimentoParcela: number;
  public dataPagamento: Date;
  public dataCredito: Date

  public vendaId: string ;

  public cenda: Venda ;
  public pagamentoVendasFormasPagamentos:PagamentoVendaFormaPagamento[];
  public pagamentos:Pagamento[];

}
