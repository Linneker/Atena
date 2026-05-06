import { Injectable } from '@angular/core';
import { CrudService } from '@shared/crud/crud.service';

export interface Cliente { id?: string; nome: string; cpfCnpj: string; email: string; telefone: string; }
export interface Fornecedor { id?: string; razaoSocial: string; cnpj: string; email: string; telefone: string; }
export interface Funcionario { id?: string; nome: string; cpf: string; cargo: string; departamento: string; }
export interface Produto { id?: string; codigo: string; descricao: string; precoVenda: number; unidade: string; }
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
export class CentroCustoService extends CrudService<CentroCusto> { protected readonly resource = 'centros-custo'; }

@Injectable({ providedIn: 'root' })
export class PlanoContasService extends CrudService<PlanoContas> { protected readonly resource = 'plano-contas'; }
