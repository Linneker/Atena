import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class TokenInterceptorService implements HttpInterceptor {

  constructor() {
  }



  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    debugger;
    const jwt = sessionStorage.getItem('jwt') || 'teste';
    let tokenValido = (Date.parse(sessionStorage.getItem("expiration")) >= Date.now());


    req = req.clone({
      setHeaders: {
        'Authorization': `Bearer ${jwt}`
      }
    });
    return next.handle(req);
  }




}
