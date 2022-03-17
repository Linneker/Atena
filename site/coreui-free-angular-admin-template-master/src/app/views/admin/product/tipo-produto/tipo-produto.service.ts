import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { TipoProdutoCadastroRequest } from '../../../../view-model/request/product/tipo-produto-cadastro-request';
import { TipoProduto } from '../../../../view-model/response/product/tipo-produto';
import { AuthorizationService } from '../../../auth/authorization.service';
import { RequestDefaultService } from '../../../auth/request-default.service';
import { CookieConsertService } from '../../../cookie-consert/cookie-consert.service';

@Injectable({
  providedIn: 'root'
})
export class TipoProdutoService extends  RequestDefaultService<TipoProdutoCadastroRequest,TipoProduto>
{

    constructor(httpClient: HttpClient, private router : Router,private cookie: CookieConsertService,
      private authorizationService: AuthorizationService) {
        super(httpClient);
    }



}
