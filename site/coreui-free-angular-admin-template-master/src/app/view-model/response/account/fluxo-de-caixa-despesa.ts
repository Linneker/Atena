import { Despesa } from './despesa';
import { AbstractEntity } from "../abstract-entity";
import { FluxoDeCaixa } from './fluxo-de-caixa';

export class FluxoDeCaixaDespesa extends AbstractEntity{
  public  fluxoDeCaixaId: string;
  public  despesaId: string;

  public fluxoDeCaixa: FluxoDeCaixa;
  public despesa: Despesa;
}
