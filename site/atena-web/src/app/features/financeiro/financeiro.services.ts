import { Injectable } from '@angular/core';
import { CrudService } from '@shared/crud/crud.service';

export interface Despesa { id?: string; descricao: string; valor: number; vencimento: string; status: string; }
export interface Receita { id?: string; descricao: string; valor: number; vencimento: string; status: string; }
export interface ContaPagar { id?: string; descricao: string; fornecedor: string; valor: number; vencimento: string; status: string; }
export interface ContaReceber { id?: string; descricao: string; cliente: string; valor: number; vencimento: string; status: string; }

@Injectable({ providedIn: 'root' })
export class DespesaService extends CrudService<Despesa> { protected readonly resource = 'despesas'; }

@Injectable({ providedIn: 'root' })
export class ReceitaService extends CrudService<Receita> { protected readonly resource = 'receitas'; }

@Injectable({ providedIn: 'root' })
export class ContaPagarService extends CrudService<ContaPagar> { protected readonly resource = 'contas-pagar'; }

@Injectable({ providedIn: 'root' })
export class ContaReceberService extends CrudService<ContaReceber> { protected readonly resource = 'contas-receber'; }
