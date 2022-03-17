import { AbstractEntityCompetencia } from "../abstract-entity-competencia";
import { FluxoDeCaixaDespesa } from "./fluxo-de-caixa-despesa";

export class Despesa extends AbstractEntityCompetencia{
  public nome:string;
  public descricao:string;
  public valor: number;
  public despesaFixa: boolean;

  public fluxoDeCaixasDespesas:FluxoDeCaixaDespesa[];
}
