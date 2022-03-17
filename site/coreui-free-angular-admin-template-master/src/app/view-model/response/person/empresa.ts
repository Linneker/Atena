import { EnderecoEmpresa } from "../util/endereco-empresa";
import { Pessoa } from "./pessoa";

export class  Empresa extends Pessoa{
  public  isFilial: boolean;
  public  empresaMatrizId :string;
  public  empresaMatriz: Empresa;

  public  razaoSocial:string;

  public enderecoEmpresas : EnderecoEmpresa[];
}
