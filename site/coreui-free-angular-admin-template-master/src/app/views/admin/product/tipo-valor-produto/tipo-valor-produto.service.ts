import { Observable } from 'rxjs';
import { TipoValorProduto } from './../../../../view-model/response/product/price/tipo-valor-produto';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { TipoValorProdutoCadastroRequest } from '../../../../view-model/request/product/price/tipo-valor-produto-cadastro-request';
import { AuthorizationService } from '../../../auth/authorization.service';
import { RequestDefaultService } from '../../../auth/request-default.service';
import { CookieConsertService } from '../../../cookie-consert/cookie-consert.service';

@Injectable({
  providedIn: 'root'
})
export class TipoValorProdutoService extends  RequestDefaultService<TipoValorProdutoCadastroRequest,TipoValorProduto> {

  constructor(httpClient: HttpClient, private router : Router,private cookie: CookieConsertService,
    private authorizationService: AuthorizationService) {
      super(httpClient);
     }

     getTipoValorProdutoByNome(nome: string):Observable<TipoValorProduto> {
      return this.httpClient.get<TipoValorProduto>(`${this.receitaUrlBase}TipoValorProduto/Nome/${nome}`);
     }
}
