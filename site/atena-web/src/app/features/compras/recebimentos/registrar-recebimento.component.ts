import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { EstoquesService, type EstoqueResumo } from '@features/estoque/estoque.services';
import { InfoDialogComponent } from '@shared/confirmation/info-dialog.component';
import {
  PedidoCompraDetalhe, PedidoCompraItemDetalhe, PedidoCompraService,
  RecebimentoCompraService, RegistrarRecebimentoItem,
} from '../compras.services';

interface LinhaRecebimento {
  item: PedidoCompraItemDetalhe;
  selecionar: boolean;
  quantidade: number;
  precoUnitario: number;
}

@Component({
  selector: 'app-registrar-recebimento',
  standalone: true,
  imports: [CommonModule, FormsModule, InfoDialogComponent],
  template: `
    @if (pedido(); as p) {
      <h3>Registrar Recebimento — Pedido {{ p.numero }}</h3>
      <p class="text-muted">
        Fornecedor: <strong>{{ p.fornecedorNome ?? '—' }}</strong> ·
        Status: <span class="badge bg-info">{{ p.status }}</span>
      </p>

      @if (p.status === 'Rascunho' || p.status === 'Cancelado') {
        <div class="alert alert-warning">
          Não é possível receber pedido em status <strong>{{ p.status }}</strong>.
        </div>
      }

      <div class="row g-2 mb-3">
        <div class="col-md-4">
          <label class="form-label small">Estoque destino *</label>
          <select class="form-select form-select-sm" [(ngModel)]="estoqueId">
            <option [ngValue]="null">— selecione —</option>
            @for (e of estoques(); track e.id) {
              <option [ngValue]="e.id">{{ e.codigo }} — {{ e.nome }}</option>
            }
          </select>
        </div>
        <div class="col-md-3">
          <label class="form-label small">Data de recebimento</label>
          <input type="date" class="form-control form-control-sm" [(ngModel)]="dataRecebimento" />
        </div>
        <div class="col-md-3">
          <label class="form-label small">Vencimento da Conta a Pagar *</label>
          <input type="date" class="form-control form-control-sm" [(ngModel)]="vencimento" />
        </div>
        <div class="col-md-2">
          <label class="form-label small">NF nº</label>
          <input class="form-control form-control-sm" [(ngModel)]="numeroNotaFiscal" />
        </div>
      </div>

      <h5>Itens do pedido</h5>
      <table class="table table-sm">
        <thead class="table-light">
          <tr>
            <th><input type="checkbox" [(ngModel)]="todosSelecionados" (ngModelChange)="alternarTodos($event)" /></th>
            <th>Produto</th>
            <th class="text-end">Qtd pedida</th>
            <th class="text-end">Já recebida</th>
            <th class="text-end">Pendente</th>
            <th class="text-end">Qtd a receber</th>
            <th class="text-end">Preço unit.</th>
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
              <td class="text-end">{{ l.item.quantidadeRecebida }}</td>
              <td class="text-end">{{ l.item.quantidadePendente }}</td>
              <td class="text-end" style="width:120px">
                <input type="number" class="form-control form-control-sm text-end"
                       [(ngModel)]="l.quantidade"
                       [disabled]="!l.selecionar || l.item.quantidadePendente <= 0"
                       [max]="l.item.quantidadePendente" min="0" step="0.001"
                       (ngModelChange)="recalcular()" />
              </td>
              <td class="text-end" style="width:120px">
                <input type="number" class="form-control form-control-sm text-end"
                       [(ngModel)]="l.precoUnitario"
                       [disabled]="!l.selecionar"
                       min="0" step="0.01"
                       (ngModelChange)="recalcular()" />
              </td>
              <td class="text-end">{{ (l.selecionar ? l.quantidade * l.precoUnitario : 0).toFixed(2) }}</td>
            </tr>
          }
        </tbody>
        <tfoot>
          <tr>
            <th colspan="7" class="text-end">Total do recebimento:</th>
            <th class="text-end">R$ {{ total().toFixed(2) }}</th>
          </tr>
        </tfoot>
      </table>

      @if (erro()) {
        <div class="alert alert-danger mt-3">{{ erro() }}</div>
      }

      <div class="mt-4 d-flex gap-2">
        <button class="btn btn-primary" [disabled]="!podeSalvar() || salvando()" (click)="salvar()">
          {{ salvando() ? 'Registrando...' : 'Registrar recebimento' }}
        </button>
        <button class="btn btn-link" (click)="voltar()">Cancelar</button>
      </div>

      @if (dialog(); as d) {
        <app-info-dialog [titulo]="d.titulo" [mensagem]="d.mensagem" [tipo]="d.estado" (fechar)="onDialogFechar(d)" />
      }
    } @else {
      <p>Carregando pedido...</p>
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RegistrarRecebimentoComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly pedidoService = inject(PedidoCompraService);
  private readonly recebService = inject(RecebimentoCompraService);
  private readonly estoquesService = inject(EstoquesService);

  readonly pedido = signal<PedidoCompraDetalhe | null>(null);
  readonly estoques = signal<EstoqueResumo[]>([]);
  readonly linhas = signal<LinhaRecebimento[]>([]);
  readonly salvando = signal(false);
  readonly erro = signal<string | null>(null);
  readonly dialog = signal<{ titulo: string; mensagem: string; estado: 'sucesso' | 'erro' } | null>(null);

  todosSelecionados = false;
  estoqueId: string | null = null;
  dataRecebimento = new Date().toISOString().slice(0, 10);
  vencimento = new Date(Date.now() + 30 * 24 * 60 * 60 * 1000).toISOString().slice(0, 10);
  numeroNotaFiscal = '';

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) return;
    forkJoin({
      pedido: this.pedidoService.obterDetalhe(id),
      estoques: this.estoquesService.listar(),
    }).subscribe(({ pedido, estoques }) => {
      this.pedido.set(pedido);
      this.estoques.set(estoques.items.filter((e) => e.ativo));
      this.linhas.set(pedido.itens.map((i) => ({
        item: i,
        selecionar: i.quantidadePendente > 0,
        quantidade: i.quantidadePendente,
        precoUnitario: i.precoUnitario,
      })));
      this.todosSelecionados = pedido.itens.some((i) => i.quantidadePendente > 0);
    });
  }

  alternarTodos(valor: boolean): void {
    this.linhas.update((arr) => arr.map((l) => ({ ...l, selecionar: valor && l.item.quantidadePendente > 0 })));
  }

  recalcular(): void { this.linhas.update((a) => [...a]); }

  total(): number {
    return this.linhas().reduce((acc, l) =>
      acc + (l.selecionar ? l.quantidade * l.precoUnitario : 0), 0);
  }

  podeSalvar(): boolean {
    const p = this.pedido();
    if (!p || !this.estoqueId || !this.vencimento) return false;
    if (p.status === 'Rascunho' || p.status === 'Cancelado') return false;
    return this.linhas().some((l) => l.selecionar && l.quantidade > 0);
  }

  salvar(): void {
    const p = this.pedido();
    if (!p || !this.estoqueId) return;
    const itens: RegistrarRecebimentoItem[] = this.linhas()
      .filter((l) => l.selecionar && l.quantidade > 0)
      .map((l) => ({
        pedidoCompraItemId: l.item.id,
        quantidadeRecebida: Number(l.quantidade),
        precoUnitario: Number(l.precoUnitario),
      }));

    this.salvando.set(true);
    this.erro.set(null);
    this.recebService.registrar({
      pedidoCompraId: p.id,
      estoqueId: this.estoqueId,
      dataRecebimento: this.dataRecebimento || null,
      numeroNotaFiscal: this.numeroNotaFiscal || null,
      vencimentoContaPagar: this.vencimento,
      itens,
    }).subscribe({
      next: (r) => this.dialog.set({
        titulo: 'Recebimento registrado',
        mensagem: `Recebimento ${r.numero} criado. Estoque atualizado e Conta a Pagar gerada.`,
        estado: 'sucesso',
      }),
      error: (e) => {
        this.salvando.set(false);
        this.erro.set(e?.error?.message ?? 'Falha ao registrar recebimento.');
      },
    });
  }

  onDialogFechar(d: { estado: 'sucesso' | 'erro' }): void {
    this.dialog.set(null);
    if (d.estado === 'sucesso') this.router.navigateByUrl('/compras/recebimentos');
  }

  voltar(): void { this.router.navigateByUrl('/compras/pedidos'); }
}
