import { Pessoa } from './pessoa';
export class UsuarioResponse{
  Login: string = "";
  Senha : string = "";
  TermoDeAceite: string = "";
  PessoaId: string = "";
  Pessoa : Pessoa= new Pessoa();
  PermissaoUsuarios : any;
  Pagamentos : any;
  Dividas: any;
  Compras: any;
  Vendas: any;
}
