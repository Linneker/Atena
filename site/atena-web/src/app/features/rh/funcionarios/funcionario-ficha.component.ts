import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { FuncionarioRhService } from '../rh.services';

type Aba = 'dados' | 'contrato' | 'salario' | 'beneficios' | 'dependentes' | 'escalas';

/**
 * Ficha completa do funcionário — agrega 5 áreas em abas. Cada aba mostra a leitura
 * dos dados e expõe ações específicas: registrar reajuste salarial, vincular benefício,
 * cadastrar dependente. Operações usam endpoints /rh/funcionarios/{id}/{recurso}.
 */
@Component({
  selector: 'app-funcionario-ficha',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    @if (carregando()) {
      <div>Carregando ficha…</div>
    } @else if (ficha()) {
      <h3>{{ ficha()!.ficha.dadosPessoais.nomeCompleto }}</h3>
      <p class="text-muted">CPF: {{ ficha()!.ficha.dadosPessoais.cpf }} ·
         Matrícula: {{ ficha()!.ficha.contrato.codigoMatricula ?? '—' }} ·
         Salário vigente: <strong>{{ ficha()!.ficha.salarioVigente | currency:'BRL' }}</strong></p>

      <ul class="nav nav-tabs mb-3">
        <li class="nav-item" *ngFor="let a of abas">
          <a class="nav-link" [class.active]="abaAtiva() === a.chave"
             (click)="abaAtiva.set(a.chave)" style="cursor:pointer">{{ a.titulo }}</a>
        </li>
      </ul>

      @if (abaAtiva() === 'dados') {
        <pre>{{ ficha()!.ficha.dadosPessoais | json }}</pre>
      }
      @if (abaAtiva() === 'contrato') {
        <pre>{{ ficha()!.ficha.contrato | json }}</pre>
      }
      @if (abaAtiva() === 'salario') {
        <h5>Histórico salarial</h5>
        <table class="table table-sm">
          <thead><tr><th>Vigência</th><th>Fim</th><th>Valor</th><th>Motivo</th></tr></thead>
          <tbody>
            <tr *ngFor="let h of ficha()!.ficha.historicoSalarial">
              <td>{{ h.vigenciaInicio }}</td><td>{{ h.vigenciaFim ?? '—' }}</td>
              <td>{{ h.valor | currency:'BRL' }}</td><td>{{ h.motivo }}</td>
            </tr>
          </tbody>
        </table>
        <div class="card p-3 mt-2">
          <h6>Registrar reajuste</h6>
          <div class="row g-2">
            <div class="col-md-3"><input type="number" step="0.01" class="form-control" placeholder="Novo valor" [(ngModel)]="reajuste.novoValor" /></div>
            <div class="col-md-3"><input type="date" class="form-control" [(ngModel)]="reajuste.vigenciaInicio" /></div>
            <div class="col-md-3"><select class="form-select" [(ngModel)]="reajuste.motivo">
              <option *ngFor="let m of motivos" [ngValue]="m">{{ m }}</option>
            </select></div>
            <div class="col-md-3"><button class="btn btn-primary" (click)="registrarReajuste()">Registrar</button></div>
          </div>
        </div>
      }
      @if (abaAtiva() === 'beneficios') {
        <h5>Benefícios vinculados</h5>
        <table class="table table-sm">
          <thead><tr><th>Catálogo</th><th>Valor</th><th>Vigência</th><th></th></tr></thead>
          <tbody>
            <tr *ngFor="let b of ficha()!.ficha.beneficios">
              <td>{{ b.beneficioCatalogoId }}</td>
              <td>{{ b.valor | currency:'BRL' }}</td>
              <td>{{ b.vigenciaInicio }} → {{ b.vigenciaFim ?? '∞' }}</td>
              <td><button class="btn btn-sm btn-outline-danger" (click)="removerBeneficio(b.id)">Remover</button></td>
            </tr>
          </tbody>
        </table>
      }
      @if (abaAtiva() === 'dependentes') {
        <h5>Dependentes</h5>
        <table class="table table-sm">
          <thead><tr><th>Nome</th><th>CPF</th><th>Tipo</th><th>IRRF</th><th></th></tr></thead>
          <tbody>
            <tr *ngFor="let d of ficha()!.ficha.dependentes">
              <td>{{ d.nomeCompleto }}</td><td>{{ d.cpf }}</td><td>{{ d.tipo }}</td>
              <td>{{ d.irrf ? 'Sim' : 'Não' }}</td>
              <td><button class="btn btn-sm btn-outline-danger" (click)="removerDependente(d.id)">Remover</button></td>
            </tr>
          </tbody>
        </table>
      }
      @if (abaAtiva() === 'escalas') {
        <pre>{{ ficha()!.ficha.escalas | json }}</pre>
      }
    } @else {
      <div class="alert alert-warning">Funcionário não encontrado.</div>
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FuncionarioFichaComponent {
  private readonly route = inject(ActivatedRoute);
  private readonly svc = inject(FuncionarioRhService);
  readonly ficha = signal<any | null>(null);
  readonly carregando = signal(true);
  readonly abaAtiva = signal<Aba>('dados');
  readonly abas: Array<{ chave: Aba; titulo: string }> = [
    { chave: 'dados', titulo: 'Dados' },
    { chave: 'contrato', titulo: 'Contrato' },
    { chave: 'salario', titulo: 'Histórico salarial' },
    { chave: 'beneficios', titulo: 'Benefícios' },
    { chave: 'dependentes', titulo: 'Dependentes' },
    { chave: 'escalas', titulo: 'Escalas' },
  ];
  readonly motivos = ['ReajusteAnual', 'Promocao', 'Dissidio', 'Correcao', 'Outro'];
  reajuste = { novoValor: 0, vigenciaInicio: new Date().toISOString().slice(0, 10), motivo: 'ReajusteAnual' as string };

  private get id(): string {
    return this.route.snapshot.paramMap.get('id') ?? '';
  }

  constructor() {
    this.carregar();
  }

  private carregar(): void {
    this.carregando.set(true);
    this.svc.obterFicha(this.id).subscribe({
      next: (r) => { this.ficha.set(r); this.carregando.set(false); },
      error: () => { this.ficha.set(null); this.carregando.set(false); },
    });
  }

  registrarReajuste(): void {
    this.svc.registrarReajuste(this.id, {
      funcionarioId: this.id,
      novoValor: this.reajuste.novoValor,
      vigenciaInicio: this.reajuste.vigenciaInicio,
      motivo: this.reajuste.motivo,
    }).subscribe(() => this.carregar());
  }

  removerBeneficio(vinculoId: string): void {
    if (!confirm('Remover este benefício?')) return;
    this.svc.removerBeneficio(this.id, vinculoId).subscribe(() => this.carregar());
  }

  removerDependente(depId: string): void {
    if (!confirm('Remover este dependente?')) return;
    this.svc.removerDependente(this.id, depId).subscribe(() => this.carregar());
  }
}
