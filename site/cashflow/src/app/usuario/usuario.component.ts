import { AuthorizationService } from './../auth/authorization.service';
import { UsuarioResponse } from './../view-model/response/usuario';
import { UsuarioService } from './usuario.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-usuario',
  templateUrl: './usuario.component.html',
  styleUrls: ['./usuario.component.css']
})
export class UsuarioComponent implements OnInit {


  constructor(private usuarioService : UsuarioService, private authorizationService : AuthorizationService ) {
      authorizationService.setCompetencias();
   }

  ngOnInit(): void {
    debugger;
    this.usuarioService.getAll("Usuario").subscribe({
      next:(t: UsuarioResponse[])=> {
        console.log(t);
      }
    });
  }

}
