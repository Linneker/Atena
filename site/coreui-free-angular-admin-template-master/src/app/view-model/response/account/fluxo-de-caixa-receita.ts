import { Receita } from './receita';
import { AbstractEntity } from "../abstract-entity";
import { FluxoDeCaixa } from "./fluxo-de-caixa";

export class FluxoDeCaixaReceita extends AbstractEntity{
  public  fluxoDeCaixaId :string;
  public  receitaId :string;

  public fluxoDeCaixa: FluxoDeCaixa;
  public receita :Receita;
}
