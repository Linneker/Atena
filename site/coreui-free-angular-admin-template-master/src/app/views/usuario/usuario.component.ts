import { UsuarioResponse } from './../../view-model/response/usuario';
import { AuthorizationService } from './../auth/authorization.service';
import { UsuarioService } from './usuario.service';
import { Component, OnInit } from '@angular/core';
import { CookieConsertService } from '../cookie-consert/cookie-consert.service';

@Component({
  selector: 'app-usuario',
  templateUrl: './usuario.component.html',
  styleUrls: ['./usuario.component.css']
})
export class UsuarioComponent implements OnInit {

  tableUsuarios : UsuarioResponse[] = [];

  constructor(private usuarioService: UsuarioService, private authorizationService: AuthorizationService,
    private cookie: CookieConsertService ) {
    authorizationService.setCompetencias();
  }

  ngOnInit(): void {
    debugger;
    var valor = this.cookie.getCookie("usuario");
    this.usuarioService.getAll("Usuario").subscribe({
      next: (t: any) => {
       debugger;
        this.tableUsuarios = t;
        console.log(this.tableUsuarios);
        console.log(t);
      }
    });
  }

}
