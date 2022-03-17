import { Produto } from './../../../../view-model/response/product/produto';
import { Component, OnInit, ViewChild } from '@angular/core';
import { BsModalService, ModalDirective } from 'ngx-bootstrap/modal';
import { PermissaoUsuario } from '../../../../view-model/response/security/permissao-usuario';
import { ProdutoService } from './produto.service';
import { CookieConsertService } from '../../../cookie-consert/cookie-consert.service';
import { AuthorizationService } from '../../../auth/authorization.service';
import { UsuarioResponse } from '../../../../view-model/response/security/usuario';

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

  produtoEditRemove: Produto = new Produto();

  constructor(private authorizationService: AuthorizationService,
    private cookie: CookieConsertService ,
    private modalService: BsModalService,
    private produtosService: ProdutoService) { }

  ngOnInit(): void {
    var valor : UsuarioResponse= JSON.parse(this.cookie.getCookie("usuario"));
    valor.permissaoUsuarios.forEach(permissao => {
      this.acessoPermitido = permissao.permissao.nome == "Root" ||  permissao.permissao.nome == "Administrador";
      this.permissaoSistema = permissao;
    });
    debugger;
    this.produtosService.getAll("Produto").subscribe({
      next:(t)=>{
          debugger;
          this.produtos = t;
      },
      error:(e)=>{
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
    this.produtoEditRemove = this.produtos.filter((depart)=>depart.id == id)[0];
    this.largeModalRemove.show();
  }
}
