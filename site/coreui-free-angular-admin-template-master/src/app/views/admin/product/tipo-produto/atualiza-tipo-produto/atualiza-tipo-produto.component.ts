import { Router } from '@angular/router';
import { TipoProduto } from './../../../../../view-model/response/product/tipo-produto';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Component, OnInit, Input } from '@angular/core';
import { TipoProdutoService } from '../tipo-produto.service';
import { ModalComponent } from '../../../../shared/modal/modal.component';

@Component({
  selector: 'app-atualiza-tipo-produto',
  templateUrl: './atualiza-tipo-produto.component.html',
  styleUrls: ['./atualiza-tipo-produto.component.scss']
})
export class AtualizaTipoProdutoComponent implements OnInit {

  @Input() largeModal:BsModalRef;
  @Input() tipoProdutoEdit: TipoProduto;

  bsModalRef: BsModalRef = new BsModalRef();

  constructor(private tipoProdutoService: TipoProdutoService,
    private modalService: BsModalService,
    private router: Router) { }

  ngOnInit(): void {
  }
  salvar(evento):void{
    this.tipoProdutoService.update(this.tipoProdutoEdit,"TipoProduto").subscribe(
      {
        next:(sucesso)=>{
          this.mensagemCad("Tipo produto atualizado com sucesso","success","");
          this.cancelar();
        },
        error:(e) =>{
          this.mensagemCad("Tipo produto não atualizado!","danger","");
          console.log(e);
        }
      })
  }

  cancelar(){
    this.largeModal.hide();
  }
  mensagemCad(mensagem:string, tipo:string,redirectTo:string){
    this.bsModalRef = this.modalService.show(ModalComponent);
    this.bsModalRef.content.tipo= tipo;
    this.bsModalRef.content.mensagem= mensagem;
    this.bsModalRef.content.redirectTo = redirectTo;//'/receita';
    this.bsModalRef.content.abrindoDentroDeUmModal = true;
  }


}
