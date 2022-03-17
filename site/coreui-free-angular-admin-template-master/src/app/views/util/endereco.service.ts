import { EnderecoViaCep } from './../../view-model/response/util/endereco-via-cep';
import { Router } from '@angular/router';
import { CookieConsertService } from './../cookie-consert/cookie-consert.service';
import { RequestDefaultService } from './../auth/request-default.service';
import { Endereco } from './../../view-model/response/util/endereco';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class EnderecoService extends  RequestDefaultService<Endereco,Endereco>{

  private urlViaCep:string =  "https://viacep.com.br/ws/";
  private url: string = "Endereco";
  constructor(httpClient: HttpClient,private router : Router,private cookie: CookieConsertService) {
    super(httpClient);
  }

  public getEnderecoByCep(cep : string): Observable<Endereco>{
    return this.httpClient.get<Endereco>(`${this.receitaUrlBase}${this.url}/ByCepAsync/${cep}`);
  }

  public getEnderecoViaCep(cep : string): Observable<EnderecoViaCep>{
    return this.httpClient.get<EnderecoViaCep>(`${this.urlViaCep}${cep}/json`);
  }

}
