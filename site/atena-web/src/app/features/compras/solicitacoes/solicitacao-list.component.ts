import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import {
  AcaoLinha, ColunaTabela, DataTableComponent, OrdenacaoTabela, PaginaResultado,
} from '@shared/data-table/data-table.component';
import { ConfirmarDialogComponent } from '@shared/confirmation/confirmar-dialog.component';
import { InfoDialogComponent } from '@shared/confirmation/info-dialog.component';
import { SolicitacaoCompra, SolicitacaoCompraService } from '../compras.services';

type DialogState =
  | { tipo: 'enviarAprovacao'; sol: SolicitacaoCompra }
  | { tipo: 'aprovar'; sol: SolicitacaoCompra }
  | { tipo: 'info'; titulo: string; mensagem: string; estado: 'sucesso' | 'erro' };

@Component({
  selector: 'app-solicitacao-list',
  standalone: true,
  imports: [DataTableComponent, ConfirmarDialogComponent, InfoDialogComponent],
  template: `
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h3 class="m-0">Solicitações de Compra</h3>
      <button class="btn btn-primary btn-sm" (click)="novo()">Nova</button>
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
        @case ('enviarAprovacao') {
          <app-confirmar-dialog
            titulo="Enviar para Aprovação"
            [mensagem]="'Enviar a solicitação ' + d.sol.numero + ' para aprovação?'"
            textoConfirmar="Enviar"
            cor="primario"
            (cancelar)="dialog.set(null)"
            (confirmar)="executarEnviar(d.sol)" />
        }
        @case ('aprovar') {
          <app-confirmar-dialog
            titulo="Aprovar Solicitação"
            [mensagem]="'Aprovar a solicitação ' + d.sol.numero + ' no valor de R$ ' + d.sol.valorTotal.toFixed(2) + '?\\n\\nApós aprovada poderá virar um Pedido de Compra.'"
            textoConfirmar="Aprovar"
            cor="sucesso"
            (cancelar)="dialog.set(null)"
            (confirmar)="executarAprovar(d.sol)" />
        }
        @case ('info') {
          <app-info-dialog [titulo]="d.titulo" [mensagem]="d.mensagem" [tipo]="d.estado" (fechar)="dialog.set(null)" />
        }
      }
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SolicitacaoListComponent implements OnInit {
  private readonly servico = inject(SolicitacaoCompraService);
  private readonly router = inject(Router);

  readonly pagina = signal<PaginaResultado<SolicitacaoCompra> | null>(null);
  readonly dialog = signal<DialogState | null>(null);

  private busca = '';
  private numeroPagina = 1;
  private ordenacao: OrdenacaoTabela | null = null;

  readonly colunas: ColunaTabela<SolicitacaoCompra>[] = [
    { campo: 'numero', titulo: 'Número' },
    { campo: 'dataSolicitacao', titulo: 'Solicitada em', tipo: 'data' },
    { campo: 'valorTotal', titulo: 'Total', tipo: 'moeda' },
    { campo: 'status', titulo: 'Status' },
  ];

  readonly acoes: AcaoLinha<SolicitacaoCompra>[] = [
    {
      rotulo: 'Enviar aprovação',
      classe: 'btn-link text-primary',
      visivel: (l) => l.status === 'Rascunho',
      executar: (l) => this.dialog.set({ tipo: 'enviarAprovacao', sol: l }),
    },
    {
      rotulo: 'Aprovar',
      classe: 'btn-link text-success',
      visivel: (l) => l.status === 'EmAprovacao' || l.status === 'AguardandoAprovacao',
      executar: (l) => this.dialog.set({ tipo: 'aprovar', sol: l }),
    },
  ];

  ngOnInit(): void { this.recarregar(); }
  onBusca(t: string): void { this.busca = t; this.numeroPagina = 1; this.recarregar(); }
  onPagina(n: number): void { this.numeroPagina = n; this.recarregar(); }
  onOrdenacao(o: OrdenacaoTabela): void { this.ordenacao = o; this.recarregar(); }

  novo(): void { this.router.navigateByUrl('/compras/solicitacoes/novo'); }
  editar(s: SolicitacaoCompra): void { this.router.navigateByUrl(`/compras/solicitacoes/${s.id}`); }

  executarEnviar(s: SolicitacaoCompra): void {
    if (!s.id) return;
    this.servico.enviarParaAprovacao(s.id).subscribe({
      next: () => {
        this.dialog.set({
          tipo: 'info',
          titulo: 'Solicitação enviada',
          mensagem: `${s.numero} foi enviada para aprovação.`,
          estado: 'sucesso',
        });
        this.recarregar();
      },
      error: (e) => this.dialog.set({
        tipo: 'info', titulo: 'Falha',
        mensagem: e?.error?.message ?? 'Não foi possível enviar para aprovação.',
        estado: 'erro',
      }),
    });
  }

  executarAprovar(s: SolicitacaoCompra): void {
    if (!s.id) return;
    this.servico.aprovar(s.id).subscribe({
      next: () => {
        this.dialog.set({
          tipo: 'info',
          titulo: 'Solicitação aprovada',
          mensagem: `${s.numero} aprovada. Já pode virar Pedido de Compra em Compras → Pedidos.`,
          estado: 'sucesso',
        });
        this.recarregar();
      },
      error: (e) => this.dialog.set({
        tipo: 'info', titulo: 'Falha',
        mensagem: e?.error?.message ?? 'Não foi possível aprovar.',
        estado: 'erro',
      }),
    });
  }

  private recarregar(): void {
    this.servico
      .listar({ pagina: this.numeroPagina, tamanhoPagina: 20, busca: this.busca, ordenacao: this.ordenacao })
      .subscribe((p) => this.pagina.set(p));
  }
}
