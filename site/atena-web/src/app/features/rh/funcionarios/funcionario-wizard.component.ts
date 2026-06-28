import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { CargoService, CriarFuncionarioCompletoPayload, DepartamentoService, FuncionarioRhService,
         JornadaService, LotacaoService } from '../rh.services';

interface Passo {
  chave: 'pessoal' | 'contrato' | 'salario' | 'beneficios';
  titulo: string;
}

/**
 * Wizard de criação de funcionário completo (4 passos). Mantém estado local em signals,
 * dispara um único POST `/rh/funcionarios` ao final que cria atomicamente: funcionário +
 * histórico salarial inicial + (opcional) escala + benefícios + dependentes.
 */
@Component({
  selector: 'app-funcionario-wizard',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    <h3>Novo funcionário</h3>
    <ul class="nav nav-pills mb-3">
      <li class="nav-item" *ngFor="let p of passos; let i = index">
        <a class="nav-link"
           [class.active]="passoAtivo() === p.chave"
           [class.text-muted]="passoAtivo() !== p.chave"
           (click)="ir(p.chave)" style="cursor:pointer">
          {{ i + 1 }}. {{ p.titulo }}
        </a>
      </li>
    </ul>

    @if (passoAtivo() === 'pessoal') {
      <div class="card p-3">
        <h5>Dados pessoais</h5>
        <div class="row g-2">
          <div class="col-md-6"><label>Nome completo *</label>
            <input class="form-control" [(ngModel)]="payload.nomeCompleto" /></div>
          <div class="col-md-3"><label>CPF *</label>
            <input class="form-control" [(ngModel)]="payload.cpf" maxlength="11" /></div>
          <div class="col-md-3"><label>Data nascimento</label>
            <input type="date" class="form-control" [(ngModel)]="payload.dataNascimento" /></div>
          <div class="col-md-6"><label>E-mail</label>
            <input type="email" class="form-control" [(ngModel)]="payload.email" /></div>
          <div class="col-md-3"><label>Telefone</label>
            <input class="form-control" [(ngModel)]="payload.telefone" /></div>
          <div class="col-md-3"><label>Estado civil</label>
            <select class="form-select" [(ngModel)]="payload.estadoCivil">
              <option [ngValue]="null">—</option>
              <option *ngFor="let e of estadosCivis" [ngValue]="e">{{ e }}</option>
            </select></div>
          <div class="col-md-4"><label>RG</label>
            <input class="form-control" [(ngModel)]="payload.rg" /></div>
          <div class="col-md-4"><label>Órgão emissor</label>
            <input class="form-control" [(ngModel)]="payload.rgOrgao" /></div>
          <div class="col-md-4"><label>UF RG</label>
            <input class="form-control" [(ngModel)]="payload.rgUf" maxlength="2" /></div>
        </div>
        <div class="mt-3 d-flex justify-content-end">
          <button class="btn btn-primary" (click)="ir('contrato')">Próximo</button>
        </div>
      </div>
    }

    @if (passoAtivo() === 'contrato') {
      <div class="card p-3">
        <h5>Contrato</h5>
        <div class="row g-2">
          <div class="col-md-3"><label>Data admissão *</label>
            <input type="date" class="form-control" [(ngModel)]="payload.dataAdmissao" /></div>
          <div class="col-md-3"><label>Tipo de contrato *</label>
            <select class="form-select" [(ngModel)]="payload.tipoContrato">
              <option *ngFor="let t of tiposContrato" [ngValue]="t">{{ t }}</option>
            </select></div>
          <div class="col-md-3"><label>Regime *</label>
            <select class="form-select" [(ngModel)]="payload.regimeRemuneracao">
              <option *ngFor="let r of regimes" [ngValue]="r">{{ r }}</option>
            </select></div>
          <div class="col-md-3"><label>Matrícula</label>
            <input class="form-control" [(ngModel)]="payload.codigoMatricula" /></div>
          <div class="col-md-6"><label>Cargo</label>
            <select class="form-select" [(ngModel)]="payload.cargoId">
              <option [ngValue]="null">—</option>
              <option *ngFor="let c of cargos()" [ngValue]="c.id">{{ c.descricao }}</option>
            </select></div>
          <div class="col-md-6"><label>Departamento</label>
            <select class="form-select" [(ngModel)]="payload.departamentoId">
              <option [ngValue]="null">—</option>
              <option *ngFor="let d of departamentos()" [ngValue]="d.id">{{ d.nome }}</option>
            </select></div>
          <div class="col-md-6"><label>Lotação</label>
            <select class="form-select" [(ngModel)]="payload.lotacaoId">
              <option [ngValue]="null">—</option>
              <option *ngFor="let l of lotacoes()" [ngValue]="l.id">{{ l.nome }}</option>
            </select></div>
          <div class="col-md-6"><label>Jornada (escala)</label>
            <select class="form-select" [(ngModel)]="payload.jornadaId">
              <option [ngValue]="null">— sem escala —</option>
              <option *ngFor="let j of jornadas()" [ngValue]="j.id">{{ j.nome }}</option>
            </select></div>
        </div>
        <div class="mt-3 d-flex justify-content-between">
          <button class="btn btn-outline-secondary" (click)="ir('pessoal')">Voltar</button>
          <button class="btn btn-primary" (click)="ir('salario')">Próximo</button>
        </div>
      </div>
    }

    @if (passoAtivo() === 'salario') {
      <div class="card p-3">
        <h5>Salário inicial</h5>
        <div class="row g-2">
          <div class="col-md-4"><label>Salário inicial *</label>
            <input type="number" step="0.01" class="form-control" [(ngModel)]="payload.salarioInicial" /></div>
          <div class="col-md-4"><label>PIS</label>
            <input class="form-control" [(ngModel)]="payload.pis" maxlength="11" /></div>
          <div class="col-md-4"><label>CTPS</label>
            <input class="form-control" [(ngModel)]="payload.ctps" /></div>
          <div class="col-md-3"><label>Série CTPS</label>
            <input class="form-control" [(ngModel)]="payload.ctpsSerie" /></div>
          <div class="col-md-3"><label>UF CTPS</label>
            <input class="form-control" [(ngModel)]="payload.ctpsUf" maxlength="2" /></div>
        </div>
        <div class="mt-3 d-flex justify-content-between">
          <button class="btn btn-outline-secondary" (click)="ir('contrato')">Voltar</button>
          <button class="btn btn-primary" (click)="ir('beneficios')">Próximo</button>
        </div>
      </div>
    }

    @if (passoAtivo() === 'beneficios') {
      <div class="card p-3">
        <h5>Benefícios + Conta bancária (opcional)</h5>
        <p class="text-muted">Os benefícios podem ser adicionados depois pela ficha do funcionário.</p>
        <div class="row g-2">
          <div class="col-md-3"><label>Banco (código)</label>
            <input class="form-control" [(ngModel)]="conta.codigoBanco" maxlength="3" /></div>
          <div class="col-md-3"><label>Agência</label>
            <input class="form-control" [(ngModel)]="conta.agencia" /></div>
          <div class="col-md-1"><label>DV</label>
            <input class="form-control" [(ngModel)]="conta.agenciaDigito" maxlength="1" /></div>
          <div class="col-md-3"><label>Conta</label>
            <input class="form-control" [(ngModel)]="conta.conta" /></div>
          <div class="col-md-1"><label>DV</label>
            <input class="form-control" [(ngModel)]="conta.contaDigito" maxlength="1" /></div>
          <div class="col-md-1"><label>Tipo</label>
            <select class="form-select" [(ngModel)]="conta.tipoConta">
              <option [ngValue]="null">—</option><option value="CC">CC</option><option value="CP">CP</option>
            </select></div>
        </div>
        <div class="mt-3 d-flex justify-content-between">
          <button class="btn btn-outline-secondary" (click)="ir('salario')">Voltar</button>
          <button class="btn btn-success" (click)="salvar()" [disabled]="salvando()">
            {{ salvando() ? 'Salvando...' : 'Criar funcionário' }}
          </button>
        </div>
        @if (erro()) { <div class="alert alert-danger mt-2">{{ erro() }}</div> }
      </div>
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FuncionarioWizardComponent {
  private readonly router = inject(Router);
  private readonly funcSvc = inject(FuncionarioRhService);

  readonly passos: Passo[] = [
    { chave: 'pessoal', titulo: 'Pessoal' },
    { chave: 'contrato', titulo: 'Contrato' },
    { chave: 'salario', titulo: 'Salário' },
    { chave: 'beneficios', titulo: 'Benefícios + Banco' },
  ];

  readonly estadosCivis = ['Solteiro', 'Casado', 'Divorciado', 'Viuvo', 'Uniao', 'Outro'];
  readonly tiposContrato = ['Clt', 'Estagio', 'JovemAprendiz', 'Terceirizado', 'Pj', 'Temporario'];
  readonly regimes = ['Mensalista', 'Horista', 'Diarista', 'Comissionado'];

  readonly cargos = signal<any[]>([]);
  readonly departamentos = signal<any[]>([]);
  readonly lotacoes = signal<any[]>([]);
  readonly jornadas = signal<any[]>([]);

  readonly passoAtivo = signal<Passo['chave']>('pessoal');
  readonly salvando = signal(false);
  readonly erro = signal<string | null>(null);

  payload: CriarFuncionarioCompletoPayload = {
    nomeCompleto: '',
    cpf: '',
    dataAdmissao: new Date().toISOString().slice(0, 10),
    tipoContrato: 'Clt',
    regimeRemuneracao: 'Mensalista',
    salarioInicial: 0,
  };
  conta = {
    codigoBanco: null as string | null, agencia: null as string | null,
    agenciaDigito: null as string | null, conta: null as string | null,
    contaDigito: null as string | null, tipoConta: null as string | null,
  };

  constructor(
    cargoSvc: CargoService,
    deptoSvc: DepartamentoService,
    lotacaoSvc: LotacaoService,
    jornadaSvc: JornadaService,
  ) {
    cargoSvc.listar({ pagina: 1, tamanhoPagina: 200 }).subscribe((p) => this.cargos.set(p.itens));
    deptoSvc.listar({ pagina: 1, tamanhoPagina: 200 }).subscribe((p) => this.departamentos.set(p.itens));
    lotacaoSvc.listar({ pagina: 1, tamanhoPagina: 200 }).subscribe((p) => this.lotacoes.set(p.itens));
    jornadaSvc.listar({ pagina: 1, tamanhoPagina: 200 }).subscribe((p) => this.jornadas.set(p.itens));
  }

  ir(p: Passo['chave']): void { this.passoAtivo.set(p); }

  salvar(): void {
    if (this.conta.codigoBanco || this.conta.agencia || this.conta.conta) {
      this.payload.contaBancaria = { ...this.conta };
    }
    this.salvando.set(true);
    this.erro.set(null);
    this.funcSvc.criarCompleto(this.payload).subscribe({
      next: (r) => {
        this.salvando.set(false);
        this.router.navigate(['/rh/funcionarios', r.funcionarioId]);
      },
      error: (err) => {
        this.salvando.set(false);
        this.erro.set(err?.error?.message ?? 'Falha ao criar funcionário.');
      },
    });
  }
}
