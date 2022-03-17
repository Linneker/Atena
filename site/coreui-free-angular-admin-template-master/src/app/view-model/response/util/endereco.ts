import { EnderecoCliente } from './endereco-cliente';
import { EnderecoUsuario } from './endereco-usuario';
import { AbstractEntity } from "../abstract-entity";
import { EnderecoFornecedor } from './endereco-fornecedor';
import { EnderecoEmpresa } from './endereco-empresa';

export class Endereco extends AbstractEntity{
   cep : string;
   pais: string;
   estado: string;
   cidade: string;
   bairro: string;
   rua: string;

  enderecoUsuarios : EnderecoUsuario[]= [];
  enderecoClientes : EnderecoCliente[]= [];
  enderecoFornecedores : EnderecoFornecedor[]= [];
  enderecoEmpresas: EnderecoEmpresa[] = [];
}
