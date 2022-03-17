import { EnderecoCliente } from './../util/endereco-cliente';
import { Pessoa } from "./pessoa";

export class Cliente extends Pessoa{
  public nomeFantasia :string;
  public inscricaoMunicipal :string;

  enderecoClientes: EnderecoCliente[];
}
