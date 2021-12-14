import { CookieConsertService } from '../cookie-consert/cookie-consert.service';
import { UsuarioRequest } from './../../view-model/request/usuario';
import { UsuarioResponse } from './../../view-model/response/usuario';
import { RequestDefaultService } from './../auth/request-default.service';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class UsuarioService extends  RequestDefaultService<UsuarioRequest,UsuarioResponse>{

  receitaUrl: string = "https://bardochiquinho.acmesistemas.com.br/api/Usuario";
  mostrarMenu : boolean = false;
  usuarioAutenticado: boolean = false;

  constructor(httpClient: HttpClient, private router : Router,private cookie: CookieConsertService ) {
    super(httpClient);
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
          this.cookie.setObjectCookie("usuario",this.usuarioAutenticado);
          this.router.navigate(['/dashboard']);
        }
        else{
        }
      }
    });
  }

  get UsuarioEstaAutenticado(){
    return this.usuarioAutenticado;
  }
}
