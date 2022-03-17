import { Permissao } from './permissao';
import { UsuarioResponse } from './usuario';
import { AbstractEntity } from "../abstract-entity";

export class PermissaoUsuario extends AbstractEntity{

  public  read: boolean;
  public  add: boolean;
  public  delete: boolean;
  public  update : boolean;

  public usuarioId : string;
  public permissaoId : string;

  public usuario : UsuarioResponse;
  public permissao: Permissao = new Permissao();

}
