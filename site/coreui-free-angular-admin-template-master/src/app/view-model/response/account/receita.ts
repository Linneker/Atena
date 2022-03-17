import { FluxoDeCaixaReceita } from './fluxo-de-caixa-receita';
import { AbstractEntityCompetencia } from "../abstract-entity-competencia";
import { Competencia } from "../util/competencia";

export class Receita extends AbstractEntityCompetencia{
  public nome:string;
  public descricao:string;
  public valor: number ;
  public receitaFixa: number;

  public competenciaId: string;

  public competencia: Competencia;

  public fluxoDeCaixasReceitas: FluxoDeCaixaReceita[];
}
