import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';
import { CrudService } from '@shared/crud/crud.service';
import { PaginaResultado } from '@shared/data-table/data-table.component';

export interface SaldoEstoque { produtoId: string; codigo: string; descricao: string; saldoTotal: number; saldoReservado: number; saldoDisponivel: number; }
export interface MovimentacaoEstoque { id?: string; produto: string; tipo: 'ENTRADA' | 'SAIDA'; quantidade: number; data: string; motivo: string; }
export interface Inventario { id?: string; descricao: string; status: string; abertoEm: string; fechadoEm?: string; }

export interface EstoqueResumo {
  id: string;
  codigo: string;
  nome: string;
  localizacao?: string | null;
  ativo: boolean;
}

export interface SaldoPorEstoque {
  estoqueId: string;
  saldoTotal: number;
  saldoReservado: number;
  saldoDisponivel: number;
}

export interface ConsultarSaldoResposta {
  produtoId: string;
  totalGeral: number;
  reservadoGeral: number;
  disponivelGeral: number;
  porEstoque: SaldoPorEstoque[];
}

@Injectable({ providedIn: 'root' })
export class EstoquesService {
  private readonly http = inject(HttpClient);
  private readonly base = `${environment.apiUrl}/${environment.apiVersion}`;

  listar(): Observable<{ items: EstoqueResumo[]; total: number }> {
    return this.http.get<{ items: EstoqueResumo[]; total: number }>(`${this.base}/estoques`);
  }

  consultarSaldoProduto(produtoId: string, estoqueId?: string): Observable<ConsultarSaldoResposta> {
    let params = new HttpParams();
    if (estoqueId) params = params.set('estoqueId', estoqueId);
    return this.http.get<ConsultarSaldoResposta>(
      `${this.base}/estoque/produtos/${produtoId}/saldo`, { params });
  }
}

@Injectable({ providedIn: 'root' })
export class SaldoEstoqueService {
  private readonly http = inject(HttpClient);
  consultar(busca = '', pagina = 1): Observable<PaginaResultado<SaldoEstoque>> {
    const params = new HttpParams().set('pagina', pagina).set('tamanhoPagina', 20).set('busca', busca);
    return this.http.get<PaginaResultado<SaldoEstoque>>(`${environment.apiUrl}/${environment.apiVersion}/estoque/saldo`, { params });
  }
}

@Injectable({ providedIn: 'root' })
export class MovimentacaoEstoqueService extends CrudService<MovimentacaoEstoque> {
  protected readonly resource = 'estoque/movimentacao';
}

@Injectable({ providedIn: 'root' })
export class InventarioService extends CrudService<Inventario> {
  protected readonly resource = 'inventarios';

  abrir(payload: Partial<Inventario>) { return this.criar(payload); }
  fechar(id: string) { return this.http.post(`${environment.apiUrl}/${environment.apiVersion}/inventarios/${id}/fechar`, {}); }
}
