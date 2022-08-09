import { Injectable } from '@angular/core';
import { HttpEvent, HttpInterceptor, HttpHandler, HttpRequest, HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable()
export class TokenInterceptorService implements HttpInterceptor {

  constructor() {
  }



  intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    const jwt = sessionStorage.getItem('jwt') || 'erro401';
    let tokenValido = (Date.parse(sessionStorage.getItem("expiration")) >= Date.now());
    if(jwt == 'erro401')
    {

    }
    else{
    req = req.clone({
      setHeaders: {
        'Authorization': `Bearer ${jwt}`
      }
    });
      return next.handle(req);
   }
}




}
