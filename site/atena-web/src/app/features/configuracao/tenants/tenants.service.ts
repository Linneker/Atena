import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable, map } from 'rxjs';
import { environment } from '@env/environment';

export interface Tenant {
  id: string;
  razaoSocial: string;
  cnpj: string;
  plano: string;
  status: number;
  logoUrl?: string | null;
  corPrimaria?: string | null;
  fusoHorario: string;
  createdAt: string;
}

export interface TenantListagem {
  itens: Tenant[];
  total: number;
}

// Body do POST /api/v1/tenants/registrar — inclui admin embutido.
export interface RegistrarTenantPayload {
  razaoSocial: string;
  cnpj: string;
  plano: string;
  fusoHorario?: string | null;
  corPrimaria?: string | null;
  logoUrl?: string | null;
  adminNomeCompleto: string;
  adminEmail: string;
  adminSenha: string;
}

// Body do PUT /api/v1/tenants/{id}
export interface AlterarTenantPayload {
  razaoSocial: string;
  plano: string;
  status: number;
  logoUrl?: string | null;
  corPrimaria?: string | null;
  fusoHorario: string;
}

@Injectable({ providedIn: 'root' })
export class TenantsService {
  private readonly http = inject(HttpClient);
  private readonly base = `${environment.apiUrl}/${environment.apiVersion}/tenants`;

  listar(termo = '', skip = 0, take = 20): Observable<TenantListagem> {
    let params = new HttpParams().set('skip', skip).set('take', take);
    if (termo) params = params.set('termo', termo);
    return this.http
      .get<{ items: Tenant[]; total: number }>(this.base, { params })
      .pipe(map((r) => ({ itens: r.items ?? [], total: r.total ?? 0 })));
  }

  obter(id: string): Observable<Tenant> {
    return this.http.get<Tenant>(`${this.base}/${id}`);
  }

  registrar(payload: RegistrarTenantPayload): Observable<Tenant> {
    return this.http.post<Tenant>(`${this.base}/registrar`, payload);
  }

  alterar(id: string, payload: AlterarTenantPayload): Observable<Tenant> {
    return this.http.put<Tenant>(`${this.base}/${id}`, payload);
  }

  excluir(id: string): Observable<void> {
    return this.http.delete<void>(`${this.base}/${id}`);
  }
}
