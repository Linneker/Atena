import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';
import { CrudService } from '@shared/crud/crud.service';

export interface SolicitacaoCompra {
  id?: string;
  numero: string;
  solicitanteId?: string | null;
  valorTotal: number;
  dataSolicitacao: string;
  status: string;
}
export interface PedidoCompra {
  id?: string;
  numero: string;
  fornecedorId: string;
  fornecedorNome?: string | null;
  dataEmissao: string;
  previsaoEntrega?: string | null;
  valorTotal: number;
  status: string;
}
export interface RecebimentoCompra { id?: string; pedidoNumero: string; data: string; tipo: string; status: string; }

export interface PedidoCompraItemDetalhe {
  id: string;
  produtoId: string;
  produtoNome?: string | null;
  quantidade: number;
  quantidadeRecebida: number;
  quantidadePendente: number;
  precoUnitario: number;
  total: number;
}

export interface PedidoCompraDetalhe {
  id: string;
  numero: string;
  fornecedorId: string;
  fornecedorNome?: string | null;
  dataEmissao: string;
  previsaoEntrega?: string | null;
  valorTotal: number;
  status: string;
  condicaoPagamento?: string | null;
  observacao?: string | null;
  itens: PedidoCompraItemDetalhe[];
}

export interface RegistrarRecebimentoItem {
  pedidoCompraItemId: string;
  quantidadeRecebida: number;
  precoUnitario?: number | null;
  observacao?: string | null;
}

export interface RegistrarRecebimentoPayload {
  pedidoCompraId: string;
  estoqueId: string;
  dataRecebimento?: string | null;
  numeroNotaFiscal?: string | null;
  chaveAcessoNFe?: string | null;
  observacao?: string | null;
  vencimentoContaPagar: string;
  planoDeContasId?: string | null;
  itens: RegistrarRecebimentoItem[];
}

export interface SolicitacaoItem {
  id: string;
  produtoId: string;
  quantidade: number;
  precoEstimado?: number | null;
  observacao?: string | null;
}

export interface SolicitacaoDetalhe {
  id: string;
  numero: string;
  solicitanteId?: string | null;
  justificativa?: string | null;
  valorTotal: number;
  dataSolicitacao: string;
  status: string;
  aprovadoPor?: string | null;
  aprovadoEm?: string | null;
  motivoRejeicao?: string | null;
  itens: SolicitacaoItem[];
}

@Injectable({ providedIn: 'root' })
export class SolicitacaoCompraService extends CrudService<SolicitacaoCompra> {
  protected readonly resource = 'solicitacoes-compra';
  aprovar(id: string): Observable<unknown> {
    return this.http.post(`${environment.apiUrl}/${environment.apiVersion}/solicitacoes-compra/${id}/aprovar`, {});
  }
  enviarParaAprovacao(id: string): Observable<unknown> {
    return this.http.post(`${environment.apiUrl}/${environment.apiVersion}/solicitacoes-compra/${id}/enviar-aprovacao`, {});
  }
  obterDetalhe(id: string): Observable<SolicitacaoDetalhe> {
    return this.http.get<SolicitacaoDetalhe>(
      `${environment.apiUrl}/${environment.apiVersion}/solicitacoes-compra/${id}`);
  }
}

export interface CriarPedidoCompraItem {
  produtoId: string;
  quantidade: number;
  precoUnitario: number;
}

export interface CriarPedidoCompraPayload {
  fornecedorId: string;
  solicitacaoCompraId?: string | null;
  previsaoEntrega?: string | null;
  condicaoPagamento?: string | null;
  observacao?: string | null;
  itens: CriarPedidoCompraItem[];
}

@Injectable({ providedIn: 'root' })
export class PedidoCompraService extends CrudService<PedidoCompra> {
  protected readonly resource = 'pedidos-compra';
  enviarFornecedor(id: string, emailDestinoOverride?: string | null): Observable<unknown> {
    return this.http.post(
      `${environment.apiUrl}/${environment.apiVersion}/pedidos-compra/${id}/enviar-fornecedor`,
      { emailDestinoOverride: emailDestinoOverride ?? null },
    );
  }
  criarComItens(payload: CriarPedidoCompraPayload): Observable<{ id: string; numero: string }> {
    return this.http.post<{ id: string; numero: string }>(
      `${environment.apiUrl}/${environment.apiVersion}/pedidos-compra`, payload);
  }
  obterDetalhe(id: string): Observable<PedidoCompraDetalhe> {
    return this.http.get<PedidoCompraDetalhe>(
      `${environment.apiUrl}/${environment.apiVersion}/pedidos-compra/${id}`);
  }
}

@Injectable({ providedIn: 'root' })
export class RecebimentoCompraService extends CrudService<RecebimentoCompra> {
  protected readonly resource = 'recebimentos-compra';
  registrar(payload: RegistrarRecebimentoPayload): Observable<{ recebimentoId: string; numero: string }> {
    return this.http.post<{ recebimentoId: string; numero: string }>(
      `${environment.apiUrl}/${environment.apiVersion}/recebimentos-compra`, payload);
  }
}
