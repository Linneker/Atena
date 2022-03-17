import { TipoProduto } from './../../../../../view-model/response/product/tipo-produto';
import { Router } from '@angular/router';
import { Component, OnInit, Input } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { ModalComponent } from '../../../../shared/modal/modal.component';
import { TipoProdutoService } from '../tipo-produto.service';
import { TipoProdutoCadastroRequest } from '../../../../../view-model/request/product/tipo-produto-cadastro-request';

@Component({
  selector: 'app-cadastro-tipo-produto',
  templateUrl: './cadastro-tipo-produto.component.html',
  styleUrls: ['./cadastro-tipo-produto.component.scss']
})
export class CadastroTipoProdutoComponent implements OnInit {
  @Input() largeModal:BsModalRef;
  @Input() listaTiposProtudos:  TipoProduto[];

  bsModalRef: BsModalRef = new BsModalRef();
  tipoProduto : TipoProdutoCadastroRequest= new TipoProdutoCadastroRequest();

  constructor(private departamentoService: TipoProdutoService,
    private modalService: BsModalService,
    private router: Router) { }

  ngOnInit(): void {
  }
  salvar(evento):void{
    this.departamentoService.add(this.tipoProduto,"TipoProduto/Add").subscribe(
      {
        next:(sucesso)=>{
          this.mensagemCad("Tipo Produto cadastrado com sucesso","success","");
          this.departamentoService.getAll("TipoProduto").subscribe(t=>{
            while(this.listaTiposProtudos.length >0){
              this.listaTiposProtudos.pop();
            }
            t.forEach(t=>{
              this.listaTiposProtudos.push(t);
            });
          });

          this.cancelar();
        },
        error:(e) =>{
          this.mensagemCad("Tipo Produto não cadastrado!","danger","");
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
