import { ResponseApi } from './../../view-model/response/util/response-api';
import { Observable } from 'rxjs';
import { AuthorizationService } from './../auth/authorization.service';
import { CookieConsertService } from '../cookie-consert/cookie-consert.service';
import { UsuarioRequest } from './../../view-model/request/usuario';
import { UsuarioResponse } from '../../view-model/response/security/usuario';
import { RequestDefaultService } from './../auth/request-default.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class UsuarioService extends  RequestDefaultService<UsuarioRequest,UsuarioResponse>{

  receitaUrl: string = environment.url+ "Usuario";
  mostrarMenu : boolean = false;
  usuarioAutenticado: boolean = false;

  constructor(httpClient: HttpClient, private router : Router,private cookie: CookieConsertService,
    private authorizationService: AuthorizationService
    ) {
    super(httpClient);
  }
  AddAsync (objeto: UsuarioResponse, servico : string) : Observable<any>
  {
    return this.httpClient.post<any>(`${this.receitaUrlBase}${servico}`,objeto);
  }
  Login(login: string, senha: string): void
  {
    let usrs : UsuarioRequest = new UsuarioRequest();
    usrs.Login = login;
    usrs.Senha= senha;
    debugger;
     this.httpClient.post<UsuarioResponse>(`${this.receitaUrl}/Login`,usrs)
     .subscribe({
      next: (usuario: UsuarioResponse)=>{
        if(usuario != null){
          this.usuarioAutenticado = true;
          this.cookie.setCookie("usuario",JSON.stringify(usuario));
          this.router.navigate(['/dashboard']);
          let tokenValido = (Date.parse(sessionStorage.getItem("expiration")) >= Date.now());
          if (sessionStorage.getItem("jwt") == null || !tokenValido) {
            this.authorizationService.setCompetencias();
          }
        }
        else{
          alert("Login ou senha invalidos");
        }
      },
      error: (e) =>{
        alert(e.error);
        console.log(e);
        console.log(e.error);
      }
    });
  }

  get UsuarioEstaAutenticado() {
    return this.usuarioAutenticado;
  }
}
