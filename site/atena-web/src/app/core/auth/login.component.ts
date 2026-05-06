import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AuthStore } from './auth.store';
import { TenantBrandingService } from '@core/branding/tenant-branding.service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [ReactiveFormsModule, RouterLink],
  template: `
    <div class="login-container d-flex align-items-center justify-content-center vh-100">
      <form class="card p-4 shadow-sm" style="min-width:340px" [formGroup]="form" (ngSubmit)="submit()">
        <h3 class="mb-3 text-center">Atena ERP</h3>
        <div class="mb-3">
          <label class="form-label">E-mail</label>
          <input type="email" class="form-control" formControlName="email" autocomplete="username" />
        </div>
        <div class="mb-3">
          <label class="form-label">Senha</label>
          <input type="password" class="form-control" formControlName="senha" autocomplete="current-password" />
        </div>
        @if (errorMsg()) {
          <div class="alert alert-danger py-2">{{ errorMsg() }}</div>
        }
        <button type="submit" class="btn btn-primary w-100" [disabled]="form.invalid || loading()">
          {{ loading() ? 'Entrando...' : 'Entrar' }}
        </button>
      </form>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class LoginComponent {
  private readonly fb = inject(FormBuilder);
  private readonly auth = inject(AuthStore);
  private readonly branding = inject(TenantBrandingService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  readonly loading = signal(false);
  readonly errorMsg = signal<string | null>(null);

  readonly form = this.fb.nonNullable.group({
    email: ['', [Validators.required, Validators.email]],
    senha: ['', [Validators.required, Validators.minLength(6)]],
  });

  submit(): void {
    if (this.form.invalid) return;
    this.loading.set(true);
    this.errorMsg.set(null);
    this.auth.login(this.form.getRawValue()).subscribe((ok) => {
      this.loading.set(false);
      if (!ok) {
        this.errorMsg.set('Credenciais inválidas');
        return;
      }
      this.branding.load();
      const returnUrl = this.route.snapshot.queryParamMap.get('returnUrl') ?? '/dashboard';
      this.router.navigateByUrl(returnUrl);
    });
  }
}
