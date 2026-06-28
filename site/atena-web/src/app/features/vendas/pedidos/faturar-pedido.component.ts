import { ChangeDetectionStrategy, Component, OnInit, computed, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import {
  FaturamentoService,
  FaturarPedidoItem,
  PedidoVendaDetalhe,
  PedidoVendaItemDetalhe,
  PedidoVendaService,
} from '../vendas.services';

interface LinhaSelecionavel {
  item: PedidoVendaItemDetalhe;
  selecionar: boolean;
  quantidade: number;
}

@Component({
  selector: 'app-faturar-pedido',
  standalone: true,
  imports: [CommonModule, FormsModule],
  template: `
    @if (pedido(); as p) {
      <h3>Faturar Pedido {{ p.numero }}</h3>
      <p class="text-muted">
        Cliente: <strong>{{ p.clienteNome ?? '(sem cliente)' }}</strong> ·
        Emitido em: {{ p.dataEmissao | date:'dd/MM/yyyy' }} ·
        Status: <span class="badge bg-info">{{ p.status }}</span>
      </p>

      @if (p.status !== 'Confirmado' && p.status !== 'FaturamentoParcial') {
        <div class="alert alert-warning">
          Pedido com status <strong>{{ p.status }}</strong> não pode ser faturado.
          Apenas <em>Confirmado</em> ou <em>FaturamentoParcial</em> permitem faturamento.
        </div>
      }

      <h5 class="mt-4">Itens do pedido</h5>
      <table class="table table-sm">
        <thead class="table-light">
          <tr>
            <th><input type="checkbox" [(ngModel)]="todosSelecionados" (ngModelChange)="alternarTodos($event)" /></th>
            <th>Produto</th>
            <th class="text-end">Qtd pedida</th>
            <th class="text-end">Já faturada</th>
            <th class="text-end">Pendente</th>
            <th class="text-end">Preço</th>
            <th class="text-end">Qtd a faturar</th>
            <th class="text-end">Subtotal</th>
          </tr>
        </thead>
        <tbody>
          @for (l of linhas(); track l.item.id) {
            <tr [class.table-secondary]="l.item.quantidadePendente <= 0">
              <td>
                <input type="checkbox" [(ngModel)]="l.selecionar"
                       [disabled]="l.item.quantidadePendente <= 0"
                       (ngModelChange)="recalcular()" />
              </td>
              <td>{{ l.item.produtoNome ?? l.item.produtoId }}</td>
              <td class="text-end">{{ l.item.quantidade }}</td>
              <td class="text-end">{{ l.item.quantidadeFaturada }}</td>
              <td class="text-end">{{ l.item.quantidadePendente }}</td>
              <td class="text-end">{{ l.item.precoUnitario.toFixed(2) }}</td>
              <td class="text-end" style="width:120px">
                <input type="number" class="form-control form-control-sm text-end"
                       [(ngModel)]="l.quantidade"
                       [disabled]="!l.selecionar || l.item.quantidadePendente <= 0"
                       [max]="l.item.quantidadePendente"
                       min="0" step="0.001"
                       (ngModelChange)="recalcular()" />
              </td>
              <td class="text-end">
                {{ (l.selecionar ? l.quantidade * l.item.precoUnitario : 0).toFixed(2) }}
              </td>
            </tr>
          }
        </tbody>
        <tfoot>
          <tr>
            <th colspan="7" class="text-end">Total do faturamento:</th>
            <th class="text-end">R$ {{ totalFaturamento().toFixed(2) }}</th>
          </tr>
        </tfoot>
      </table>

      <div class="row g-2 mt-3 align-items-end">
        <div class="col-md-3">
          <label class="form-label small">Vencimento da Conta a Receber *</label>
          <input type="date" class="form-control form-control-sm" [(ngModel)]="vencimento" />
        </div>
        <div class="col-md-3">
          <label class="form-label small">% Comissão (opcional)</label>
          <input type="number" class="form-control form-control-sm" [(ngModel)]="percentualComissao" step="0.01" />
        </div>
      </div>

      @if (erro()) {
        <div class="alert alert-danger mt-3">{{ erro() }}</div>
      }

      <div class="mt-4">
        <button class="btn btn-primary" [disabled]="!podeFaturar() || salvando()" (click)="faturar()">
          {{ salvando() ? 'Faturando...' : 'Confirmar faturamento' }}
        </button>
        <button class="btn btn-link" (click)="voltar()">Cancelar</button>
      </div>
    } @else {
      <p>Carregando pedido...</p>
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FaturarPedidoComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly pedidoService = inject(PedidoVendaService);
  private readonly faturamentoService = inject(FaturamentoService);

  readonly pedido = signal<PedidoVendaDetalhe | null>(null);
  readonly linhas = signal<LinhaSelecionavel[]>([]);
  readonly totalFaturamento = signal(0);
  readonly salvando = signal(false);
  readonly erro = signal<string | null>(null);

  todosSelecionados = false;
  vencimento = new Date(Date.now() + 30 * 24 * 60 * 60 * 1000).toISOString().slice(0, 10);
  percentualComissao: number | null = null;

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) return;
    this.pedidoService.obterDetalhe(id).subscribe((p) => {
      this.pedido.set(p);
      this.linhas.set(
        p.itens.map((item) => ({
          item,
          selecionar: item.quantidadePendente > 0,
          quantidade: item.quantidadePendente,
        })),
      );
      this.todosSelecionados = p.itens.some((i) => i.quantidadePendente > 0);
      this.recalcular();
    });
  }

  alternarTodos(valor: boolean): void {
    this.linhas.update((arr) => arr.map((l) => ({
      ...l,
      selecionar: valor && l.item.quantidadePendente > 0,
    })));
    this.recalcular();
  }

  recalcular(): void {
    const total = this.linhas().reduce(
      (acc, l) => acc + (l.selecionar ? l.quantidade * l.item.precoUnitario : 0),
      0);
    this.totalFaturamento.set(total);
  }

  podeFaturar(): boolean {
    const p = this.pedido();
    if (!p) return false;
    if (p.status !== 'Confirmado' && p.status !== 'FaturamentoParcial') return false;
    return this.linhas().some((l) => l.selecionar && l.quantidade > 0)
      && !!this.vencimento;
  }

  faturar(): void {
    const p = this.pedido();
    if (!p) return;
    const itens: FaturarPedidoItem[] = this.linhas()
      .filter((l) => l.selecionar && l.quantidade > 0)
      .map((l) => ({ pedidoVendaItemId: l.item.id, quantidade: Number(l.quantidade) }));

    if (itens.length === 0) {
      this.erro.set('Selecione ao menos 1 item para faturar.');
      return;
    }

    this.salvando.set(true);
    this.erro.set(null);
    this.faturamentoService.faturarPedido({
      pedidoVendaId: p.id,
      vencimentoContaReceber: this.vencimento,
      planoDeContasId: null,
      percentualComissaoOverride: this.percentualComissao,
      itens,
    }).subscribe({
      next: () => {
        alert('Pedido faturado com sucesso. A Conta a Receber foi gerada.');
        this.router.navigateByUrl('/vendas/faturamentos');
      },
      error: (e) => {
        this.salvando.set(false);
        this.erro.set(e?.error?.message ?? 'Falha ao faturar pedido.');
      },
    });
  }

  voltar(): void {
    this.router.navigateByUrl('/vendas/pedidos');
  }
}
