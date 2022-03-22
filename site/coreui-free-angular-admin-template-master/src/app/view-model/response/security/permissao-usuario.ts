import { Permissao } from './permissao';
import { UsuarioResponse } from './usuario';
import { AbstractEntity } from "../abstract-entity";
import { Tela } from './tela';

export class PermissaoUsuario extends AbstractEntity{

  public  acesso: string;

  public usuarioId : string;
  public permissaoId : string;
  public telaId: string;

  public usuario : UsuarioResponse;
  public permissao: Permissao = new Permissao();
  public tela: Tela = new Tela();
}
