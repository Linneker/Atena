import { UsuarioResponse } from './../security/usuario';
import { AbstractEntity } from "../abstract-entity";
import { EnderecoPessoa } from './endereco-pessoa';

export class EnderecoUsuario extends EnderecoPessoa{

  public usuarioId : string;
  public usuario: UsuarioResponse= new UsuarioResponse();
}
