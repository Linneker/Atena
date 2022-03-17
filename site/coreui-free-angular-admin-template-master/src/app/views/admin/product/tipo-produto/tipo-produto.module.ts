import { TipoProdutoComponent } from './tipo-produto.component';
import { CommonModule } from "@angular/common";
import { NgModule } from "@angular/core";
import { FormsModule } from "@angular/forms";
import { NgbModule } from "@ng-bootstrap/ng-bootstrap";
import { ModalModule } from "ngx-bootstrap/modal";
import { CadastroTipoProdutoComponent } from './cadastro-tipo-produto/cadastro-tipo-produto.component';
import { AtualizaTipoProdutoComponent } from './atualiza-tipo-produto/atualiza-tipo-produto.component';
import { RemoveTipoProdutoComponent } from './remove-tipo-produto/remove-tipo-produto.component';


@NgModule({
  declarations: [
    TipoProdutoComponent,
    CadastroTipoProdutoComponent,
    AtualizaTipoProdutoComponent,
    RemoveTipoProdutoComponent
  ],
  imports: [
    FormsModule,
    CommonModule,
    ModalModule,
    NgbModule
  ]
})
export class TipoProdutoModule { }
