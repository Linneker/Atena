import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';

export type TipoMarcacao = 'Entrada' | 'SaidaAlmoco' | 'VoltaAlmoco' | 'Saida' | 'Pausa' | 'RetornoPausa';
export type OrigemMarcacao = 'Web' | 'MobileApp' | 'Kiosk' | 'Manual' | 'Importacao';
export type StatusMarcacao = 'Valida' | 'AjusteSolicitado' | 'Ajustada' | 'Invalida';
export type TipoAjuste = 'AlteracaoHora' | 'Inclusao' | 'Exclusao' | 'Justificativa';
export type StatusAjuste = 'Pendente' | 'Aprovado' | 'Rejeitado' | 'Cancelado';
export type StatusFechamentoPonto = 'Aberto' | 'EmConferencia' | 'Fechado' | 'Reaberto';
export type OrigemMovimentoBancoHoras = 'Acumulo' | 'Compensacao' | 'Pagamento' | 'Ajuste' | 'Expiracao';

export interface MarcacaoPonto {
  id: string;
  dataHora: string;
  tipo: TipoMarcacao;
  origem: OrigemMarcacao;
  status: StatusMarcacao;
  hashIntegridade: string;
}

export interface BaterPontoPayload {
  tipo?: TipoMarcacao | null;
  latitude?: number | null;
  longitude?: number | null;
  fotoUrl?: string | null;
}

export interface BaterPontoResponse {
  id: string;
  dataHora: string;
  tipo: TipoMarcacao;
  hashIntegridade: string;
}

export interface SolicitarAjustePayload {
  marcacaoOriginalId?: string | null;
  tipoAjuste: TipoAjuste;
  dataHoraProposta?: string | null;
  tipoMarcacaoProposta?: TipoMarcacao | null;
  motivo: string;
  anexoUrl?: string | null;
}

export interface AjustePendente {
  id: string;
  funcionarioId: string;
  marcacaoOriginalId?: string | null;
  tipoAjuste: TipoAjuste;
  dataHoraProposta?: string | null;
  motivo: string;
  solicitadoEm: string;
}

export interface EspelhoBatida { id: string; hora: string; tipo: string; origem: string; }
export interface EspelhoDia {
  data: string;
  diaSemana: string;
  ehFeriado: boolean;
  ehDiaUtil: boolean;
  janelaEsperadaEntrada?: string | null;
  janelaEsperadaSaida?: string | null;
  batidas: EspelhoBatida[];
  trabalhadoMinutos: number;
  esperadoMinutos: number;
  saldoMinutos: number;
  atrasoMinutos: number;
  anomalias: string[];
}
export interface EspelhoMensal {
  funcionarioId: string;
  funcionarioNome: string;
  funcionarioCpf: string;
  competencia: string;
  jornadaVigente: { nome: string; cargaSemanal: number; };
  politicaBancoHoras?: { nome: string; limiteAcumularMinutos: number; } | null;
  dias: EspelhoDia[];
  totais: {
    diasUteis: number; diasTrabalhados: number; diasFalta: number;
    trabalhadoMinutos: number; esperadoMinutos: number; saldoMesMinutos: number;
    horasExtrasMinutos: number; saldoBancoAcumuladoMinutos: number;
  };
  hashEspelho: string;
  geradoEm: string;
}

export interface SaldoBancoHoras {
  funcionarioId: string; competencia: string;
  horasDevidas: number; horasRealizadas: number;
  saldoMinutos: number; politicaId?: string | null;
}

export interface MovimentoBancoHoras {
  id: string;
  data: string;
  origem: OrigemMovimentoBancoHoras;
  minutos: number;
  observacao?: string | null;
}

export interface PoliticaBancoHoras {
  id?: string;
  nome: string;
  vigenciaInicio: string;
  vigenciaFim?: string | null;
  limiteHorasAcumular: number;
  prazoCompensacaoDias: number;
  permitePagarExcedente: boolean;
  fatorPagamento: number;
  ativo: boolean;
}

@Injectable({ providedIn: 'root' })
export class PontoService {
  private readonly http = inject(HttpClient);
  private get url(): string {
    return `${environment.apiUrl}/${environment.apiVersion}/rh/ponto`;
  }

  baterPonto(payload: BaterPontoPayload): Observable<BaterPontoResponse> {
    return this.http.post<BaterPontoResponse>(`${this.url}/bater`, payload);
  }

  listarProprio(dataInicio: string, dataFim: string): Observable<{ items: MarcacaoPonto[]; total: number; }> {
    return this.http.get<{ items: MarcacaoPonto[]; total: number; }>(
      `${this.url}/proprio?dataInicio=${dataInicio}&dataFim=${dataFim}`);
  }

  listarEquipe(funcionarioId: string, dataInicio: string, dataFim: string)
    : Observable<{ items: MarcacaoPonto[]; total: number; }> {
    return this.http.get<{ items: MarcacaoPonto[]; total: number; }>(
      `${this.url}/equipe/${funcionarioId}?dataInicio=${dataInicio}&dataFim=${dataFim}`);
  }

  incluirManual(payload: { funcionarioId: string; dataHora: string; tipo: TipoMarcacao; motivo: string; }) {
    return this.http.post(`${this.url}/manual`, payload);
  }

  solicitarAjuste(payload: SolicitarAjustePayload): Observable<{ id: string; }> {
    return this.http.post<{ id: string; }>(`${this.url}/ajustes`, payload);
  }

  listarAjustesPendentes(): Observable<{ items: AjustePendente[]; total: number; }> {
    return this.http.get<{ items: AjustePendente[]; total: number; }>(`${this.url}/ajustes/pendentes`);
  }

  aprovarAjuste(id: string, justificativa?: string) {
    return this.http.post(`${this.url}/ajustes/${id}/aprovar`, { id, justificativa });
  }

  rejeitarAjuste(id: string, justificativa: string) {
    return this.http.post(`${this.url}/ajustes/${id}/rejeitar`, { id, justificativa });
  }

  obterEspelho(funcionarioId: string, competencia: string): Observable<{ espelho: EspelhoMensal; }> {
    return this.http.get<{ espelho: EspelhoMensal; }>(
      `${this.url}/espelho?funcionarioId=${funcionarioId}&competencia=${competencia}`);
  }

  baixarEspelhoPdf(funcionarioId: string, competencia: string): Observable<Blob> {
    return this.http.get(
      `${this.url}/espelho.pdf?funcionarioId=${funcionarioId}&competencia=${competencia}`,
      { responseType: 'blob' });
  }

  fecharCompetencia(funcionarioId: string, competencia: string, observacoes?: string) {
    return this.http.post(`${this.url}/competencia/fechar`,
      { funcionarioId, competencia, observacoes });
  }

  reabrirCompetencia(funcionarioId: string, competencia: string, motivo: string) {
    return this.http.post(`${this.url}/competencia/reabrir`,
      { funcionarioId, competencia, motivo });
  }

  listarStatusFechamento(competencia: string)
    : Observable<{ items: Array<{ funcionarioId: string; status: StatusFechamentoPonto; fechadoEm?: string | null; }>; total: number; }> {
    return this.http.get<any>(`${this.url}/competencia/${competencia}/status`);
  }
}

@Injectable({ providedIn: 'root' })
export class BancoHorasService {
  private readonly http = inject(HttpClient);
  private get url(): string {
    return `${environment.apiUrl}/${environment.apiVersion}/rh/banco-horas`;
  }

  obterSaldo(funcionarioId: string, competencia: string): Observable<SaldoBancoHoras> {
    return this.http.get<SaldoBancoHoras>(
      `${this.url}/saldo?funcionarioId=${funcionarioId}&competencia=${competencia}`);
  }

  listarMovimentos(funcionarioId: string, competencia: string)
    : Observable<{ items: MovimentoBancoHoras[]; total: number; saldoMinutos: number; }> {
    return this.http.get<any>(
      `${this.url}/movimentos?funcionarioId=${funcionarioId}&competencia=${competencia}`);
  }

  compensar(funcionarioId: string, data: string, minutos: number, motivo: string) {
    return this.http.post(`${this.url}/compensar`, { funcionarioId, data, minutos, motivo });
  }

  pagarSaldo(funcionarioId: string, competencia: string, minutos: number) {
    return this.http.post(`${this.url}/pagar`, { funcionarioId, competencia, minutos });
  }

  listarPoliticas(): Observable<{ items: PoliticaBancoHoras[]; total: number; }> {
    return this.http.get<any>(`${this.url}/politicas`);
  }

  criarPolitica(payload: Omit<PoliticaBancoHoras, 'id' | 'ativo'>) {
    return this.http.post(`${this.url}/politicas`, payload);
  }
}
