import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ModalModule } from 'ngx-bootstrap/modal';
import { FormsModule } from '@angular/forms';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TipoValorProdutoComponent } from './tipo-valor-produto.component';
import { CadastroTipoValorProdutoComponent } from './cadastro-tipo-valor-produto/cadastro-tipo-valor-produto.component';
import { TipoValorProdutoService } from './tipo-valor-produto.service';



@NgModule({
  declarations: [
    TipoValorProdutoComponent,
    CadastroTipoValorProdutoComponent
  ],
  imports: [
    FormsModule,
    CommonModule,
    ModalModule,
    NgbModule
  ],
  providers:[
    TipoValorProdutoService
  ]
})
export class TipoValorProdutoModule { }
