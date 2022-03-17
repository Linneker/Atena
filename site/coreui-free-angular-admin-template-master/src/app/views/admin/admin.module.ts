import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProductModule } from './product/product.module';
import { AdminRoutingModule } from './admin-routing.module';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    AdminRoutingModule
  ]
})
export class AdminModule { }
