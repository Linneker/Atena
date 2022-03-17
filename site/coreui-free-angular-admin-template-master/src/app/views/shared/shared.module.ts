import { PipeModule } from './../pipe/pipe.module';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ModalComponent } from './modal/modal.component';
import { ModalModule } from 'ngx-bootstrap/modal';



@NgModule({
  declarations: [
    ModalComponent
  ],
  imports: [
    CommonModule,
    PipeModule,
    ModalModule
  ],
  exports: [
    ModalComponent
  ]
})
export class SharedModule { }
