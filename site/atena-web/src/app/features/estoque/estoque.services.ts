import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';
import { CrudService } from '@shared/crud/crud.service';
import { PaginaResultado } from '@shared/data-table/data-table.component';

export interface SaldoEstoque { produtoId: string; codigo: string; descricao: string; saldoTotal: number; saldoReservado: number; saldoDisponivel: number; }
export interface MovimentacaoEstoque { id?: string; produto: string; tipo: 'ENTRADA' | 'SAIDA'; quantidade: number; data: string; motivo: string; }
export interface Inventario { id?: string; descricao: string; status: string; abertoEm: string; fechadoEm?: string; }

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
