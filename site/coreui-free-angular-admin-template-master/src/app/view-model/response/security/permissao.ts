import { AbstractEntity } from "../abstract-entity";
import { PermissaoUsuario } from "./permissao-usuario";

export class Permissao extends AbstractEntity{
  public  nome: string;
  public  nivel: number;

  public PermissaoUsuarios: PermissaoUsuario[];
}
