import { Divida } from './../account/divida';
import { Pagamento } from './../account/pagamento';
import { AbstractEntity } from '../abstract-entity';
import { UsuarioResponse } from '../security/usuario';

export abstract class Pessoa extends AbstractEntity{
  nome: string = "";
  cpfCnpj: string = "";
  email: string = "";
  celular: string = "";
  dataNascimento: Date = new Date();

  pagamentos: Pagamento[]= [];
  dividas : Divida[]= [];
}
