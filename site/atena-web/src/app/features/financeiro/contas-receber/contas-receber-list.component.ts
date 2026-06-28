import { ChangeDetectionStrategy, Component, OnInit, ViewChild, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import {
  AcaoLinha, ColunaTabela, DataTableComponent, OrdenacaoTabela, PaginaResultado,
} from '@shared/data-table/data-table.component';
import {
  Competencia, CompetenciaSelectorComponent, competenciaAtual,
} from '@shared/competencia/competencia-selector.component';
import { ContaReceber, ContaReceberService, ReceberContaReceberPayload } from '../financeiro.services';
import { ReceberContaDialogComponent } from './receber-conta-dialog.component';

@Component({
  selector: 'app-contas-receber-list',
  standalone: true,
  imports: [DataTableComponent, ReceberContaDialogComponent, CompetenciaSelectorComponent],
  template: `
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h3 class="m-0">Contas a Receber</h3>
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
      <app-receber-conta-dialog
        #dialog
        [conta]="c"
        (fechar)="contaSelecionada.set(null)"
        (confirmar)="confirmarRecebimento(c, $event)" />
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ContasReceberListComponent implements OnInit {
  private readonly servico = inject(ContaReceberService);
  private readonly router = inject(Router);

  @ViewChild('dialog') dialog?: ReceberContaDialogComponent;

  readonly pagina = signal<PaginaResultado<ContaReceber> | null>(null);
  readonly contaSelecionada = signal<ContaReceber | null>(null);

  private busca = '';
  private numeroPagina = 1;
  private ordenacao: OrdenacaoTabela | null = null;
  private competencia: Competencia = competenciaAtual();

  readonly colunas: ColunaTabela<ContaReceber>[] = [
    { campo: 'descricao', titulo: 'Descrição' },
    { campo: 'clienteNome', titulo: 'Cliente', formato: (l) => l.clienteNome ?? '—' },
    { campo: 'valorOriginal', titulo: 'Valor', tipo: 'moeda' },
    { campo: 'saldo', titulo: 'Saldo', tipo: 'moeda' },
    { campo: 'dataVencimento', titulo: 'Vencimento', tipo: 'data' },
    { campo: 'status', titulo: 'Status' },
  ];

  readonly acoes: AcaoLinha<ContaReceber>[] = [
    {
      rotulo: 'Receber',
      classe: 'btn-link text-success',
      visivel: (l) => (l.saldo ?? l.valorOriginal) > 0 && l.status !== 'Recebida' && l.status !== 'Cancelada',
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
    this.router.navigateByUrl('/financeiro/contas-receber/novo');
  }

  editar(c: ContaReceber): void {
    this.router.navigateByUrl(`/financeiro/contas-receber/${c.id}`);
  }

  confirmarRecebimento(conta: ContaReceber, payload: ReceberContaReceberPayload): void {
    if (!conta.id) return;
    this.servico.receber(conta.id, payload).subscribe({
      next: () => {
        this.contaSelecionada.set(null);
        this.recarregar();
      },
      error: (e) => this.dialog?.mostrarErro(e?.error?.message ?? 'Falha ao registrar recebimento.'),
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
