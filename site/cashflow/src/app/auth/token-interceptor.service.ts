import { AutorizacaoApiRequest } from './../view-model/request/autorizacao-api-request/autorizacao-api-request.module';
import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { AutorizacaoApiResponse } from '../view-model/response/autorizacao-api-response';

@Injectable()
export class TokenInterceptorService implements HttpInterceptor {

  constructor() {
  }



  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const jwt = sessionStorage.getItem('jwt') || 'teste';

    req = req.clone({
      setHeaders: {
        'Authorization': `Bearer ${jwt}`
      }
    });
    return next.handle(req);
  }




}
