import { Endereco } from "./endereco";
import { AbstractEntity } from "../abstract-entity";

export abstract class EnderecoPessoa extends AbstractEntity {
  public numero: string;
  public complemento: string;
  public latitude: string;
  public longitude: string;
  public enderecoId: string;

  public endereco: Endereco= new Endereco();
}
