import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { TipoProduto } from '../../../../../view-model/response/product/tipo-produto';
import { ModalComponent } from '../../../../shared/modal/modal.component';
import { TipoProdutoService } from '../tipo-produto.service';

@Component({
  selector: 'app-remove-tipo-produto',
  templateUrl: './remove-tipo-produto.component.html',
  styleUrls: ['./remove-tipo-produto.component.scss']
})
export class RemoveTipoProdutoComponent implements OnInit {

  @Input() largeModal:BsModalRef;
  @Input() tipoProdutoRemove: TipoProduto;
  @Input() listaTiposProdutos:  TipoProduto[];

  bsModalRef: BsModalRef = new BsModalRef();

  constructor(private tipoProdutoService:TipoProdutoService,
    private modalService: BsModalService,
    private router: Router) { }

  ngOnInit(): void {
  }

  salvar(evento):void{
    this.tipoProdutoService.delete(this.tipoProdutoRemove.id,"TipoProduto").subscribe(
      {
        next:(sucesso)=>{
          this.mensagemCad("Tipo Produto removido com sucesso","success","");
          const elementoRemover = this.listaTiposProdutos.indexOf(this.tipoProdutoRemove);
          this.listaTiposProdutos.splice(elementoRemover,1);
          this.cancelar();
        },
        error:(e) =>{
          this.mensagemCad("Tipo Produto não removido!","danger","");
          console.log(e);//gravar log fazer algo
          this.cancelar();
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
