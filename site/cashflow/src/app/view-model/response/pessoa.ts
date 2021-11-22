import { UsuarioResponse } from './usuario';
export class Pessoa{
  Nome: string = "";
  CpfCnpj: string = "";
  Email: string = "";
  Celular: string = "";
  DataNascimento: Date = new Date();

  Usuarios : UsuarioResponse[] = [];
  Pagamentos: any;
  Vendas: any;
  Dividas : any;
}
