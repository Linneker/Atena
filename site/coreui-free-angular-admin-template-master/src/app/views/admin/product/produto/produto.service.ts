import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { ProdutoCadastroRequest } from '../../../../view-model/request/product/produto-cadastro-request';
import { Produto } from '../../../../view-model/response/product/produto';
import { AuthorizationService } from '../../../auth/authorization.service';
import { RequestDefaultService } from '../../../auth/request-default.service';
import { CookieConsertService } from '../../../cookie-consert/cookie-consert.service';

@Injectable({
  providedIn: 'root'
})
export class ProdutoService extends  RequestDefaultService<ProdutoCadastroRequest,Produto>{

  constructor(httpClient: HttpClient, private router : Router,private cookie: CookieConsertService,
    private authorizationService: AuthorizationService) {
      super(httpClient);
     }


}
