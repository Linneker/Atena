import { CookieConsertService } from './../cookie-consert/cookie-consert.service';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { UsuarioService } from '../usuario/usuario.service';
import { AuthorizationService } from './authorization.service';

@Injectable()
export class AuthGuard implements CanActivate{

  constructor(private usuarioService: UsuarioService, private router : Router,
    private cookie: CookieConsertService,
    private authorizationService: AuthorizationService){

  }

  canActivate(
    route : ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):Observable<boolean> | boolean{
    var valor  = JSON.parse(this.cookie.getCookie("usuario"));
    let tokenValido = (Date.parse(sessionStorage.getItem("expiration")) >= Date.now());
    debugger;
    if(valor !== null && valor !== undefined && tokenValido){
      return true;
    }
    this.router.navigate(['/login']);
    return false;

  }
}
