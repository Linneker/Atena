import { AbstractEntity } from './abstract-entity';
import { Competencia } from './util/competencia';

export abstract class AbstractEntityCompetencia extends AbstractEntity {
  public competenciaId : string;
  public competencia: Competencia;
}
