import { EnderecoFornecedor } from "../util/endereco-fornecedor";
import { Pessoa } from "./pessoa";

export class Fornecedor extends Pessoa{

  public nomeFantasia:string;
  public inscricaoMunicipal:string;

  public enderecoFornecedores: EnderecoFornecedor[];
}
