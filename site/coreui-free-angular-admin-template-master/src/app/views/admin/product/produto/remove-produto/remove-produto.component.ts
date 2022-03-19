import { Component, OnInit, Input } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Produto } from '../../../../../view-model/response/product/produto';
import { ModalComponent } from '../../../../shared/modal/modal.component';
import { ProdutoService } from '../produto.service';

@Component({
  selector: 'app-remove-produto',
  templateUrl: './remove-produto.component.html',
  styleUrls: ['./remove-produto.component.scss']
})
export class RemoveProdutoComponent implements OnInit {

  @Input() largeModal:BsModalRef;
  @Input() produtoRemove: Produto;
  @Input() listaProdutos:  Produto[];

  bsModalRef: BsModalRef = new BsModalRef();

  constructor(private produtoService:ProdutoService,
    private modalService: BsModalService,
    private router: Router) { }

  ngOnInit(): void {
  }

  salvar(evento):void{
    this.produtoService.delete(this.produtoRemove.id,"Produto").subscribe(
      {
        next:(sucesso)=>{
          this.mensagemCad("Tipo Produto removido com sucesso","success","");
          const elementoRemover = this.listaProdutos.indexOf(this.produtoRemove);
          this.listaProdutos.splice(elementoRemover,1);
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
