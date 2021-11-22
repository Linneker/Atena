import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { UsuarioService } from '../usuario/usuario.service';

@Injectable()
export class AuthGuard implements CanActivate{

  constructor(private usuarioService: UsuarioService, private router : Router){

  }

  canActivate(
    route : ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ):Observable<boolean> | boolean{

    if(this.usuarioService.UsuarioEstaAutenticado){
      return true;
    }
    this.router.navigate(['/login']);
    return false;

  }
}
