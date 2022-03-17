import { ValorProduto } from './../../../../../view-model/response/product/price/valor-produto';
import { TipoProdutoService } from './../../tipo-produto/tipo-produto.service';
import { TipoValorProdutoService } from './../../tipo-valor-produto/tipo-valor-produto.service';
import { TipoProduto } from './../../../../../view-model/response/product/tipo-produto';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { Component, Input, OnInit } from '@angular/core';
import { NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { Produto } from '../../../../../view-model/response/product/produto';
import { CustomDateParserFormatter } from '../../../../usuario/cadastro-usuario/cadastro-usuario.component';
import { Router } from '@angular/router';
import { ProdutoService } from '../produto.service';
import { ProdutoCadastroRequest } from '../../../../../view-model/request/product/produto-cadastro-request';
import { ModalComponent } from '../../../../shared/modal/modal.component';

@Component({
  selector: 'app-cadastro-produto',
  templateUrl: './cadastro-produto.component.html',
  styleUrls: ['./cadastro-produto.component.scss'],
  providers: [
    {provide: NgbDateParserFormatter, useClass: CustomDateParserFormatter}
  ]
})
export class CadastroProdutoComponent implements OnInit {

  produto: ProdutoCadastroRequest =  new ProdutoCadastroRequest();
  dataVencimento: any;
  valorCompraProduto: number;
  margemDeLucro: number;
  tipoValorProdutoCompraId: string;
  tipoValorProdutoVendaId: string;

  @Input() largeModal:BsModalRef;
  @Input() listaProdutos:  Produto[];
  bsModalRef: BsModalRef = new BsModalRef();
  tipoProdutos: TipoProduto[];

  constructor(private produtoService: ProdutoService,
    private modalService: BsModalService,
    private router: Router,
    private tipoValorProdutoService: TipoValorProdutoService,
    private tipoProdutoService: TipoProdutoService) { }

  ngOnInit(): void {
    this.tipoValorProdutoService.getTipoValorProdutoByNome("Compra").subscribe({
      next: (t)=>{
        this.tipoValorProdutoCompraId = t.id;
      }
    });
    this.tipoValorProdutoService.getTipoValorProdutoByNome("Venda").subscribe({
      next: (t)=>{
        this.tipoValorProdutoVendaId = t.id;
      }
    });

    this.tipoProdutoService.getAll("TipoProduto").subscribe({
      next: (t)=>{
        this.tipoProdutos= t;
      }
    });
  }
  salvar(evento):void{
    var valorProduto : ValorProduto = new ValorProduto();
    valorProduto.tipoValorProdutoId = this.tipoValorProdutoCompraId;
    valorProduto.valor = this.valorCompraProduto;
    this.produto.valorProdutos.push(valorProduto);

    valorProduto = new ValorProduto();
    valorProduto.tipoValorProdutoId = this.tipoValorProdutoVendaId;
    valorProduto.valor = Number(Number(this.valorCompraProduto) + Number(Number(this.valorCompraProduto * this.margemDeLucro)/100));
    this.produto.valorProdutos.push(valorProduto);
    console.log(valorProduto.valor);
    this.produto.validade = null;

    this.produtoService.add(this.produto,"Produto/Add").subscribe({
      next:(sucesso)=>{
        this.mensagemCad("Produto cadastrado com sucesso","success","");
        this.produtoService.getAll("Produto").subscribe(t=>{
          while(this.listaProdutos.length >0){
            this.listaProdutos.pop();
          }
          t.forEach(t=>{
            this.listaProdutos.push(t);
          });
        });

        this.cancelar();

      },
      error: (e)=>{
        this.mensagemCad("Produto não cadastrado!","danger","");
        console.log(e);
      }
    });
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
