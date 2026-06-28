import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { FornecedorService, ProdutoService, type Fornecedor, type Produto } from '@features/cadastros/cadastros.services';
import { InfoDialogComponent } from '@shared/confirmation/info-dialog.component';
import { CriarPedidoCompraPayload, PedidoCompraService } from '../compras.services';

interface LinhaItem {
  produtoId: string;
  produtoNome: string;
  quantidade: number;
  precoUnitario: number;
}

@Component({
  selector: 'app-novo-pedido-compra',
  standalone: true,
  imports: [CommonModule, FormsModule, InfoDialogComponent],
  template: `
    <h3>Novo Pedido de Compra</h3>

    <div class="row g-3">
      <div class="col-md-6">
        <label class="form-label">Fornecedor *</label>
        <select class="form-select" [(ngModel)]="fornecedorId">
          <option [ngValue]="null">— selecione —</option>
          @for (f of fornecedores(); track f.id) {
            <option [ngValue]="f.id">{{ f.nome }}</option>
          }
        </select>
      </div>
      <div class="col-md-3">
        <label class="form-label">Previsão de entrega</label>
        <input type="date" class="form-control" [(ngModel)]="previsaoEntrega" />
      </div>
      <div class="col-md-3">
        <label class="form-label">Condição de pagamento</label>
        <input class="form-control" [(ngModel)]="condicaoPagamento" placeholder="ex: 30/60" />
      </div>
      <div class="col-md-12">
        <label class="form-label">Observação</label>
        <input class="form-control" [(ngModel)]="observacao" />
      </div>
    </div>

    <hr />

    <div class="d-flex justify-content-between align-items-center mb-2">
      <h5 class="m-0">Itens</h5>
      <button class="btn btn-sm btn-outline-primary" (click)="adicionarLinha()">+ Adicionar produto</button>
    </div>

    <div class="table-responsive">
      <table class="table table-sm">
        <thead class="table-light">
          <tr>
            <th>Produto</th>
            <th class="text-end">Qtd</th>
            <th class="text-end">Preço unit.</th>
            <th class="text-end">Subtotal</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          @for (linha of linhas(); track linha; let i = $index) {
            <tr>
              <td>
                <select class="form-select form-select-sm" [(ngModel)]="linha.produtoId"
                        (ngModelChange)="onProdutoMudou(linha)">
                  <option [ngValue]="''">— selecione produto —</option>
                  @for (p of produtos(); track p.id) {
                    <option [ngValue]="p.id">{{ p.codigo }} — {{ p.nome }}</option>
                  }
                </select>
              </td>
              <td class="text-end">
                <input type="number" class="form-control form-control-sm text-end" style="width:90px"
                       [(ngModel)]="linha.quantidade" min="0" step="0.001" />
              </td>
              <td class="text-end">
                <input type="number" class="form-control form-control-sm text-end" style="width:110px"
                       [(ngModel)]="linha.precoUnitario" min="0" step="0.01" />
              </td>
              <td class="text-end">{{ (linha.quantidade * linha.precoUnitario).toFixed(2) }}</td>
              <td>
                <button class="btn btn-sm btn-link text-danger" (click)="removerLinha(i)">remover</button>
              </td>
            </tr>
          } @empty {
            <tr><td colspan="5" class="text-center text-muted">Nenhum item.</td></tr>
          }
        </tbody>
        <tfoot class="table-light">
          <tr>
            <th colspan="3" class="text-end">Total:</th>
            <th class="text-end">R$ {{ total().toFixed(2) }}</th>
            <th></th>
          </tr>
        </tfoot>
      </table>
    </div>

    @if (erro()) {
      <div class="alert alert-danger mt-3">{{ erro() }}</div>
    }

    <div class="mt-4 d-flex gap-2">
      <button class="btn btn-primary" [disabled]="!podeSalvar() || salvando()" (click)="salvar()">
        {{ salvando() ? 'Salvando...' : 'Salvar pedido' }}
      </button>
      <button class="btn btn-link" (click)="voltar()">Cancelar</button>
    </div>

    @if (dialog(); as d) {
      <app-info-dialog [titulo]="d.titulo" [mensagem]="d.mensagem" [tipo]="d.estado" (fechar)="dialog.set(null)" />
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NovoPedidoCompraComponent implements OnInit {
  private readonly fornecedorService = inject(FornecedorService);
  private readonly produtoService = inject(ProdutoService);
  private readonly pedidoService = inject(PedidoCompraService);
  private readonly router = inject(Router);

  readonly fornecedores = signal<Fornecedor[]>([]);
  readonly produtos = signal<Produto[]>([]);
  readonly linhas = signal<LinhaItem[]>([]);
  readonly dialog = signal<{ titulo: string; mensagem: string; estado: 'sucesso' | 'erro' } | null>(null);
  readonly salvando = signal(false);
  readonly erro = signal<string | null>(null);

  fornecedorId: string | null = null;
  previsaoEntrega = '';
  condicaoPagamento = '';
  observacao = '';

  ngOnInit(): void {
    forkJoin({
      fornecedores: this.fornecedorService.listar({ pagina: 1, tamanhoPagina: 200 }),
      produtos: this.produtoService.listar({ pagina: 1, tamanhoPagina: 200 }),
    }).subscribe(({ fornecedores, produtos }) => {
      this.fornecedores.set(fornecedores.itens);
      this.produtos.set(produtos.itens);
    });
  }

  adicionarLinha(): void {
    this.linhas.update((arr) => [...arr, {
      produtoId: '', produtoNome: '', quantidade: 1, precoUnitario: 0,
    }]);
  }

  removerLinha(i: number): void {
    this.linhas.update((arr) => arr.filter((_, idx) => idx !== i));
  }

  onProdutoMudou(linha: LinhaItem): void {
    const prod = this.produtos().find((p) => p.id === linha.produtoId);
    if (prod) {
      linha.produtoNome = prod.nome;
      linha.precoUnitario = prod.custoMedio ?? 0;
    }
    this.linhas.update((a) => [...a]);
  }

  total(): number {
    return this.linhas().reduce((acc, l) => acc + l.quantidade * l.precoUnitario, 0);
  }

  podeSalvar(): boolean {
    if (!this.fornecedorId) return false;
    const linhas = this.linhas();
    if (linhas.length === 0) return false;
    return linhas.every((l) => l.produtoId && l.quantidade > 0 && l.precoUnitario >= 0);
  }

  salvar(): void {
    if (!this.fornecedorId) return;
    this.salvando.set(true);
    this.erro.set(null);
    const payload: CriarPedidoCompraPayload = {
      fornecedorId: this.fornecedorId,
      previsaoEntrega: this.previsaoEntrega || null,
      condicaoPagamento: this.condicaoPagamento || null,
      observacao: this.observacao || null,
      itens: this.linhas().map((l) => ({
        produtoId: l.produtoId,
        quantidade: Number(l.quantidade),
        precoUnitario: Number(l.precoUnitario),
      })),
    };
    this.pedidoService.criarComItens(payload).subscribe({
      next: (r) => this.dialog.set({
        titulo: 'Pedido de compra criado',
        mensagem: `Pedido ${r.numero} em Rascunho. Use "Enviar fornecedor" na listagem para iniciar a compra.`,
        estado: 'sucesso',
      }),
      error: (e) => {
        this.salvando.set(false);
        this.erro.set(e?.error?.message ?? 'Falha ao criar pedido.');
      },
    });
  }

  voltar(): void {
    this.router.navigateByUrl('/compras/pedidos');
  }
}
