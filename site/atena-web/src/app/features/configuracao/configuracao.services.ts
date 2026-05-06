import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';
import { CrudService } from '@shared/crud/crud.service';

export interface Usuario { id?: string; nome: string; email: string; ativo: boolean; rolesIds?: string[]; }
export interface Role { id?: string; nome: string; descricao: string; permissoes?: string[]; }
export interface Permissao { recurso: string; acao: string; chave: string; descricao: string; }
export interface ParametroSistema { chave: string; valor: string; descricao: string; }
export interface BrandingTenant {
  razaoSocial: string; logoUrl?: string; corPrimaria: string;
  corSecundaria: string; corAccent: string; fusoHorario: string;
}

@Injectable({ providedIn: 'root' })
export class UsuarioService extends CrudService<Usuario> { protected readonly resource = 'usuarios'; }

@Injectable({ providedIn: 'root' })
export class RoleService extends CrudService<Role> { protected readonly resource = 'roles'; }

@Injectable({ providedIn: 'root' })
export class PermissaoService {
  private readonly http = inject(HttpClient);
  listar(): Observable<Permissao[]> {
    return this.http.get<Permissao[]>(`${environment.apiUrl}/${environment.apiVersion}/permissoes`);
  }
}

@Injectable({ providedIn: 'root' })
export class ParametroService {
  private readonly http = inject(HttpClient);
  listar(): Observable<ParametroSistema[]> {
    return this.http.get<ParametroSistema[]>(`${environment.apiUrl}/${environment.apiVersion}/parametros`);
  }
  salvar(p: ParametroSistema): Observable<unknown> {
    return this.http.put(`${environment.apiUrl}/${environment.apiVersion}/parametros/${p.chave}`, p);
  }
}

@Injectable({ providedIn: 'root' })
export class BrandingService {
  private readonly http = inject(HttpClient);
  obter(): Observable<BrandingTenant> {
    return this.http.get<BrandingTenant>(`${environment.apiUrl}/${environment.apiVersion}/tenants/me/branding`);
  }
  salvar(b: BrandingTenant): Observable<unknown> {
    return this.http.put(`${environment.apiUrl}/${environment.apiVersion}/tenants/me/branding`, b);
  }
}
