import { Pessoa } from './pessoa';
export class UsuarioResponse{
  login: string = "";
  senha : string = "";
  termoDeAceite: string = "";
  pessoaId: string = "";
  pessoa : Pessoa= new Pessoa();
  permissaoUsuarios : any;
  pagamentos : any;
  dividas: any;
  compras: any;
  vendas: any;
}
