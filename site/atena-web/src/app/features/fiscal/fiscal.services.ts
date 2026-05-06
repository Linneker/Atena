import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';
import { CrudService } from '@shared/crud/crud.service';

export interface ConfiguracaoFiscal { id?: string; ambiente: 'HOMOLOGACAO' | 'PRODUCAO'; certificadoNome?: string; certificadoVencimento?: string; serieNFe: number; }
export interface NFe { id?: string; numero: string; chave: string; cliente: string; valor: number; status: string; emissao: string; }

@Injectable({ providedIn: 'root' })
export class ConfiguracaoFiscalService {
  private readonly http = inject(HttpClient);
  obter(): Observable<ConfiguracaoFiscal> {
    return this.http.get<ConfiguracaoFiscal>(`${environment.apiUrl}/${environment.apiVersion}/configuracao-fiscal`);
  }
  alterarAmbiente(ambiente: ConfiguracaoFiscal['ambiente']): Observable<unknown> {
    return this.http.post(`${environment.apiUrl}/${environment.apiVersion}/configuracao-fiscal/ambiente`, { ambiente });
  }
  importarCertificado(file: File, senha: string): Observable<unknown> {
    const fd = new FormData();
    fd.append('certificado', file);
    fd.append('senha', senha);
    return this.http.post(`${environment.apiUrl}/${environment.apiVersion}/configuracao-fiscal/certificado`, fd);
  }
}

@Injectable({ providedIn: 'root' })
export class NFeService extends CrudService<NFe> {
  protected readonly resource = 'nfe';
  cancelar(id: string, justificativa: string): Observable<unknown> {
    return this.http.post(`${environment.apiUrl}/${environment.apiVersion}/nfe/${id}/cancelar`, { justificativa });
  }
  emitirCce(id: string, correcao: string): Observable<unknown> {
    return this.http.post(`${environment.apiUrl}/${environment.apiVersion}/nfe/${id}/cce`, { correcao });
  }
}
