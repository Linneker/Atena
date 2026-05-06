import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';

export interface DashboardKpis {
  receita: number; despesa: number; resultado: number;
  vendasAbertas: number; vencimentos: number; estoqueCritico: number;
}

export interface EvolucaoMes { mes: string; receita: number; despesa: number; }
export interface LinhaDre { conta: string; valor: number; }
export interface AgingFaixa { faixa: string; valor: number; quantidade: number; }
export interface VendaRelatorio { vendedor: string; cliente: string; produto: string; valor: number; data: string; }
export interface PosicaoEstoque { produto: string; saldo: number; valor: number; }

@Injectable({ providedIn: 'root' })
export class RelatoriosService {
  private readonly http = inject(HttpClient);

  private url(path: string): string { return `${environment.apiUrl}/${environment.apiVersion}/${path}`; }

  kpis(): Observable<DashboardKpis> { return this.http.get<DashboardKpis>(this.url('dashboard/kpis')); }
  evolucao(): Observable<EvolucaoMes[]> { return this.http.get<EvolucaoMes[]>(this.url('dashboard/evolucao')); }
  dre(competencia: string): Observable<LinhaDre[]> {
    return this.http.get<LinhaDre[]>(this.url('relatorios/dre'), { params: new HttpParams().set('competencia', competencia) });
  }
  balanco(competencia: string): Observable<LinhaDre[]> {
    return this.http.get<LinhaDre[]>(this.url('relatorios/balanco'), { params: new HttpParams().set('competencia', competencia) });
  }
  agingPagar(): Observable<AgingFaixa[]> { return this.http.get<AgingFaixa[]>(this.url('relatorios/contas-pagar/aging')); }
  agingReceber(): Observable<AgingFaixa[]> { return this.http.get<AgingFaixa[]>(this.url('relatorios/contas-receber/aging')); }
  vendas(inicio: string, fim: string): Observable<VendaRelatorio[]> {
    return this.http.get<VendaRelatorio[]>(this.url('relatorios/vendas'), { params: { inicio, fim } });
  }
  posicaoEstoque(): Observable<PosicaoEstoque[]> { return this.http.get<PosicaoEstoque[]>(this.url('relatorios/estoque/posicao')); }
}
