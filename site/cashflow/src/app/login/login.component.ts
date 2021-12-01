import { UsuarioRequest } from './../view-model/request/usuario';
import { Component, OnInit } from '@angular/core';
import { UsuarioService } from '../usuario/usuario.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  login : UsuarioRequest = new UsuarioRequest();
  constructor(private usuarioService : UsuarioService,private router: Router) { }
  mostrarMenu: boolean = false;

  ngOnInit(): void {
    this.login = new UsuarioRequest();
  }

  Login():void
  {
    debugger;
    console.log(this.login);
    if(this.login.Login ==''){
      alert("ERRO PREENCHA O LOGIN");
      return;;
    }
    this.usuarioService.Login(this.login.Login,this.login.Senha)
  }
  Registrar():void{

  }
  EsqueceuSenha():void{

  }
}
