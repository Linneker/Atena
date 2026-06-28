import { HttpClient, HttpParams } from '@angular/common/http';
import { inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from '@env/environment';
import { OrdenacaoTabela, PaginaResultado } from '../data-table/data-table.component';

export interface ListarOpcoes {
  pagina?: number;
  tamanhoPagina?: number;
  busca?: string;
  ordenacao?: OrdenacaoTabela | null;
  /** Filtros adicionais aplicados como query string (ex: status, vencimentoInicio, vencimentoFim). */
  filtros?: Record<string, string | number | boolean | null | undefined>;
}

// Contrato real do backend Acme: query por skip/take/termo, response { items, total }.
interface BackendListarResponse<T> {
  items: T[];
  total: number;
}

export abstract class CrudService<T extends { id?: string }> {
  protected readonly http = inject(HttpClient);
  protected abstract readonly resource: string;

  private url(): string {
    return `${environment.apiUrl}/${environment.apiVersion}/${this.resource}`;
  }

  listar(opcoes?: ListarOpcoes): Observable<PaginaResultado<T>>;
  listar(pagina: number, tamanhoPagina?: number, busca?: string): Observable<PaginaResultado<T>>;
  listar(arg1?: number | ListarOpcoes, tamanhoPagina = 20, busca = ''): Observable<PaginaResultado<T>> {
    const opcoes: ListarOpcoes =
      typeof arg1 === 'object' || arg1 === undefined
        ? (arg1 ?? {})
        : { pagina: arg1, tamanhoPagina, busca };

    const numeroPagina = opcoes.pagina ?? 1;
    const tamanho = opcoes.tamanhoPagina ?? 20;
    const skip = (numeroPagina - 1) * tamanho;

    let params = new HttpParams()
      .set('skip', skip)
      .set('take', tamanho);
    if (opcoes.busca) params = params.set('termo', opcoes.busca);
    if (opcoes.ordenacao) {
      params = params.set('ordenarPor', opcoes.ordenacao.campo).set('direcao', opcoes.ordenacao.direcao);
    }
    if (opcoes.filtros) {
      for (const [k, v] of Object.entries(opcoes.filtros)) {
        if (v !== undefined && v !== null && v !== '') params = params.set(k, String(v));
      }
    }

    return this.http.get<BackendListarResponse<T>>(this.url(), { params }).pipe(
      map((res) => ({
        itens: res.items ?? [],
        total: res.total ?? 0,
        pagina: numeroPagina,
        tamanhoPagina: tamanho,
      })),
    );
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
