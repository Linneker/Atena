import { Router } from '@angular/router';
import { Component, OnInit, Input } from '@angular/core';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { TipoValorProdutoCadastroRequest } from '../../../../../view-model/request/product/price/tipo-valor-produto-cadastro-request';
import { TipoValorProduto } from '../../../../../view-model/response/product/price/tipo-valor-produto';
import { ModalComponent } from '../../../../shared/modal/modal.component';
import { TipoValorProdutoService } from '../tipo-valor-produto.service';

@Component({
  selector: 'app-cadastro-tipo-valor-produto',
  templateUrl: './cadastro-tipo-valor-produto.component.html',
  styleUrls: ['./cadastro-tipo-valor-produto.component.scss']
})
export class CadastroTipoValorProdutoComponent implements OnInit {

  @Input() largeModal:BsModalRef;
  @Input() listaTiposValoresProdutos:  TipoValorProduto[];

  bsModalRef: BsModalRef = new BsModalRef();
  tipoValorProduto : TipoValorProdutoCadastroRequest= new TipoValorProdutoCadastroRequest();

  constructor(private tipoValorProdutoService: TipoValorProdutoService,
    private modalService: BsModalService,
    private router: Router) { }

  ngOnInit(): void {
  }
  salvar(evento):void{
    this.tipoValorProdutoService.add(this.tipoValorProduto,"TipoValorProduto/Add").subscribe(
      {
        next:(sucesso)=>{
          this.mensagemCad("Tipo valor produto cadastrado com sucesso","success","");
          this.tipoValorProdutoService.getAll("TipoValorProduto").subscribe(t=>{
            while(this.listaTiposValoresProdutos.length >0){
              this.listaTiposValoresProdutos.pop();
            }
            t.forEach(t=>{
              this.listaTiposValoresProdutos.push(t);
            });
          });

          this.cancelar();
        },
        error:(e) =>{
          this.mensagemCad("Tipo valor produto não cadastrado!","danger","");
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
