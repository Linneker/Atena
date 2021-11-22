import { UsuarioResponse } from './../view-model/response/usuario';
import { RequestDefaultService } from './../auth/request-default.service';
import { UsuarioRequest } from './../view-model/request/usuario';
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

  constructor(protected httpClient: HttpClient, private router : Router) {
    super( httpClient);
  }

  Login(login: string, senha: string): void
  {
    let usrs : UsuarioRequest = new UsuarioRequest();
    usrs.Login = login;
    usrs.Senha= senha;

     this.httpClient.post<UsuarioResponse>(`${this.receitaUrl}/Login`,usrs)
     .subscribe({
      next: (usuario: UsuarioResponse)=>{
        if(usuario != null){
          this.usuarioAutenticado = true;
          this.router.navigate(['/home']);
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
