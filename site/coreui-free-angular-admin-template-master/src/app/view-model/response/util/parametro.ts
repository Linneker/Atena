import { AbstractEntity } from "../abstract-entity";

export class Parametro extends AbstractEntity {
  public nome: string;
  public descricao: string;
  public valor: string;
  public exibirTela: string;
}
