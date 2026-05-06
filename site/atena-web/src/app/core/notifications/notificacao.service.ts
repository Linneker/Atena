import { DestroyRef, Injectable, computed, inject, signal } from '@angular/core';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { HttpClient } from '@angular/common/http';
import { Subscription, interval, switchMap } from 'rxjs';
import { environment } from '@env/environment';
import { AuthStore } from '@core/auth/auth.store';

export interface Notificacao {
  id: string;
  tipo: 'APROVACAO_PENDENTE' | 'VENCIMENTO' | 'ESTOQUE_MINIMO' | 'INFO';
  titulo: string;
  mensagem: string;
  link?: string;
  lida: boolean;
  criadaEm: string;
}

const POLL_INTERVAL_MS = 30_000;

@Injectable({ providedIn: 'root' })
export class NotificacaoService {
  private readonly http = inject(HttpClient);
  private readonly auth = inject(AuthStore);
  private readonly destroyRef = inject(DestroyRef);

  private readonly notificacoesSig = signal<Notificacao[]>([]);
  readonly notificacoes = this.notificacoesSig.asReadonly();
  readonly naoLidas = computed(() => this.notificacoesSig().filter((n) => !n.lida).length);

  private subscription: Subscription | null = null;

  iniciarPolling(): void {
    if (this.subscription) return;
    this.subscription = interval(POLL_INTERVAL_MS)
      .pipe(
        switchMap(() => this.http.get<Notificacao[]>(`${environment.apiUrl}/${environment.apiVersion}/notificacoes`)),
        takeUntilDestroyed(this.destroyRef),
      )
      .subscribe({
        next: (lista) => this.notificacoesSig.set(lista),
        error: () => {},
      });
    this.recarregar();
  }

  pararPolling(): void {
    this.subscription?.unsubscribe();
    this.subscription = null;
    this.notificacoesSig.set([]);
  }

  recarregar(): void {
    if (!this.auth.isAuthenticated()) return;
    this.http
      .get<Notificacao[]>(`${environment.apiUrl}/${environment.apiVersion}/notificacoes`)
      .subscribe({ next: (lista) => this.notificacoesSig.set(lista), error: () => {} });
  }

  marcarLida(id: string): void {
    this.http
      .post(`${environment.apiUrl}/${environment.apiVersion}/notificacoes/${id}/ler`, {})
      .subscribe(() => {
        this.notificacoesSig.update((lista) => lista.map((n) => (n.id === id ? { ...n, lida: true } : n)));
      });
  }
}
