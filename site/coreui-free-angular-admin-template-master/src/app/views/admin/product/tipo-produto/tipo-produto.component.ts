import { Permissao } from './../../../../view-model/response/security/permissao';
import { Component, OnInit, ViewChild } from '@angular/core';
import { BsModalService, ModalDirective } from 'ngx-bootstrap/modal';
import { UsuarioResponse } from '../../../../view-model/response/security/usuario';
import { AuthorizationService } from '../../../auth/authorization.service';
import { CookieConsertService } from '../../../cookie-consert/cookie-consert.service';
import { PermissaoUsuario } from '../../../../view-model/response/security/permissao-usuario';
import { TipoProduto } from '../../../../view-model/response/product/tipo-produto';
import { TipoProdutoService } from './tipo-produto.service';
import { isDebuggerStatement } from 'typescript';

@Component({
  selector: 'app-tipo-produto',
  templateUrl: './tipo-produto.component.html',
  styleUrls: ['./tipo-produto.component.scss']
})
export class TipoProdutoComponent implements OnInit {

  @ViewChild('largeModal') public largeModal: ModalDirective;
  @ViewChild('largeModalEdit') public largeModalEdit: ModalDirective;
  @ViewChild('largeModalRemove') public largeModalRemove: ModalDirective;

  acessoPermitido: boolean = false;
  permissaoSistema: PermissaoUsuario = new PermissaoUsuario();
  tiposProdutos: TipoProduto[]=[];

  tipoProdutoEditRemove: TipoProduto = new TipoProduto();

  constructor(private authorizationService: AuthorizationService,
    private cookie: CookieConsertService ,
    private modalService: BsModalService,
    private tipoProdutosService:TipoProdutoService) { }

  ngOnInit(): void {
    var valor : UsuarioResponse= JSON.parse(this.cookie.getCookie("usuario"));
    valor.permissaoUsuarios.forEach(permissao => {
      this.acessoPermitido = permissao.permissao.nome == "Root" ||  permissao.permissao.nome == "Administrador";
      this.permissaoSistema = permissao;
    });
    debugger;
    this.tipoProdutosService.getAll("TipoProduto").subscribe({
      next:(t)=>{
          debugger;
          this.tiposProdutos = t;
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
    this.tipoProdutoEditRemove = this.tiposProdutos.filter((depart)=>depart.id == id)[0];
    this.largeModalEdit.show();
  }

  openDeletar(id){
    this.tipoProdutoEditRemove = this.tiposProdutos.filter((depart)=>depart.id == id)[0];
    this.largeModalRemove.show();
  }
}
