import { Fornecedor } from './../person/fornecedor';
import { EnderecoPessoa } from "./endereco-pessoa";

export class EnderecoFornecedor extends EnderecoPessoa{
  public fornecedorId: string;

  public fornecedor: Fornecedor;
}
