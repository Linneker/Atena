import { Injectable, computed, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { Observable, catchError, map, of, tap, timer } from 'rxjs';
import { environment } from '@env/environment';
import { AuthSession, JwtPayload, LoginRequest, LoginResponse } from './auth.types';

const STORAGE_KEY = 'atena.session';
const REFRESH_LEAD_MS = 60_000;

@Injectable({ providedIn: 'root' })
export class AuthStore {
  private readonly http = inject(HttpClient);
  private readonly router = inject(Router);

  private readonly sessionSig = signal<AuthSession | null>(null);
  private refreshHandle: ReturnType<typeof setTimeout> | null = null;

  readonly session = this.sessionSig.asReadonly();
  readonly isAuthenticated = computed(() => {
    const s = this.sessionSig();
    return !!s && s.expiresAt > Date.now();
  });
  readonly user = computed(() => this.sessionSig()?.user ?? null);
  readonly permissions = computed(() => this.sessionSig()?.user.permissions ?? []);
  readonly tenantId = computed(() => this.sessionSig()?.user.tenantId ?? null);

  bootstrap(): void {
    const raw = localStorage.getItem(STORAGE_KEY);
    if (!raw) return;
    try {
      const session = JSON.parse(raw) as AuthSession;
      if (session.expiresAt <= Date.now()) {
        this.clear();
        return;
      }
      this.sessionSig.set(session);
      this.scheduleRefresh(session);
    } catch {
      this.clear();
    }
  }

  login(req: LoginRequest): Observable<boolean> {
    return this.http
      .post<LoginResponse>(`${environment.apiUrl}/${environment.apiVersion}/autenticacao/login`, req)
      .pipe(
        map((res) => {
          this.persist(res);
          return true;
        }),
        catchError(() => of(false)),
      );
  }

  refresh(): Observable<boolean> {
    const current = this.sessionSig();
    if (!current) return of(false);
    return this.http
      .post<LoginResponse>(`${environment.apiUrl}/${environment.apiVersion}/autenticacao/renovar`, {
        refreshToken: current.refreshToken,
      })
      .pipe(
        tap((res) => this.persist(res)),
        map(() => true),
        catchError(() => {
          this.logout();
          return of(false);
        }),
      );
  }

  logout(): void {
    const current = this.sessionSig();
    if (current) {
      this.http
        .post(`${environment.apiUrl}/${environment.apiVersion}/autenticacao/logout`, {
          refreshToken: current.refreshToken,
        })
        .subscribe({ error: () => {} });
    }
    this.clear();
    this.router.navigate(['/login']);
  }

  hasPermission(permissao: string): boolean {
    return this.permissions().includes(permissao);
  }

  hasAnyPermission(permissoes: string[]): boolean {
    const granted = new Set(this.permissions());
    return permissoes.some((p) => granted.has(p));
  }

  accessToken(): string | null {
    return this.sessionSig()?.accessToken ?? null;
  }

  private persist(res: LoginResponse): void {
    const payload = decodeJwt(res.accessToken);
    if (!payload) {
      this.clear();
      return;
    }
    const session: AuthSession = {
      accessToken: res.accessToken,
      refreshToken: res.refreshToken,
      expiresAt: payload.exp * 1000,
      user: {
        id: payload.sub,
        email: payload.email,
        nome: payload.nome,
        tenantId: payload.tenant_id,
        permissions: payload.permissions ?? [],
      },
    };
    localStorage.setItem(STORAGE_KEY, JSON.stringify(session));
    this.sessionSig.set(session);
    this.scheduleRefresh(session);
  }

  private scheduleRefresh(session: AuthSession): void {
    if (this.refreshHandle) clearTimeout(this.refreshHandle);
    const delay = Math.max(session.expiresAt - Date.now() - REFRESH_LEAD_MS, 5_000);
    this.refreshHandle = setTimeout(() => {
      this.refresh().subscribe();
    }, delay);
  }

  private clear(): void {
    if (this.refreshHandle) {
      clearTimeout(this.refreshHandle);
      this.refreshHandle = null;
    }
    localStorage.removeItem(STORAGE_KEY);
    this.sessionSig.set(null);
  }
}

function decodeJwt(token: string): JwtPayload | null {
  try {
    const part = token.split('.')[1];
    const json = atob(part.replace(/-/g, '+').replace(/_/g, '/'));
    return JSON.parse(json) as JwtPayload;
  } catch {
    return null;
  }
}
