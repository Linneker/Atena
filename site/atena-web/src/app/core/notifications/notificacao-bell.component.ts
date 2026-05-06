import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { Notificacao, NotificacaoService } from './notificacao.service';

@Component({
  selector: 'app-notificacao-bell',
  standalone: true,
  template: `
    <div class="position-relative d-inline-block">
      <button class="btn btn-sm btn-outline-secondary" (click)="aberto.set(!aberto())">
        🔔
        @if (servico.naoLidas() > 0) {
          <span class="badge bg-danger position-absolute top-0 start-100 translate-middle">{{ servico.naoLidas() }}</span>
        }
      </button>
      @if (aberto()) {
        <div class="card position-absolute end-0 mt-1 shadow" style="width: 320px; z-index: 1050">
          <div class="card-body p-2" style="max-height: 400px; overflow-y: auto">
            @for (n of servico.notificacoes(); track n.id) {
              <div class="border-bottom p-2" [class.bg-light]="!n.lida"
                   style="cursor:pointer" (click)="abrir(n)">
                <strong>{{ n.titulo }}</strong>
                <p class="m-0 small">{{ n.mensagem }}</p>
                <small class="text-muted">{{ n.criadaEm }}</small>
              </div>
            } @empty {
              <p class="m-0 text-center text-muted py-3">Sem notificações</p>
            }
          </div>
        </div>
      }
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NotificacaoBellComponent {
  readonly servico = inject(NotificacaoService);
  private readonly router = inject(Router);
  readonly aberto = signal(false);

  abrir(n: Notificacao): void {
    this.servico.marcarLida(n.id);
    if (n.link) this.router.navigateByUrl(n.link);
    this.aberto.set(false);
  }
}
