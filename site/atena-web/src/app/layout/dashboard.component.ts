import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { AuthStore } from '@core/auth/auth.store';

@Component({
  selector: 'app-dashboard',
  standalone: true,
  template: `
    <h2 class="mb-3">Bem-vindo, {{ auth.user()?.nome }}</h2>
    <p class="text-muted">Acesse os módulos pelo menu lateral.</p>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DashboardComponent {
  readonly auth = inject(AuthStore);
}
