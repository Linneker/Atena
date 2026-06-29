import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';

export type TipoRep = 'RepP' | 'RepC';

export interface EnderecoRep {
  logradouro: string; numero?: string | null; complemento?: string | null;
  bairro?: string | null; cidade: string; uf: string; cep?: string | null;
}

export interface ConfiguracaoRepPayload {
  empresaId: string;
  tipo: TipoRep;
  razaoSocial: string;
  cnpjCei: string;
  cno?: string | null;
  inscricaoEstadual?: string | null;
  cnaePrincipal?: string | null;
  endereco: EnderecoRep;
  certificadoId: string;
  responsavelCpf: string;
  responsavelNome: string;
}

export interface ConfiguracaoRepResponse extends ConfiguracaoRepPayload {
  id: string;
}

export interface ValidacaoRepItem { item: string; ok: boolean; mensagem?: string | null; }
export interface ValidacaoRep { apto: boolean; checagens: ValidacaoRepItem[]; }

export interface ExportarAfdPayload {
  empresaId: string; periodoInicio: string; periodoFim: string;
}
export interface ExportacaoResponse {
  exportacaoId: string; status: string;
  arquivoUrl?: string | null; assinaturaUrl?: string | null; hashSha256?: string | null;
}

@Injectable({ providedIn: 'root' })
export class Oficial671Service {
  private readonly http = inject(HttpClient);
  private readonly base = `${environment.apiBaseUrl}/api/v1/rh/ponto/671`;

  salvarConfiguracao(p: ConfiguracaoRepPayload): Observable<{ configuracaoId: string; criada: boolean }> {
    return this.http.post<{ configuracaoId: string; criada: boolean }>(`${this.base}/configuracao`, p);
  }

  obterConfiguracao(empresaId: string): Observable<ConfiguracaoRepResponse> {
    return this.http.get<ConfiguracaoRepResponse>(`${this.base}/configuracao/${empresaId}`);
  }

  validar(empresaId: string): Observable<ValidacaoRep> {
    return this.http.get<ValidacaoRep>(`${this.base}/validar/${empresaId}`);
  }

  exportarAfd(p: ExportarAfdPayload): Observable<ExportacaoResponse> {
    return this.http.post<ExportacaoResponse>(`${this.base}/afd/exportar`, p);
  }

  exportarAej(p: ExportarAfdPayload): Observable<ExportacaoResponse> {
    return this.http.post<ExportacaoResponse>(`${this.base}/aej/exportar`, p);
  }

  downloadAfdUrl(exportacaoId: string): string {
    return `${this.base}/afd/${exportacaoId}/download`;
  }
  downloadAejUrl(exportacaoId: string, formato?: 'json' | 'jws'): string {
    const q = formato === 'jws' ? '?formato=jws' : '';
    return `${this.base}/aej/${exportacaoId}/download${q}`;
  }
  segundaViaPdfUrl(marcacaoId: string): string {
    return `${this.base}/comprovantes/${marcacaoId}.pdf`;
  }
}
