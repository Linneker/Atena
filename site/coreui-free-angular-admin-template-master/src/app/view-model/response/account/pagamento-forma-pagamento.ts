import { FormaPagamento } from './forma-pagamento';
import { Pagamento } from './pagamento';
import { AbstractEntity } from "../abstract-entity";

export class PagamentoFormaPagamento extends AbstractEntity{
  public pagamentoId : string;
  public formaPagamentoId: string;

  public pagamento : Pagamento;
  public formaPagamento : FormaPagamento;
}
