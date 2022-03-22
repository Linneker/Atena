import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProdutoComponent } from './produto.component';
import { FormsModule } from '@angular/forms';
import { ModalModule } from 'ngx-bootstrap/modal';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { CadastroProdutoComponent } from './cadastro-produto/cadastro-produto.component';
import { RemoveProdutoComponent } from './remove-produto/remove-produto.component';

@NgModule({
  imports: [
    CommonModule,
    FormsModule,
    ModalModule,
    NgbModule
  ],
  declarations: [
    ProdutoComponent,
    CadastroProdutoComponent,
    RemoveProdutoComponent
  ]
})
export class ProdutoModule { }
