import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { catchError, throwError } from 'rxjs';
import { AuthStore } from '@core/auth/auth.store';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthStore);
  return next(req).pipe(
    catchError((err: HttpErrorResponse) => {
      if (err.status === 401) {
        auth.logout();
      }
      return throwError(() => err);
    }),
  );
};
