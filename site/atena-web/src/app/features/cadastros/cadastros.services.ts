import { Injectable } from '@angular/core';
import { CrudService } from '@shared/crud/crud.service';

export interface Cliente {
  id?: string;
  nome: string;
  nomeFantasia?: string | null;
  documento: string;
  email?: string | null;
  telefone?: string | null;
  status?: string;
  inadimplente?: boolean;
}
export interface Fornecedor { id?: string; nome: string; nomeFantasia?: string | null; documento: string; email?: string | null; telefone?: string | null; }
export interface Funcionario {
  id?: string;
  nomeCompleto: string;
  cpf: string;
  email?: string | null;
  telefone?: string | null;
  cargo?: string | null;
  departamento?: string | null;
  centroDeCustoId?: string | null;
  centroDeCustoNome?: string | null;
  dataAdmissao?: string | null;
}
export interface Produto {
  id?: string;
  codigo: string;
  nome: string;
  descricao?: string;
  codigoBarras?: string | null;
  unidadeMedida: string;
  custoMedio?: number | null;
  estoqueMinimo?: number | null;
  tipoProdutoId?: string | null;
  fornecedorId?: string | null;
  fornecedorNome?: string | null;
  status?: string;
}
export interface CentroCusto { id?: string; codigo: string; nome: string; }
export interface PlanoContas { id?: string; codigo: string; descricao: string; tipo: string; paiId?: string; }

@Injectable({ providedIn: 'root' })
export class ClienteService extends CrudService<Cliente> { protected readonly resource = 'clientes'; }

@Injectable({ providedIn: 'root' })
export class FornecedorService extends CrudService<Fornecedor> { protected readonly resource = 'fornecedores'; }

@Injectable({ providedIn: 'root' })
export class FuncionarioService extends CrudService<Funcionario> { protected readonly resource = 'funcionarios'; }

@Injectable({ providedIn: 'root' })
export class ProdutoService extends CrudService<Produto> { protected readonly resource = 'produtos'; }

@Injectable({ providedIn: 'root' })
export class CentroCustoService extends CrudService<CentroCusto> { protected readonly resource = 'centros-de-custo'; }

@Injectable({ providedIn: 'root' })
export class PlanoContasService extends CrudService<PlanoContas> { protected readonly resource = 'plano-de-contas'; }
