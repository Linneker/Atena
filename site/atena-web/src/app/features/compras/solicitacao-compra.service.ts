import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';

export interface CriarSolicitacaoItem {
  produtoId: string;
  quantidade: number;
  precoEstimado?: number | null;
  observacao?: string | null;
}

export interface CriarSolicitacaoPayload {
  justificativa?: string | null;
  itens: CriarSolicitacaoItem[];
  enviarParaAprovacao?: boolean;
}

export interface CriarSolicitacaoResponse {
  id: string;
  numero: string;
}

@Injectable({ providedIn: 'root' })
export class SolicitacaoCompraApiService {
  private readonly http = inject(HttpClient);
  private readonly url = `${environment.apiUrl}/${environment.apiVersion}/solicitacoes-compra`;

  criar(payload: CriarSolicitacaoPayload): Observable<CriarSolicitacaoResponse> {
    return this.http.post<CriarSolicitacaoResponse>(this.url, payload);
  }
}
