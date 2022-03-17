import { EnderecoUsuario } from './../util/endereco-usuario';
import { PermissaoUsuario } from './permissao-usuario';
import { Pessoa } from '../person/pessoa';
import { Compra } from '../product/compra';
import { Venda } from '../product/venda';
export class UsuarioResponse extends Pessoa{
  login: string = "";
  senha : string = "";
  termoDeAceite: boolean = false;


  permissaoUsuarios : PermissaoUsuario[]=[];
  compras: Compra[]=[];
  vendas: Venda[]=[];
  enderecoUsuarios: EnderecoUsuario[]=[];
}
