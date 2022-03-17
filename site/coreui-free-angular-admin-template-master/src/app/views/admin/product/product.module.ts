import { TipoProdutoComponent } from './tipo-produto/tipo-produto.component';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ProdutoComponent } from './produto/produto.component';
import { ProductRoutingModule } from './product-routing.module';
import { FormsModule } from '@angular/forms';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TipoProdutoModule } from './tipo-produto/tipo-produto.module';
import { TipoValorProdutoComponent } from './tipo-valor-produto/tipo-valor-produto.component';
import { CadastroTipoValorProdutoComponent } from './tipo-valor-produto/cadastro-tipo-valor-produto/cadastro-tipo-valor-produto.component';
import { TipoValorProdutoModule } from './tipo-valor-produto/tipo-valor-produto.module';
import { ProdutoModule } from './produto/produto.module';



@NgModule({
  declarations: [],
  imports: [
    FormsModule,
    CommonModule,
    ModalModule,
    NgbModule,
    ProdutoModule,
    ProductRoutingModule,
    TipoProdutoModule,
    TipoValorProdutoModule
  ]
})
export class ProductModule { }
