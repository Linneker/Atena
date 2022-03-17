import { Empresa } from "../person/empresa";
import { EnderecoPessoa } from "./endereco-pessoa";

export class EnderecoEmpresa extends EnderecoPessoa{
  public empresaId: string;
  public empresa: Empresa;
}
