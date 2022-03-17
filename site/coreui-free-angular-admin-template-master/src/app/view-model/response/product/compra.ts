import { Fornecedor } from './../person/fornecedor';
import { UsuarioResponse } from './../security/usuario';
import { AbstractEntityCompetencia } from "../abstract-entity-competencia";
import { CompraProduto } from './compra-produto';

export class  Compra extends AbstractEntityCompetencia{
  public dataCompra : Date;
  public valorTotal: number;
  public usuarioId :string;
  public fornecedorId : string;

  public usuario : UsuarioResponse;
  public fornecedor: Fornecedor;
  public comprasProdutos: CompraProduto[];
}
