import { EnderecoService } from './../util/endereco.service';
import { CadastroUsuarioComponent } from './cadastro-usuario/cadastro-usuario.component';
import { UsuarioResponse } from '../../view-model/response/security/usuario';
import { AuthorizationService } from './../auth/authorization.service';
import { UsuarioService } from './usuario.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { CookieConsertService } from '../cookie-consert/cookie-consert.service';
import { BsModalRef, BsModalService, ModalDirective } from 'ngx-bootstrap/modal';
import { ModalComponent } from '../shared/modal/modal.component';

@Component({
  selector: 'app-usuario',
  templateUrl: './usuario.component.html',
  styleUrls: ['./usuario.component.css']
})
export class UsuarioComponent implements OnInit {

  @ViewChild('largeModal') public largeModal: ModalDirective;

  tableUsuarios : UsuarioResponse[] = [];
  bsModalRef: BsModalRef = new BsModalRef();

  constructor(private usuarioService: UsuarioService, private authorizationService: AuthorizationService,
    private cookie: CookieConsertService ,
    private modalService: BsModalService) {
    authorizationService.setCompetencias();
  }

  ngOnInit(): void {
    var valor  = JSON.parse(this.cookie.getCookie("usuario"));

    this.usuarioService.getAll("Usuario").subscribe({
      next: (t: any) => {
        this.tableUsuarios = t;
      }
    });
  }
  openCadastro(){
    //this.modalService.show(CadastroUsuarioComponent);
    this.largeModal.show();
  }
  openModal(){
    this.bsModalRef = this.modalService.show(ModalComponent);
    this.bsModalRef.content.titulo= "Titulo";
    this.bsModalRef.content.campos= "<app-cadastro-usuario></app-cadastro-usuario>";
    this.bsModalRef.content.botaoSalvar = "<button type='button' class='btn btn-primary'>Save changes</button>";
  }
}
