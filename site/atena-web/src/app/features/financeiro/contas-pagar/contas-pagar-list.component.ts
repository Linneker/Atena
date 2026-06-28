import { ChangeDetectionStrategy, Component, OnInit, ViewChild, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import {
  AcaoLinha, ColunaTabela, DataTableComponent, OrdenacaoTabela, PaginaResultado,
} from '@shared/data-table/data-table.component';
import {
  Competencia, CompetenciaSelectorComponent, competenciaAtual,
} from '@shared/competencia/competencia-selector.component';
import { BaixarContaPagarPayload, ContaPagar, ContaPagarService } from '../financeiro.services';
import { BaixarContaDialogComponent } from './baixar-conta-dialog.component';

@Component({
  selector: 'app-contas-pagar-list',
  standalone: true,
  imports: [DataTableComponent, BaixarContaDialogComponent, CompetenciaSelectorComponent],
  template: `
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h3 class="m-0">Contas a Pagar</h3>
      <button class="btn btn-primary btn-sm" (click)="novo()">Novo</button>
    </div>
    <div class="mb-3">
      <app-competencia-selector (mudou)="onCompetencia($event)" />
    </div>
    <app-data-table
      [colunas]="colunas"
      [pagina]="pagina()"
      [acoes]="acoes"
      (paginaChange)="onPagina($event)"
      (buscaChange)="onBusca($event)"
      (ordenacaoChange)="onOrdenacao($event)"
      (editar)="editar($event)" />

    @if (contaSelecionada(); as c) {
      <app-baixar-conta-dialog
        #dialog
        [conta]="c"
        (fechar)="contaSelecionada.set(null)"
        (confirmar)="confirmarBaixa(c, $event)" />
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ContasPagarListComponent implements OnInit {
  private readonly servico = inject(ContaPagarService);
  private readonly router = inject(Router);

  @ViewChild('dialog') dialog?: BaixarContaDialogComponent;

  readonly pagina = signal<PaginaResultado<ContaPagar> | null>(null);
  readonly contaSelecionada = signal<ContaPagar | null>(null);

  private busca = '';
  private numeroPagina = 1;
  private ordenacao: OrdenacaoTabela | null = null;
  private competencia: Competencia = competenciaAtual();

  readonly colunas: ColunaTabela<ContaPagar>[] = [
    { campo: 'descricao', titulo: 'Descrição' },
    { campo: 'fornecedorNome', titulo: 'Fornecedor', formato: (l) => l.fornecedorNome ?? '—' },
    { campo: 'valorOriginal', titulo: 'Valor', tipo: 'moeda' },
    { campo: 'saldo', titulo: 'Saldo', tipo: 'moeda' },
    { campo: 'dataVencimento', titulo: 'Vencimento', tipo: 'data' },
    { campo: 'status', titulo: 'Status' },
  ];

  readonly acoes: AcaoLinha<ContaPagar>[] = [
    {
      rotulo: 'Baixar',
      classe: 'btn-link text-success',
      visivel: (l) => (l.saldo ?? l.valorOriginal) > 0 && l.status !== 'Paga' && l.status !== 'Cancelada',
      executar: (l) => this.contaSelecionada.set(l),
    },
  ];

  ngOnInit(): void { /* competencia-selector dispara no init */ }

  onCompetencia(c: Competencia): void { this.competencia = c; this.numeroPagina = 1; this.recarregar(); }

  onBusca(termo: string): void {
    this.busca = termo;
    this.numeroPagina = 1;
    this.recarregar();
  }

  onPagina(num: number): void {
    this.numeroPagina = num;
    this.recarregar();
  }

  onOrdenacao(o: OrdenacaoTabela): void {
    this.ordenacao = o;
    this.recarregar();
  }

  novo(): void {
    this.router.navigateByUrl('/financeiro/contas-pagar/novo');
  }

  editar(c: ContaPagar): void {
    this.router.navigateByUrl(`/financeiro/contas-pagar/${c.id}`);
  }

  confirmarBaixa(conta: ContaPagar, payload: BaixarContaPagarPayload): void {
    if (!conta.id) return;
    this.servico.baixar(conta.id, payload).subscribe({
      next: () => {
        this.contaSelecionada.set(null);
        this.recarregar();
      },
      error: (e) => this.dialog?.mostrarErro(e?.error?.message ?? 'Falha ao dar baixa.'),
    });
  }

  private recarregar(): void {
    this.servico
      .listar({
        pagina: this.numeroPagina,
        tamanhoPagina: 20,
        busca: this.busca,
        ordenacao: this.ordenacao,
        filtros: {
          vencimentoInicio: this.competencia.inicio,
          vencimentoFim: this.competencia.fim,
        },
      })
      .subscribe((p) => this.pagina.set(p));
  }
}
