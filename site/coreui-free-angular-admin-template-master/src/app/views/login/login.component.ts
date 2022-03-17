import { AuthorizationService } from "./../auth/authorization.service";
import { Component } from "@angular/core";
import { Router } from "@angular/router";
import { UsuarioRequest } from "../../view-model/request/usuario";
import { UsuarioService } from "../usuario/usuario.service";

@Component({
  selector: "app-dashboard",
  templateUrl: "login.component.html",
})
export class LoginComponent {
  login: UsuarioRequest = new UsuarioRequest();
  mostrarMenu: boolean = false;
  nomeLogin: string = "Login";
  constructor(
    private usuarioService: UsuarioService,
    private router: Router,
    private authorizationService: AuthorizationService
  ) {}

  ngOnInit() {

  }

  Login(): void {
    debugger;
    console.log(this.login);
    console.log(this.login);
    if (this.login.Login == "") {
      alert("ERRO PREENCHA O LOGIN");
      return;
    }
    this.nomeLogin = "Carregando...";
    this.usuarioService.Login(this.login.Login, this.login.Senha);
  }
}
