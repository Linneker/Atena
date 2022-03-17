import { FluxoDeCaixaReceita } from './fluxo-de-caixa-receita';
import { AbstractEntityCompetencia } from "../abstract-entity-competencia";
import { FluxoDeCaixaDespesa } from "./fluxo-de-caixa-despesa";

export class FluxoDeCaixa extends AbstractEntityCompetencia{
  public  saldoOperacional:number;
  public  saldoFinalCaixa :number;
  public  saldoInicial :number;

  public fluxoDeCaixasReceitas: FluxoDeCaixaReceita[];
  public fluxoDeCaixasDespesas: FluxoDeCaixaDespesa[];
}
