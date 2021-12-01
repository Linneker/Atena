import { Observable } from 'rxjs';
import { ResponseApi } from './../view-model/response/response-api';
import { TokenInterceptorService } from './token-interceptor.service';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class RequestDefaultService<T,P> extends TokenInterceptorService{

  receitaUrlBase: string ="https://bardochiquinho.acmesistemas.com.br/api/";

  constructor(protected httpClient: HttpClient)  {
    super();
  }

  add (objeto: T, servico : string) : Observable<ResponseApi>
  {
    return this.httpClient.post<ResponseApi>(`${this.receitaUrlBase}${servico}`,objeto);
  }

  getAll (servico : string) : Observable<P[]>
  {
    return this.httpClient.get<P[]>(`${this.receitaUrlBase}${servico}`);
  }

  getById (id:string, servico : string) : Observable<P[]>
  {
    return this.httpClient.get<P[]>(`${this.receitaUrlBase}${servico}/${id}`);
  }
}
