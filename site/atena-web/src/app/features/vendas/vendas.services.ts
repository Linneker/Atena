import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';
import { CrudService } from '@shared/crud/crud.service';

export interface Orcamento { id?: string; numero: string; cliente: string; validade: string; total: number; status: string; }
export interface PedidoVenda { id?: string; numero: string; cliente: string; vendedor: string; total: number; status: string; }
export interface Faturamento { id?: string; pedidoNumero: string; numeroNota: string; valor: number; status: string; data: string; }
export interface DevolucaoVenda { id?: string; faturamentoId: string; motivo: string; valor: number; data: string; status: string; }

@Injectable({ providedIn: 'root' })
export class OrcamentoService extends CrudService<Orcamento> {
  protected readonly resource = 'orcamentos';
  converterEmPedido(id: string): Observable<unknown> {
    return this.http.post(`${environment.apiUrl}/${environment.apiVersion}/orcamentos/${id}/converter`, {});
  }
}

@Injectable({ providedIn: 'root' })
export class PedidoVendaService extends CrudService<PedidoVenda> {
  protected readonly resource = 'pedidos-venda';
  confirmar(id: string): Observable<unknown> {
    return this.http.post(`${environment.apiUrl}/${environment.apiVersion}/pedidos-venda/${id}/confirmar`, {});
  }
}

@Injectable({ providedIn: 'root' })
export class FaturamentoService extends CrudService<Faturamento> {
  protected readonly resource = 'faturamentos';
}

@Injectable({ providedIn: 'root' })
export class DevolucaoVendaService extends CrudService<DevolucaoVenda> {
  protected readonly resource = 'devolucoes-venda';
}
