import { HttpClient } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { Observable } from 'rxjs';
import { environment } from '@env/environment';
import { CrudService } from '@shared/crud/crud.service';

// ====== Tipos compartilhados ======

export type TipoJornada = 'Fixa' | 'Flexivel' | 'Escala12x36' | 'EscalaPersonalizada' | 'Banco';
export type TipoContrato = 'Clt' | 'Estagio' | 'JovemAprendiz' | 'Terceirizado' | 'Pj' | 'Temporario';
export type RegimeRemuneracao = 'Mensalista' | 'Horista' | 'Diarista' | 'Comissionado';
export type EstadoCivil = 'Solteiro' | 'Casado' | 'Divorciado' | 'Viuvo' | 'Uniao' | 'Outro';
export type MotivoSalario = 'Admissao' | 'ReajusteAnual' | 'Promocao' | 'Dissidio' | 'Correcao' | 'Outro';
export type TipoDependente = 'Filho' | 'Conjuge' | 'Pais' | 'Outro';
export type TipoBeneficio = 'ValeTransporte' | 'ValeRefeicao' | 'ValeAlimentacao'
  | 'PlanoSaude' | 'PlanoOdonto' | 'AuxilioCreche' | 'SeguroVida'
  | 'AuxilioHomeOffice' | 'GymPass' | 'Outro';

export interface Jornada {
  id?: string;
  nome: string;
  tipo: TipoJornada;
  cargaSemanalHoras: number;
  cargaDiariaHoras?: number | null;
  janelasJson: string;
  permiteMarcarIntervalo: boolean;
  toleranciaMinutos: number;
  ativo: boolean;
}

export interface Cargo {
  id?: string;
  codigo?: string | null;
  descricao: string;
  codigoCbo?: string | null;
  salarioBaseSugerido?: number | null;
  ativo: boolean;
}

export interface Lotacao {
  id?: string;
  nome: string;
  empresaId?: string | null;
  cnpj?: string | null;
  enderecoJson?: string | null;
  ativo: boolean;
}

export interface Departamento {
  id?: string;
  codigo?: string | null;
  nome: string;
  centroDeCustoId?: string | null;
  ativo: boolean;
}

export interface BeneficioCatalogo {
  id?: string;
  codigo?: string | null;
  descricao: string;
  tipo: TipoBeneficio;
  descontoFuncionarioPct?: number | null;
  custoEmpresaPadrao?: number | null;
  naturezaRubricaEsocial?: string | null;
  ativo: boolean;
}

export interface Cbo {
  codigo: string;
  titulo: string;
  grandeGrupo?: string | null;
  familia?: string | null;
}

// ====== Funcionário ======

export interface EnderecoFuncionario {
  cep?: string | null;
  logradouro?: string | null;
  numero?: string | null;
  complemento?: string | null;
  bairro?: string | null;
  cidade?: string | null;
  uf?: string | null;
  pais?: string | null;
}

export interface ContaBancariaFuncionario {
  codigoBanco?: string | null;
  nomeBanco?: string | null;
  agencia?: string | null;
  agenciaDigito?: string | null;
  conta?: string | null;
  contaDigito?: string | null;
  tipoConta?: string | null;
  chavePix?: string | null;
}

export interface CriarFuncionarioCompletoPayload {
  nomeCompleto: string;
  cpf: string;
  email?: string | null;
  telefone?: string | null;
  dataNascimento?: string | null;
  estadoCivil?: EstadoCivil | null;
  naturalidade?: string | null;
  nacionalidade?: string | null;
  rg?: string | null;
  rgOrgao?: string | null;
  rgUf?: string | null;
  endereco?: EnderecoFuncionario | null;
  dataAdmissao: string;
  cargoId?: string | null;
  lotacaoId?: string | null;
  departamentoId?: string | null;
  centroDeCustoId?: string | null;
  tipoContrato: TipoContrato;
  regimeRemuneracao: RegimeRemuneracao;
  codigoMatricula?: string | null;
  pis?: string | null;
  ctps?: string | null;
  ctpsSerie?: string | null;
  ctpsUf?: string | null;
  salarioInicial: number;
  contaBancaria?: ContaBancariaFuncionario | null;
  jornadaId?: string | null;
  beneficios?: Array<{
    beneficioCatalogoId: string;
    valor?: number | null;
    descontoFuncionarioPct?: number | null;
    vigenciaInicio: string;
  }>;
  dependentes?: Array<{
    nomeCompleto: string;
    cpf?: string | null;
    dataNascimento: string;
    tipo: TipoDependente;
    irrf?: boolean;
    salarioFamilia?: boolean;
    pensaoAlimenticiaPct?: number | null;
  }>;
}

export interface FichaFuncionario {
  ficha: {
    dadosPessoais: any;
    contrato: any;
    salarioVigente: number | null;
    historicoSalarial: any[];
    beneficios: any[];
    dependentes: any[];
    escalas: any[];
  };
}

// ====== Services ======

@Injectable({ providedIn: 'root' })
export class JornadaService extends CrudService<Jornada> {
  protected readonly resource = 'rh/jornadas';
}

@Injectable({ providedIn: 'root' })
export class CargoService extends CrudService<Cargo> {
  protected readonly resource = 'rh/cargos';
}

@Injectable({ providedIn: 'root' })
export class LotacaoService extends CrudService<Lotacao> {
  protected readonly resource = 'rh/lotacoes';
}

@Injectable({ providedIn: 'root' })
export class DepartamentoService extends CrudService<Departamento> {
  protected readonly resource = 'rh/departamentos';
}

@Injectable({ providedIn: 'root' })
export class BeneficioCatalogoService extends CrudService<BeneficioCatalogo> {
  protected readonly resource = 'rh/beneficios/catalogo';
}

@Injectable({ providedIn: 'root' })
export class CboService {
  private readonly http = inject(HttpClient);
  private get url(): string {
    return `${environment.apiUrl}/${environment.apiVersion}/rh/cbos`;
  }

  listar(): Observable<{ items: Cbo[]; total: number }> {
    return this.http.get<{ items: Cbo[]; total: number }>(this.url);
  }
}

@Injectable({ providedIn: 'root' })
export class FuncionarioRhService {
  private readonly http = inject(HttpClient);
  private get url(): string {
    return `${environment.apiUrl}/${environment.apiVersion}/rh/funcionarios`;
  }

  criarCompleto(payload: CriarFuncionarioCompletoPayload): Observable<{
    funcionarioId: string; historicoSalarioId: string;
    escalaId?: string | null; beneficiosCriados: number; dependentesCriados: number;
  }> {
    return this.http.post<any>(this.url, payload);
  }

  alterarDados(id: string, payload: any): Observable<{ id: string }> {
    return this.http.put<any>(`${this.url}/${id}/dados`, payload);
  }

  alterarContrato(id: string, payload: any): Observable<{ id: string }> {
    return this.http.put<any>(`${this.url}/${id}/contrato`, payload);
  }

  registrarReajuste(id: string, payload: any): Observable<any> {
    return this.http.post<any>(`${this.url}/${id}/salarios`, payload);
  }

  vincularBeneficio(id: string, payload: any): Observable<any> {
    return this.http.post<any>(`${this.url}/${id}/beneficios`, payload);
  }

  removerBeneficio(id: string, vinculoId: string): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}/beneficios/${vinculoId}`);
  }

  cadastrarDependente(id: string, payload: any): Observable<any> {
    return this.http.post<any>(`${this.url}/${id}/dependentes`, payload);
  }

  removerDependente(id: string, depId: string): Observable<void> {
    return this.http.delete<void>(`${this.url}/${id}/dependentes/${depId}`);
  }

  atribuirEscala(id: string, payload: any): Observable<any> {
    return this.http.post<any>(`${this.url}/${id}/escalas`, payload);
  }

  obterFicha(id: string): Observable<FichaFuncionario> {
    return this.http.get<FichaFuncionario>(`${this.url}/${id}/ficha`);
  }
}
