import { CookieConsertService } from './../cookie-consert/cookie-consert.service';
import { Router } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { RequestDefaultService } from '../auth/request-default.service';
import { Injectable } from '@angular/core';
import { Permissao } from '../../view-model/response/security/permissao';
import { AuthorizationService } from '../auth/authorization.service';

@Injectable({
  providedIn: 'root'
})
export class PermissaoService extends RequestDefaultService<Permissao,Permissao> {

  constructor(httpClient: HttpClient, private router : Router,private cookie: CookieConsertService,
    private authorizationService: AuthorizationService) {
      super(httpClient);
    }
}
