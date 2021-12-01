import { UsuarioResponse } from './../../view-model/response/usuario';
import { AuthorizationService } from './../auth/authorization.service';
import { UsuarioService } from './usuario.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-usuario',
  templateUrl: './usuario.component.html',
  styleUrls: ['./usuario.component.css']
})
export class UsuarioComponent implements OnInit {

  tableUsuarios : UsuarioResponse[] = [];

  constructor(private usuarioService: UsuarioService, private authorizationService: AuthorizationService) {
    authorizationService.setCompetencias();
  }

  ngOnInit(): void {
    debugger;
    this.usuarioService.getAll("Usuario").subscribe({
      next: (t: any) => {
        this.tableUsuarios = t.$values;
        console.log(this.tableUsuarios);
        console.log(t);
      }
    });
  }

}
