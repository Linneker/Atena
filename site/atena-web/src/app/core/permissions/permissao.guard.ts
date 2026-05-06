import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { AuthStore } from '@core/auth/auth.store';

export const permissaoGuard: CanActivateFn = (route) => {
  const auth = inject(AuthStore);
  const router = inject(Router);

  const required = route.data['permissao'] as string | string[] | undefined;
  if (!required) return true;

  const list = Array.isArray(required) ? required : [required];
  if (auth.hasAnyPermission(list)) return true;

  return router.createUrlTree(['/dashboard'], { queryParams: { sem_permissao: list.join(',') } });
};
