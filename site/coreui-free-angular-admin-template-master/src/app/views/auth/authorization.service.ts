import { environment } from './../../../environments/environment';
import { Observable } from 'rxjs';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { AutorizacaoApiRequest } from '../../view-model/request/autorizacao-api-request/autorizacao-api-request.module';
import { AutorizacaoApiResponse } from '../../view-model/response/autorizacao-api-response';
import { PermissaoAcesso } from '../../view-model/util/permissa-acessos';
import { UsuarioResponse } from '../../view-model/response/security/usuario';
import { CookieConsertService } from '../cookie-consert/cookie-consert.service';

@Injectable({
  providedIn: 'root'
})
export class AuthorizationService {

  private autorizacaoURl: string = environment.url+"AutorizacaoApi";
  _request: AutorizacaoApiRequest = new AutorizacaoApiRequest();
  _autoricaoApiResponse: AutorizacaoApiResponse = new AutorizacaoApiResponse();


  constructor(protected httpClient: HttpClient,
    private cookie: CookieConsertService) { }

  setCompetencias(): void {
    const jwt = sessionStorage.getItem('jwt') || 'teste';
    let tokenValido = (Date.parse(sessionStorage.getItem("expiration")) >= Date.now());

    if( jwt=='teste' || !tokenValido){
      this.autorizacaoApi().subscribe({
        next: (autorizcaoApiResponse: AutorizacaoApiResponse) => {
          debugger;
          this._autoricaoApiResponse = autorizcaoApiResponse;
          sessionStorage.setItem("jwt",this._autoricaoApiResponse.accessToken);
          sessionStorage.setItem("expiration",this._autoricaoApiResponse.expiration);

        },
        error: err => console.log("ERRO: ", err)
      });
    }
  }

  autorizacaoApi(): Observable<AutorizacaoApiResponse> {
    return this.httpClient.post<AutorizacaoApiResponse>(this.autorizacaoURl, this._request, {
      headers: new HttpHeaders()
    });
  }
  get getCompetencias(): AutorizacaoApiResponse {
    return this._autoricaoApiResponse;
  }

  hasAutorization(tela:string) : PermissaoAcesso {
    var permissoes : UsuarioResponse= JSON.parse(this.cookie.getCookie("usuario"));
    debugger;
    var perm : PermissaoAcesso = new PermissaoAcesso();

    permissoes.permissaoUsuarios.forEach(permissao => {
      if(permissao.tela.name==tela || permissao.tela.name=="All"){
      let valoresPermissao = permissao.acesso.replace(" ","").split(',');
        valoresPermissao.forEach(valorPermissao =>{
          if(valorPermissao=="Read" || valorPermissao=="All")
          perm.read = true;
          if(valorPermissao=="Add" || valorPermissao=="All")
          perm.add = true;
          if(valorPermissao=="Update" || valorPermissao=="All")
          perm.update = true;
          if(valorPermissao=="Delete" || valorPermissao=="All")
          perm.delete = true;
        });
      }
    });
    return perm;
  }
}
