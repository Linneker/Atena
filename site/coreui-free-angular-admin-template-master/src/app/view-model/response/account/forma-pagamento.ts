import { PagamentoVendaFormaPagamento } from './pagamento-venda-forma-pagamento';
import { AbstractEntity } from "../abstract-entity";
import { PagamentoFormaPagamento } from "./pagamento-forma-pagamento";

export class FormaPagamento extends AbstractEntity
{
  public  nome :string;
  public  codigo : number;

  public pagamentoFormaPagamentos : PagamentoFormaPagamento[];
  public pagamentoVendasFormasPagamentos: PagamentoVendaFormaPagamento[];

}
