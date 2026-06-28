import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import {
  AcaoLinha, ColunaTabela, DataTableComponent, OrdenacaoTabela, PaginaResultado,
} from '@shared/data-table/data-table.component';
import { ConfirmarDialogComponent } from '@shared/confirmation/confirmar-dialog.component';
import { InfoDialogComponent } from '@shared/confirmation/info-dialog.component';
import {
  Competencia, CompetenciaSelectorComponent, competenciaAtual,
} from '@shared/competencia/competencia-selector.component';
import { Despesa, DespesaService } from '../financeiro.services';

type DialogState =
  | { tipo: 'confirmar'; despesa: Despesa }
  | { tipo: 'info'; titulo: string; mensagem: string; estado: 'sucesso' | 'erro' };

@Component({
  selector: 'app-despesa-list',
  standalone: true,
  imports: [DataTableComponent, ConfirmarDialogComponent, InfoDialogComponent, CompetenciaSelectorComponent],
  template: `
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h3 class="m-0">Despesas</h3>
      <div class="d-flex gap-2">
        <button class="btn btn-outline-secondary btn-sm" (click)="gerarRecorrencias()" [disabled]="gerandoRecorrencias()">
          {{ gerandoRecorrencias() ? 'Gerando...' : 'Gerar recorrências (3 meses)' }}
        </button>
        <button class="btn btn-primary btn-sm" (click)="novo()">Novo</button>
      </div>
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

    @if (dialog(); as d) {
      @switch (d.tipo) {
        @case ('confirmar') {
          <app-confirmar-dialog
            titulo="Gerar Conta a Pagar"
            [mensagem]="'Criar uma Conta a Pagar vinculada à despesa &quot;' + d.despesa.nome + '&quot; no valor de R$ ' + d.despesa.valor.toFixed(2) + '?'"
            textoConfirmar="Gerar conta"
            cor="primario"
            (cancelar)="dialog.set(null)"
            (confirmar)="confirmarGeracao(d.despesa)" />
        }
        @case ('info') {
          <app-info-dialog [titulo]="d.titulo" [mensagem]="d.mensagem" [tipo]="d.estado" (fechar)="dialog.set(null)" />
        }
      }
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class DespesaListComponent implements OnInit {
  private readonly servico = inject(DespesaService);
  private readonly router = inject(Router);

  readonly pagina = signal<PaginaResultado<Despesa> | null>(null);
  readonly dialog = signal<DialogState | null>(null);
  readonly gerandoRecorrencias = signal(false);

  private busca = '';
  private numeroPagina = 1;
  private ordenacao: OrdenacaoTabela | null = null;
  private competencia: Competencia = competenciaAtual();

  readonly colunas: ColunaTabela<Despesa>[] = [
    { campo: 'nome', titulo: 'Nome' },
    { campo: 'despesaFixa', titulo: 'Tipo', formato: (l) => l.despesaFixa ? 'Fixa' : 'Variável' },
    { campo: 'valor', titulo: 'Valor', tipo: 'moeda' },
    { campo: 'dataVencimento', titulo: 'Vencimento', tipo: 'data' },
    { campo: 'centroDeCustoNome', titulo: 'Centro de Custo', formato: (l) => l.centroDeCustoNome ?? '—' },
    { campo: 'statusPagamento', titulo: 'Status' },
  ];

  readonly acoes: AcaoLinha<Despesa>[] = [
    {
      rotulo: 'Gerar conta',
      classe: 'btn-link text-primary',
      visivel: (l) => l.statusPagamento === 'Pendente',
      executar: (l) => this.dialog.set({ tipo: 'confirmar', despesa: l }),
    },
  ];

  ngOnInit(): void { /* competencia-selector dispara onCompetencia no init */ }
  onBusca(t: string): void { this.busca = t; this.numeroPagina = 1; this.recarregar(); }
  onPagina(n: number): void { this.numeroPagina = n; this.recarregar(); }
  onOrdenacao(o: OrdenacaoTabela): void { this.ordenacao = o; this.recarregar(); }
  onCompetencia(c: Competencia): void { this.competencia = c; this.numeroPagina = 1; this.recarregar(); }

  novo(): void { this.router.navigateByUrl('/financeiro/despesas/novo'); }
  editar(d: Despesa): void { this.router.navigateByUrl(`/financeiro/despesas/${d.id}`); }

  gerarRecorrencias(): void {
    this.gerandoRecorrencias.set(true);
    this.servico.gerarRecorrencias(3).subscribe({
      next: (r) => {
        this.gerandoRecorrencias.set(false);
        this.dialog.set({
          tipo: 'info',
          titulo: 'Recorrências geradas',
          mensagem: `${r.geradas} despesa(s) criada(s) nos próximos 3 meses. ${r.ignoradasJaExistentes} já existiam e foram mantidas.`,
          estado: 'sucesso',
        });
        this.recarregar();
      },
      error: (e) => {
        this.gerandoRecorrencias.set(false);
        this.dialog.set({
          tipo: 'info', titulo: 'Falha',
          mensagem: e?.error?.message ?? 'Não foi possível gerar as recorrências.',
          estado: 'erro',
        });
      },
    });
  }

  confirmarGeracao(d: Despesa): void {
    this.servico.gerarContaPagar(d).subscribe({
      next: () => {
        this.dialog.set({
          tipo: 'info',
          titulo: 'Conta a Pagar gerada',
          mensagem: 'A conta foi criada e vinculada à despesa. Acesse Financeiro → Contas a Pagar para baixá-la.',
          estado: 'sucesso',
        });
        this.recarregar();
      },
      error: (e) => this.dialog.set({
        tipo: 'info',
        titulo: 'Falha',
        mensagem: e?.error?.message ?? 'Não foi possível gerar a conta a pagar.',
        estado: 'erro',
      }),
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
