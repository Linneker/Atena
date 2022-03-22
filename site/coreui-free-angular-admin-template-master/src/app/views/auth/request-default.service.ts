import { Observable } from 'rxjs';
import { TokenInterceptorService } from './token-interceptor.service';
import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ResponseApi } from '../../view-model/response/util/response-api';
import { environment } from '../../../environments/environment.prod';

@Injectable({
  providedIn: 'root'
})
export class RequestDefaultService<T,P> extends TokenInterceptorService{

  receitaUrlBase: string =environment.url;

  constructor(protected httpClient: HttpClient)  {
    super();
  }

  add (objeto: T, servico : string) : Observable<ResponseApi>
  {
    return this.httpClient.post<ResponseApi>(`${this.receitaUrlBase}${servico}`,objeto);
  }
  update (objeto: T, servico : string) : Observable<any>
  {
    return this.httpClient.put<any>(`${this.receitaUrlBase}${servico}`,objeto);
  }
  delete (objeto: string, servico : string) : Observable<any>
  {
    return this.httpClient.delete<any>(`${this.receitaUrlBase}${servico}/Remove/${objeto}`);
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
