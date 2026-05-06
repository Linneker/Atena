import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { AuthStore } from '@core/auth/auth.store';

export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthStore);
  const token = auth.accessToken();
  const tenantId = auth.tenantId();
  if (!token) return next(req);

  const headers: Record<string, string> = { Authorization: `Bearer ${token}` };
  if (tenantId) headers['X-Tenant-Id'] = tenantId;

  return next(req.clone({ setHeaders: headers }));
};
