import { Cliente } from "../person/cliente";
import { EnderecoPessoa } from "./endereco-pessoa";

export class EnderecoCliente extends EnderecoPessoa{

  public clienteId: string;

  public cliente: Cliente;
}
