import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';
import { CrudService } from '@shared/crud/crud.service';

export interface Orcamento { id?: string; numero: string; cliente: string; validade: string; total: number; status: string; }
export interface PedidoVenda {
  id?: string;
  numero: string;
  clienteId: string;
  clienteNome?: string | null;
  vendedorId?: string | null;
  vendedorNome?: string | null;
  dataEmissao: string;
  valorTotal: number;
  status: string;
}
export interface Faturamento {
  id?: string;
  numero: string;
  pedidoVendaId: string;
  dataFaturamento: string;
  tipo: string;
  valorTotal: number;
  nfeId?: string | null;
  contaReceberId?: string | null;
}
export interface DevolucaoVenda { id?: string; faturamentoId: string; motivo: string; valor: number; data: string; status: string; }

@Injectable({ providedIn: 'root' })
export class OrcamentoService extends CrudService<Orcamento> {
  protected readonly resource = 'orcamentos';
  converterEmPedido(id: string): Observable<unknown> {
    return this.http.post(`${environment.apiUrl}/${environment.apiVersion}/orcamentos/${id}/converter`, {});
  }
}

export interface PedidoVendaDetalhe {
  id: string;
  numero: string;
  clienteId: string;
  clienteNome?: string | null;
  vendedorId?: string | null;
  dataEmissao: string;
  valorTotal: number;
  status: string;
  condicaoPagamento?: string | null;
  observacao?: string | null;
  itens: PedidoVendaItemDetalhe[];
}

export interface PedidoVendaItemDetalhe {
  id: string;
  produtoId: string;
  produtoNome?: string | null;
  quantidade: number;
  quantidadeFaturada: number;
  quantidadePendente: number;
  precoUnitario: number;
  total: number;
}

export interface FaturarPedidoItem {
  pedidoVendaItemId: string;
  quantidade: number;
}

export interface FaturarPedidoPayload {
  pedidoVendaId: string;
  vencimentoContaReceber: string;
  planoDeContasId?: string | null;
  percentualComissaoOverride?: number | null;
  itens: FaturarPedidoItem[];
}

export interface CriarPedidoVendaItem {
  produtoId: string;
  quantidade: number;
  precoUnitario: number;
}

export interface CriarPedidoVendaPayload {
  clienteId: string;
  vendedorId?: string | null;
  estoqueId: string;
  orcamentoId?: string | null;
  descontoPercentual?: number | null;
  condicaoPagamento?: string | null;
  observacao?: string | null;
  itens: CriarPedidoVendaItem[];
}

@Injectable({ providedIn: 'root' })
export class PedidoVendaService extends CrudService<PedidoVenda> {
  protected readonly resource = 'pedidos-venda';
  confirmar(id: string): Observable<unknown> {
    return this.http.post(`${environment.apiUrl}/${environment.apiVersion}/pedidos-venda/${id}/confirmar`, {});
  }
  obterDetalhe(id: string): Observable<PedidoVendaDetalhe> {
    return this.http.get<PedidoVendaDetalhe>(
      `${environment.apiUrl}/${environment.apiVersion}/pedidos-venda/${id}`);
  }
  criarComItens(payload: CriarPedidoVendaPayload): Observable<{ id: string; numero: string; valorTotal: number }> {
    return this.http.post<{ id: string; numero: string; valorTotal: number }>(
      `${environment.apiUrl}/${environment.apiVersion}/pedidos-venda`, payload);
  }
}

export interface FaturamentoItemDetalhe {
  id: string;
  pedidoVendaItemId: string;
  produtoId: string;
  produtoNome?: string | null;
  quantidade: number;
  precoUnitario: number;
  total: number;
}

export interface FaturamentoDetalhe {
  id: string;
  numero: string;
  pedidoVendaId: string;
  dataFaturamento: string;
  tipo: string;
  valorTotal: number;
  nfeId?: string | null;
  contaReceberId?: string | null;
  observacao?: string | null;
  itens: FaturamentoItemDetalhe[];
}

export interface RegistrarDevolucaoItem {
  faturamentoItemId: string;
  quantidade: number;
}

export interface RegistrarDevolucaoPayload {
  faturamentoId: string;
  estoqueDestinoId: string;
  motivo?: string | null;
  itens: RegistrarDevolucaoItem[];
}

@Injectable({ providedIn: 'root' })
export class FaturamentoService extends CrudService<Faturamento> {
  protected readonly resource = 'faturamentos';
  faturarPedido(payload: FaturarPedidoPayload): Observable<unknown> {
    return this.http.post(`${environment.apiUrl}/${environment.apiVersion}/faturamentos`, payload);
  }
  obterDetalhe(id: string): Observable<FaturamentoDetalhe> {
    return this.http.get<FaturamentoDetalhe>(
      `${environment.apiUrl}/${environment.apiVersion}/faturamentos/${id}`);
  }
}

@Injectable({ providedIn: 'root' })
export class DevolucaoVendaService extends CrudService<DevolucaoVenda> {
  protected readonly resource = 'devolucoes-venda';
  registrar(payload: RegistrarDevolucaoPayload): Observable<{ devolucaoId: string; numero: string }> {
    return this.http.post<{ devolucaoId: string; numero: string }>(
      `${environment.apiUrl}/${environment.apiVersion}/devolucoes-venda`, payload);
  }
}
