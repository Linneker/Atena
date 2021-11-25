import { LoginComponent } from './../login/login.component';
import { UsuarioService } from './usuario.service';
import { UsuarioComponent } from './usuario.component';
import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';



@NgModule({
  declarations: [
    UsuarioComponent,
    LoginComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    RouterModule.forChild([
    ])
  ],
  providers:[
    UsuarioService
  ]
})
export class UsuarioModule { }
