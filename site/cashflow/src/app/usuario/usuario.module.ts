import { UsuarioService } from './usuario.service';
import { UsuarioComponent } from './usuario.component';
import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';



@NgModule({
  declarations: [
    UsuarioComponent
  ],
  imports: [
    CommonModule,
    RouterModule.forChild([
    ])
  ],
  providers:[
    UsuarioService
  ]
})
export class UsuarioModule { }
