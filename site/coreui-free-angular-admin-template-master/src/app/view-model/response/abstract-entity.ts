import { EnumStatus } from "./enum/enum-status";

export abstract class AbstractEntity {

  public id: string;
  public dataCriacao :string;
  public dataModificacao :string;
  public dataInativacao :string;

  public status : EnumStatus;
}
