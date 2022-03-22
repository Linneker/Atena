import { Router } from '@angular/router';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { PermissaoService } from './../../security/permissao.service';
import { PermissaoUsuario } from './../../../view-model/response/security/permissao-usuario';
import { Permissao } from './../../../view-model/response/security/permissao';
import { UsuarioService } from './../usuario.service';
import { EnderecoService } from './../../util/endereco.service';
import { EnderecoUsuario } from './../../../view-model/response/util/endereco-usuario';
import { UsuarioResponse } from './../../../view-model/response/security/usuario';
import { Component, OnInit, Injectable, ElementRef, Input } from '@angular/core';
import { ModalComponent } from '../../shared/modal/modal.component';
import { NgbCalendar, NgbDateAdapter, NgbDateParserFormatter, NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { TipoProduto } from '../../../view-model/response/product/tipo-produto';
import { PermissaoAcesso } from '../../../view-model/util/permissa-acessos';




@Injectable()
export class CustomDateParserFormatter extends NgbDateParserFormatter {

  readonly DELIMITER = '/';

  parse(value: string): NgbDateStruct | null {
    if (value) {
      const date = value.split(this.DELIMITER);
      return {
        day : parseInt(date[0], 10),
        month : parseInt(date[1], 10),
        year : parseInt(date[2], 10)
      };
    }
    return null;
  }

  format(date: NgbDateStruct | null): string {
    return date ? date.day + this.DELIMITER + date.month + this.DELIMITER + date.year : '';
  }
}


@Component({
  selector: 'app-cadastro-usuario',
  templateUrl: './cadastro-usuario.component.html',
  styleUrls: ['./cadastro-usuario.component.scss'],
  providers: [
    {provide: NgbDateParserFormatter, useClass: CustomDateParserFormatter}
  ]
})
export class CadastroUsuarioComponent implements OnInit {

  ngDp : NgbDateStruct;
  usuario: UsuarioResponse=new UsuarioResponse();
  enderecoUsuario: EnderecoUsuario = new EnderecoUsuario() ;
  permissoes: Permissao[];
  permissaoUsuario: PermissaoUsuario = new PermissaoUsuario();
  permissao : Permissao = new Permissao();
  dataNascimento: any;
  permissaoAcesso: PermissaoAcesso = new PermissaoAcesso();

  @Input() largeModal:BsModalRef;
  bsModalRef: BsModalRef = new BsModalRef();

  constructor(private ngbCalendar: NgbCalendar, private dateAdapter: NgbDateAdapter<string>,
    private el: ElementRef,
    private enderecoService:EnderecoService,
    private usuarioService: UsuarioService,
    private permissoesService: PermissaoService,
    private modalService: BsModalService,
    private router: Router) {}

  ngOnInit(): void {
    this.permissoesService.getAll("Permissao/GetQueryables").subscribe({
      next: t =>{
        this.permissoes = t;
      },
      error: e => {
          console.log(e);
      }
    });
  }

  salvar():void{
    this.usuario.enderecoUsuarios = [];
    this.enderecoUsuario.usuario = null;
    this.usuario.enderecoUsuarios.push(this.enderecoUsuario);
    this.usuario.permissaoUsuarios =[];
    this.usuario.permissaoUsuarios.push(this.permissaoUsuario);
    this.usuario.dataNascimento = new Date(`${this.dataNascimento.year}-${(this.dataNascimento.month < 10 ? `0${this.dataNascimento.month}` : `${this.dataNascimento.month}`) }-${(this.dataNascimento.day < 10 ? `0${(this.dataNascimento.day)}` : `${(this.dataNascimento.day)}`) }`);
    console.log(this.usuario.dataNascimento);
    this.usuarioService.AddAsync(this.usuario,"Usuario/Add/Async").subscribe({
      next: s => {
        this.mensagemCad("Usuario cadastrado com Sucesso!","success","/buttons/usuario");
        this.largeModal.hide();
        console.log(s);
      },
      error: e =>{
        this.mensagemCad("Problema ao cadastradar o usuario!","danger","/buttons/usuario");
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
  }
  pesquisarCep(){
    this.enderecoService.getEnderecoByCep(this.enderecoUsuario.endereco.cep).subscribe({
      next: sucesso => {
        if(sucesso){
          this.enderecoUsuario.endereco.pais = sucesso.pais;
          this.enderecoUsuario.endereco.estado = sucesso.estado;
          this.enderecoUsuario.endereco.cidade = sucesso.cidade;
          this.enderecoUsuario.endereco.bairro = sucesso.bairro;
          this.enderecoUsuario.endereco.rua  = sucesso.rua;
        }else{
          this.enderecoService.getEnderecoViaCep(this.enderecoUsuario.endereco.cep).subscribe({
            next: sucessoViaCep => {
              this.enderecoUsuario.endereco.pais = "";
              this.enderecoUsuario.endereco.estado = sucessoViaCep.uf;
              this.enderecoUsuario.endereco.cidade = sucessoViaCep.localidade;
              this.enderecoUsuario.endereco.bairro = sucessoViaCep.bairro;
              this.enderecoUsuario.endereco.rua  =  sucessoViaCep.logradouro;
            },
            error: erroViaCep =>{
              this.enderecoUsuario.endereco.pais = "";
              this.enderecoUsuario.endereco.estado = "";
              this.enderecoUsuario.endereco.cidade = "";
              this.enderecoUsuario.endereco.bairro = "";
              this.enderecoUsuario.endereco.rua  ="";
            }
          });
        }
      },
      error: erro =>{
        this.enderecoService.getEnderecoViaCep(this.enderecoUsuario.endereco.cep).subscribe({
          next: sucessoViaCep => {
            this.enderecoUsuario.endereco.pais = "";
            this.enderecoUsuario.endereco.estado = sucessoViaCep.uf;
            this.enderecoUsuario.endereco.cidade = sucessoViaCep.localidade;
            this.enderecoUsuario.endereco.bairro = sucessoViaCep.bairro;
            this.enderecoUsuario.endereco.rua  =  sucessoViaCep.logradouro;
          },
          error: erroViaCep =>{
            this.enderecoUsuario.endereco.pais = "";
            this.enderecoUsuario.endereco.estado = "";
            this.enderecoUsuario.endereco.cidade = "";
            this.enderecoUsuario.endereco.bairro = "";
            this.enderecoUsuario.endereco.rua  ="";
          }
        });
      }
    });
  }

  proximoEndereco(evento){
    let elementoDadosPessoais = this.el.nativeElement.querySelector("#dados-pessoais");
    elementoDadosPessoais.classList.remove('show');
    elementoDadosPessoais.classList.remove('active');

    let elementoDadosPessoaisTab = this.el.nativeElement.querySelector("#dados-pessoais-tab");
    elementoDadosPessoaisTab.classList.remove('active');

    let elementoEndereco = this.el.nativeElement.querySelector("#endereco");
    elementoEndereco.classList.remove('hide');
    elementoEndereco.classList.add('show');
    elementoEndereco.classList.add('active');

    let elementoEnderecoTab = this.el.nativeElement.querySelector("#endereco-tab");
    elementoEnderecoTab.classList.add('active');
  }
  anteriorDadosPessoa(evento){
    let elementoDadosPessoais = this.el.nativeElement.querySelector("#dados-pessoais");
    elementoDadosPessoais.classList.add('show');
    elementoDadosPessoais.classList.add('active');

    let elementoDadosPessoaisTab = this.el.nativeElement.querySelector("#dados-pessoais-tab");
    elementoDadosPessoaisTab.classList.add('active');

    let elementoEndereco = this.el.nativeElement.querySelector("#endereco");
    elementoEndereco.classList.add('hide');
    elementoEndereco.classList.remove('show');
    elementoEndereco.classList.remove('active');

    let elementoEnderecoTab = this.el.nativeElement.querySelector("#endereco-tab");
    elementoEnderecoTab.classList.remove('active');
  }

  proximoDadosUsuario(evento){
    let elementoDadosPessoais = this.el.nativeElement.querySelector("#dados-usuario");
    elementoDadosPessoais.classList.add('show');
    elementoDadosPessoais.classList.add('active');

    let elementoDadosPessoaisTab = this.el.nativeElement.querySelector("#dados-usuario-tab");
    elementoDadosPessoaisTab.classList.add('active');

    let elementoEndereco = this.el.nativeElement.querySelector("#endereco");
    elementoEndereco.classList.add('hide');
    elementoEndereco.classList.remove('show');
    elementoEndereco.classList.remove('active');

    let elementoEnderecoTab = this.el.nativeElement.querySelector("#endereco-tab");
    elementoEnderecoTab.classList.remove('active');
  }

  anteriorEndereco(event){
    let elementoDadosPessoais = this.el.nativeElement.querySelector("#dados-usuario");
    elementoDadosPessoais.classList.remove('show');
    elementoDadosPessoais.classList.remove('active');

    let elementoDadosPessoaisTab = this.el.nativeElement.querySelector("#dados-usuario-tab");
    elementoDadosPessoaisTab.classList.remove('active');

    let elementoEndereco = this.el.nativeElement.querySelector("#endereco");
    elementoEndereco.classList.remove('hide');
    elementoEndereco.classList.add('show');
    elementoEndereco.classList.add('active');

    let elementoEnderecoTab = this.el.nativeElement.querySelector("#endereco-tab");
    elementoEnderecoTab.classList.add('active');
  }

}
