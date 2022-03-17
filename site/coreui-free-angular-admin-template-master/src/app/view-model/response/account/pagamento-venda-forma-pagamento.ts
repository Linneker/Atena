import { AbstractEntity } from '../abstract-entity';
import { FormaPagamento } from './forma-pagamento';
import { PagamentoVenda } from './pagamento-venda';

export class PagamentoVendaFormaPagamento extends AbstractEntity{
  public pagamentoVendaId :string;
  public formaPagamentoId: string;

  public  pagamentoVenda: PagamentoVenda;
  public  formaPagamento: FormaPagamento;
}
