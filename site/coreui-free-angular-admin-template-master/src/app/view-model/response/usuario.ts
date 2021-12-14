import { Pessoa } from './pessoa';
export class UsuarioResponse{
  login: string = "";
  senha : string = "";
  termoDeAceite: string = "";
  nome:string = "";
  cpfCnpj: string;
  email: string ;
  celular:string;
  dataNascimento: Date;
  permissaoUsuarios : any;
  pagamentos : any;
  dividas: any;
  compras: any;
  vendas: any;
}
