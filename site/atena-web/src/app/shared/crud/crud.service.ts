import { HttpClient, HttpParams } from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';
import { OrdenacaoTabela, PaginaResultado } from '../data-table/data-table.component';

export interface ListarOpcoes {
  pagina?: number;
  tamanhoPagina?: number;
  busca?: string;
  ordenacao?: OrdenacaoTabela | null;
}

export abstract class CrudService<T extends { id?: string }> {
  protected readonly http = inject(HttpClient);
  protected abstract readonly resource: string;

  private url(): string {
    return `${environment.apiUrl}/${environment.apiVersion}/${this.resource}`;
  }

  listar(opcoes: ListarOpcoes = {}): Observable<PaginaResultado<T>>;
  listar(pagina: number, tamanhoPagina?: number, busca?: string): Observable<PaginaResultado<T>>;
  listar(arg1?: number | ListarOpcoes, tamanhoPagina = 20, busca = ''): Observable<PaginaResultado<T>> {
    const opcoes: ListarOpcoes =
      typeof arg1 === 'object' || arg1 === undefined
        ? (arg1 ?? {})
        : { pagina: arg1, tamanhoPagina, busca };

    let params = new HttpParams()
      .set('pagina', opcoes.pagina ?? 1)
      .set('tamanhoPagina', opcoes.tamanhoPagina ?? 20);
    if (opcoes.busca) params = params.set('busca', opcoes.busca);
    if (opcoes.ordenacao) {
      params = params.set('ordenarPor', opcoes.ordenacao.campo).set('direcao', opcoes.ordenacao.direcao);
    }
    return this.http.get<PaginaResultado<T>>(this.url(), { params });
  }

  obter(id: string): Observable<T> {
    return this.http.get<T>(`${this.url()}/${id}`);
  }

  criar(payload: Partial<T>): Observable<T> {
    return this.http.post<T>(this.url(), payload);
  }

  alterar(id: string, payload: Partial<T>): Observable<T> {
    return this.http.put<T>(`${this.url()}/${id}`, payload);
  }

  excluir(id: string): Observable<void> {
    return this.http.delete<void>(`${this.url()}/${id}`);
  }
}
