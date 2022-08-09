import { Produto } from './../../../../view-model/response/product/produto';
import { Component, OnInit, ViewChild } from '@angular/core';
import { BsModalRef, BsModalService, ModalDirective } from 'ngx-bootstrap/modal';
import { PermissaoUsuario } from '../../../../view-model/response/security/permissao-usuario';
import { ProdutoService } from './produto.service';
import { CookieConsertService } from '../../../cookie-consert/cookie-consert.service';
import { AuthorizationService } from '../../../auth/authorization.service';
import { UsuarioResponse } from '../../../../view-model/response/security/usuario';
import { PermissaoAcesso } from '../../../../view-model/util/permissa-acessos';
import { ModalComponent } from '../../../shared/modal/modal.component';

@Component({
  selector: 'app-produto',
  templateUrl: './produto.component.html',
  styleUrls: ['./produto.component.scss']
})
export class ProdutoComponent implements OnInit {

  @ViewChild('largeModal') public largeModal: ModalDirective;
  @ViewChild('largeModalEdit') public largeModalEdit: ModalDirective;
  @ViewChild('largeModalRemove') public largeModalRemove: ModalDirective;

  acessoPermitido: boolean = false;
  permissaoSistema: PermissaoUsuario = new PermissaoUsuario();
  produtos: Produto[]=[];
  acessos: string="";
  permissaoAcesso: PermissaoAcesso = new PermissaoAcesso();
  bsModalRef: BsModalRef = new BsModalRef();

  produtoEditRemove: Produto = new Produto();

  constructor(private authorizationService: AuthorizationService,
    private cookie: CookieConsertService ,
    private modalService: BsModalService,
    private produtosService: ProdutoService) { }

  ngOnInit(): void {
    var valor : UsuarioResponse= JSON.parse(this.cookie.getCookie("usuario"));
    this.permissaoAcesso = this.authorizationService.hasAutorization("Produto");

    debugger;

    this.produtosService.getAll("Produto").subscribe({
      next:(t)=>{
          debugger;
          this.produtos = t;
      },
      error:(e)=>{
        if(e.status == 401){

        }else{
          this.mensagemCad(e.mensagem,e.status,"");
        }
          console.log(e);
      }
    });
    debugger;
  }
  openCadastro(){
    this.largeModal.show();
  }

  openAtualizar(id){

    this.produtoEditRemove = this.produtos.filter((depart)=>depart.id == id)[0];
    this.largeModalEdit.show();
  }

  openDeletar(id){
    debugger;
    this.produtoEditRemove = this.produtos.filter((depart)=>depart.id == id)[0];
    this.largeModalRemove.show();
  }
  mensagemCad(mensagem:string, tipo:string,redirectTo:string){
    this.bsModalRef = this.modalService.show(ModalComponent);
    this.bsModalRef.content.tipo= tipo;
    this.bsModalRef.content.mensagem= mensagem;
    this.bsModalRef.content.redirectTo = redirectTo;//'/receita';
    this.bsModalRef.content.abrindoDentroDeUmModal = true;
  }
}
