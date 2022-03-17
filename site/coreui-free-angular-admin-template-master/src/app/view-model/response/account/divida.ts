import { UsuarioResponse } from '../security/usuario';
import { Fornecedor } from '../person/fornecedor';
import { AbstractEntityCompetencia } from "../abstract-entity-competencia";
import { Cliente } from "../person/cliente";
import { Empresa } from '../person/empresa';
import { EnumTipoDivida } from '../enum/enum-tipo-divida';

export class Divida extends AbstractEntityCompetencia{
  public enumTipoDivida: EnumTipoDivida;
  public valor: number;
  public dataQueDeviaTerRecebido: Date;
  public dataCompra: Date;
  public usuarioId: string;
  public fornecedorId: string;
  public clienteId: string;
  public empresaId: string;

  public cliente : Cliente ;
  public fornecedor: Fornecedor;
  public usuario: UsuarioResponse;
  public empresa: Empresa;

}
