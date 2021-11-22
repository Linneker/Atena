import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AutorizacaoApiResponse } from '../view-model/response/autorizacao-api-response';
import { AutorizacaoApiRequest } from '../view-model/request/autorizacao-api-request/autorizacao-api-request.module';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {

  private autorizacaoURl: string = "https://bardochiquinho.acmesistemas.com.br/api/AutorizacaoApi";
  _request: AutorizacaoApiRequest = new AutorizacaoApiRequest();
  _autoricaoApiResponse: AutorizacaoApiResponse = new AutorizacaoApiResponse();


  constructor(protected httpClient: HttpClient) { }

  setCompetencias(): void {
    this.autorizacaoApi().subscribe({
      next: (autorizcaoApiResponse: AutorizacaoApiResponse) => {
        this._autoricaoApiResponse = autorizcaoApiResponse;
        sessionStorage.setItem("jwt",this._autoricaoApiResponse.accessToken);
        sessionStorage.setItem("expiration",this._autoricaoApiResponse.expiration);

      },
      error: err => console.log("ERRO: ", err)
    });
  }

  autorizacaoApi(): Observable<AutorizacaoApiResponse> {
    return this.httpClient.post<AutorizacaoApiResponse>(this.autorizacaoURl, this._request, {
      headers: new HttpHeaders()
    });
  }
  get getCompetencias(): AutorizacaoApiResponse {
    return this._autoricaoApiResponse;
  }
}
