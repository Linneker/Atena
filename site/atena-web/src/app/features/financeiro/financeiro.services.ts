import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';
import { CrudService } from '@shared/crud/crud.service';

export interface BaixarContaPagarPayload {
  valorPago: number;
  dataPagamento: string;
  formaPagamento: string;
  observacao?: string | null;
}

export interface ReceberContaReceberPayload {
  valorRecebido: number;
  dataRecebimento: string;
  observacao?: string | null;
}

export interface Despesa {
  id?: string;
  nome: string;
  descricao?: string;
  valor: number;
  dataVencimento: string;
  despesaFixa?: boolean;
  centroDeCustoId?: string | null;
  centroDeCustoNome?: string | null;
  statusPagamento?: string;
  valorPago?: number;
  dataPagamento?: string;
}
export interface Receita {
  id?: string;
  nome: string;
  descricao?: string;
  valor: number;
  dataPrevistaRecebimento: string;
  receitaFixa?: boolean;
  centroDeCustoId?: string | null;
  centroDeCustoNome?: string | null;
  clienteId?: string | null;
  statusRecebimento?: string;
  valorRecebido?: number;
  dataRecebimento?: string;
}

export interface ContaPagar {
  id?: string;
  descricao: string;
  fornecedorId?: string | null;
  fornecedorNome?: string | null;
  valorOriginal: number;
  valorPago?: number;
  saldo?: number;
  dataVencimento: string;
  dataPagamento?: string | null;
  status?: string;
  observacao?: string | null;
}

export interface ContaReceber {
  id?: string;
  descricao: string;
  clienteId?: string | null;
  clienteNome?: string | null;
  valorOriginal: number;
  valorRecebido?: number;
  saldo?: number;
  dataVencimento: string;
  dataRecebimento?: string | null;
  status?: string;
  diasAtraso?: number;
  observacaoRecebimento?: string | null;
}

@Injectable({ providedIn: 'root' })
export class DespesaService extends CrudService<Despesa> {
  protected readonly resource = 'despesas';
  /** Gera entries dos próximos N meses para todas as despesas fixas. */
  gerarRecorrencias(meses: number): Observable<{ geradas: number; ignoradasJaExistentes: number }> {
    return this.http.post<{ geradas: number; ignoradasJaExistentes: number }>(
      `${environment.apiUrl}/${environment.apiVersion}/despesas/gerar-recorrencias`, { meses });
  }
  /** Cria uma ContaPagar vinculada à despesa, copiando descricao/valor/vencimento. */
  gerarContaPagar(d: Despesa): Observable<unknown> {
    return this.http.post(`${environment.apiUrl}/${environment.apiVersion}/contas-pagar`, {
      descricao: d.nome,
      fornecedorId: null,
      despesaId: d.id,
      planoDeContasId: null,
      valorOriginal: d.valor,
      dataVencimento: d.dataVencimento,
      observacao: d.descricao ?? null,
    });
  }
}

@Injectable({ providedIn: 'root' })
export class ReceitaService extends CrudService<Receita> {
  protected readonly resource = 'receitas';
  gerarRecorrencias(meses: number): Observable<{ geradas: number; ignoradasJaExistentes: number }> {
    return this.http.post<{ geradas: number; ignoradasJaExistentes: number }>(
      `${environment.apiUrl}/${environment.apiVersion}/receitas/gerar-recorrencias`, { meses });
  }
  /** Cria uma ContaReceber vinculada à receita, copiando descricao/valor/data prevista. */
  gerarContaReceber(r: Receita): Observable<unknown> {
    return this.http.post(`${environment.apiUrl}/${environment.apiVersion}/contas-receber`, {
      descricao: r.nome,
      clienteId: r.clienteId ?? null,
      receitaId: r.id,
      planoDeContasId: null,
      valorOriginal: r.valor,
      dataVencimento: r.dataPrevistaRecebimento,
    });
  }
}

@Injectable({ providedIn: 'root' })
export class ContaPagarService extends CrudService<ContaPagar> {
  protected readonly resource = 'contas-pagar';
  baixar(id: string, payload: BaixarContaPagarPayload): Observable<unknown> {
    return this.http.post(`${environment.apiUrl}/${environment.apiVersion}/contas-pagar/${id}/baixar`, payload);
  }
}

@Injectable({ providedIn: 'root' })
export class ContaReceberService extends CrudService<ContaReceber> {
  protected readonly resource = 'contas-receber';
  receber(id: string, payload: ReceberContaReceberPayload): Observable<unknown> {
    return this.http.post(`${environment.apiUrl}/${environment.apiVersion}/contas-receber/${id}/receber`, payload);
  }
}
