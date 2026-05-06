import { Injectable } from '@angular/core';
import { CrudService } from '@shared/crud/crud.service';

export interface SolicitacaoCompra { id?: string; numero: string; solicitante: string; status: string; total: number; criadaEm: string; }
export interface PedidoCompra { id?: string; numero: string; fornecedor: string; status: string; total: number; emitidoEm: string; }
export interface RecebimentoCompra { id?: string; pedidoNumero: string; data: string; tipo: string; status: string; }

@Injectable({ providedIn: 'root' })
export class SolicitacaoCompraService extends CrudService<SolicitacaoCompra> {
  protected readonly resource = 'solicitacoes-compra';
}

@Injectable({ providedIn: 'root' })
export class PedidoCompraService extends CrudService<PedidoCompra> {
  protected readonly resource = 'pedidos-compra';
}

@Injectable({ providedIn: 'root' })
export class RecebimentoCompraService extends CrudService<RecebimentoCompra> {
  protected readonly resource = 'recebimentos-compra';
}
