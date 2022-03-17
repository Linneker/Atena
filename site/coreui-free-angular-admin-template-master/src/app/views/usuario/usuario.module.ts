import { PipeModule } from './../pipe/pipe.module';
import { LoginComponent } from './../login/login.component';
import { UsuarioService } from './usuario.service';
import { UsuarioComponent } from './usuario.component';
import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { CadastroUsuarioComponent } from './cadastro-usuario/cadastro-usuario.component';
import { ModalComponent } from '../shared/modal/modal.component';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { BsModalService, ModalModule } from 'ngx-bootstrap/modal';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';



@NgModule({
  declarations: [
    UsuarioComponent,
    LoginComponent,
    CadastroUsuarioComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forChild([
    ]),
    ModalModule,
    NgbModule
  ],
  exports:[],
  providers:[
    UsuarioService
  ]
})
export class UsuarioModule { }
