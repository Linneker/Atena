import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { ClienteService, FuncionarioService, ProdutoService, type Cliente, type Funcionario, type Produto } from '@features/cadastros/cadastros.services';
import { EstoquesService, type ConsultarSaldoResposta, type EstoqueResumo } from '@features/estoque/estoque.services';
import { ConfirmarDialogComponent } from '@shared/confirmation/confirmar-dialog.component';
import { InfoDialogComponent } from '@shared/confirmation/info-dialog.component';
import { CriarPedidoVendaPayload, PedidoVendaService } from '../vendas.services';
import { SolicitacaoCompraApiService } from '../../compras/solicitacao-compra.service';

interface LinhaItem {
  produtoId: string;
  produtoNome: string;
  quantidade: number;
  precoUnitario: number;
  saldoDisponivel: number | null;   // null = ainda não consultado
  semEstoque: boolean;               // true se quantidade > saldo
}

type DialogState =
  | { tipo: 'solicitar'; linha: LinhaItem }
  | { tipo: 'info'; titulo: string; mensagem: string; estado: 'sucesso' | 'erro' };

@Component({
  selector: 'app-novo-pedido-venda',
  standalone: true,
  imports: [CommonModule, FormsModule, ConfirmarDialogComponent, InfoDialogComponent],
  template: `
    <h3>Novo Pedido de Venda</h3>

    <div class="row g-3">
      <div class="col-md-4">
        <label class="form-label">Cliente *</label>
        <select class="form-select" [(ngModel)]="clienteId">
          <option [ngValue]="null">— selecione —</option>
          @for (c of clientes(); track c.id) {
            <option [ngValue]="c.id">{{ c.nome }}</option>
          }
        </select>
      </div>
      <div class="col-md-4">
        <label class="form-label">Vendedor</label>
        <select class="form-select" [(ngModel)]="vendedorId">
          <option [ngValue]="null">— sem vendedor —</option>
          @for (v of vendedores(); track v.id) {
            <option [ngValue]="v.id">{{ v.nomeCompleto }}</option>
          }
        </select>
      </div>
      <div class="col-md-4">
        <label class="form-label">Estoque *</label>
        <select class="form-select" [(ngModel)]="estoqueId" (ngModelChange)="recalcularSaldosDeTodos()">
          <option [ngValue]="null">— selecione —</option>
          @for (e of estoques(); track e.id) {
            <option [ngValue]="e.id">{{ e.codigo }} — {{ e.nome }}</option>
          }
        </select>
      </div>
      <div class="col-md-4">
        <label class="form-label">% Desconto</label>
        <input type="number" class="form-control" [(ngModel)]="descontoPercentual" min="0" max="100" step="0.01" />
      </div>
      <div class="col-md-4">
        <label class="form-label">Condição de pagamento</label>
        <input class="form-control" [(ngModel)]="condicaoPagamento" placeholder="ex: 30/60/90" />
      </div>
      <div class="col-md-4">
        <label class="form-label">Observação</label>
        <input class="form-control" [(ngModel)]="observacao" />
      </div>
    </div>

    <hr />

    <div class="d-flex justify-content-between align-items-center mb-2">
      <h5 class="m-0">Itens</h5>
      <button class="btn btn-sm btn-outline-primary" (click)="adicionarLinha()" [disabled]="!estoqueId">
        + Adicionar produto
      </button>
    </div>

    @if (!estoqueId) {
      <div class="alert alert-warning">Selecione o estoque antes de adicionar produtos.</div>
    }

    <div class="table-responsive">
      <table class="table table-sm">
        <thead class="table-light">
          <tr>
            <th>Produto</th>
            <th class="text-end">Qtd</th>
            <th class="text-end">Saldo disp.</th>
            <th class="text-end">Preço unit.</th>
            <th class="text-end">Subtotal</th>
            <th></th>
          </tr>
        </thead>
        <tbody>
          @for (linha of linhas(); track linha; let i = $index) {
            <tr [class.table-danger]="linha.semEstoque">
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
                       [(ngModel)]="linha.quantidade" min="0" step="0.001"
                       (ngModelChange)="verificarSaldo(linha)" />
              </td>
              <td class="text-end">
                @if (linha.saldoDisponivel === null) { — }
                @else if (linha.semEstoque) {
                  <span class="text-danger fw-bold">{{ linha.saldoDisponivel }}</span>
                } @else {
                  <span class="text-success">{{ linha.saldoDisponivel }}</span>
                }
              </td>
              <td class="text-end">
                <input type="number" class="form-control form-control-sm text-end" style="width:110px"
                       [(ngModel)]="linha.precoUnitario" min="0" step="0.01" />
              </td>
              <td class="text-end">{{ (linha.quantidade * linha.precoUnitario).toFixed(2) }}</td>
              <td>
                @if (linha.semEstoque && linha.produtoId) {
                  <button class="btn btn-sm btn-warning"
                          (click)="dialog.set({ tipo: 'solicitar', linha })">
                    Solicitar compra
                  </button>
                }
                <button class="btn btn-sm btn-link text-danger" (click)="removerLinha(i)">remover</button>
              </td>
            </tr>
          } @empty {
            <tr><td colspan="6" class="text-center text-muted">Nenhum item.</td></tr>
          }
        </tbody>
        <tfoot class="table-light">
          <tr>
            <th colspan="4" class="text-end">Subtotal:</th>
            <th class="text-end">R$ {{ subtotal().toFixed(2) }}</th>
            <th></th>
          </tr>
          <tr>
            <th colspan="4" class="text-end">Total c/ desconto:</th>
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
      @switch (d.tipo) {
        @case ('solicitar') {
          <app-confirmar-dialog
            titulo="Solicitar Compra"
            [mensagem]="'Criar uma Solicitação de Compra para &quot;' + d.linha.produtoNome + '&quot; com quantidade ' + (d.linha.quantidade - (d.linha.saldoDisponivel ?? 0)) + ' (faltante)?\\n\\nA solicitação irá para o setor de compras.'"
            textoConfirmar="Solicitar compra"
            cor="primario"
            (cancelar)="dialog.set(null)"
            (confirmar)="solicitarCompra(d.linha)" />
        }
        @case ('info') {
          <app-info-dialog [titulo]="d.titulo" [mensagem]="d.mensagem" [tipo]="d.estado" (fechar)="dialog.set(null)" />
        }
      }
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NovoPedidoVendaComponent implements OnInit {
  private readonly clienteService = inject(ClienteService);
  private readonly funcionarioService = inject(FuncionarioService);
  private readonly produtoService = inject(ProdutoService);
  private readonly estoquesService = inject(EstoquesService);
  private readonly pedidoService = inject(PedidoVendaService);
  private readonly solicitacaoService = inject(SolicitacaoCompraApiService);
  private readonly router = inject(Router);

  readonly clientes = signal<Cliente[]>([]);
  readonly vendedores = signal<Funcionario[]>([]);
  readonly estoques = signal<EstoqueResumo[]>([]);
  readonly produtos = signal<Produto[]>([]);
  readonly linhas = signal<LinhaItem[]>([]);

  readonly dialog = signal<DialogState | null>(null);
  readonly salvando = signal(false);
  readonly erro = signal<string | null>(null);

  clienteId: string | null = null;
  vendedorId: string | null = null;
  estoqueId: string | null = null;
  descontoPercentual: number | null = null;
  condicaoPagamento = '';
  observacao = '';

  ngOnInit(): void {
    forkJoin({
      clientes: this.clienteService.listar({ pagina: 1, tamanhoPagina: 200 }),
      funcionarios: this.funcionarioService.listar({ pagina: 1, tamanhoPagina: 200 }),
      estoques: this.estoquesService.listar(),
      produtos: this.produtoService.listar({ pagina: 1, tamanhoPagina: 200 }),
    }).subscribe(({ clientes, funcionarios, estoques, produtos }) => {
      this.clientes.set(clientes.itens);
      this.vendedores.set(funcionarios.itens);
      this.estoques.set(estoques.items.filter((e) => e.ativo));
      this.produtos.set(produtos.itens);
    });
  }

  adicionarLinha(): void {
    this.linhas.update((arr) => [...arr, {
      produtoId: '', produtoNome: '', quantidade: 1, precoUnitario: 0,
      saldoDisponivel: null, semEstoque: false,
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
    this.verificarSaldo(linha);
  }

  verificarSaldo(linha: LinhaItem): void {
    if (!linha.produtoId || !this.estoqueId) {
      linha.saldoDisponivel = null;
      linha.semEstoque = false;
      this.linhas.update((a) => [...a]);
      return;
    }
    this.estoquesService.consultarSaldoProduto(linha.produtoId, this.estoqueId).subscribe({
      next: (s: ConsultarSaldoResposta) => {
        const porEstoque = s.porEstoque.find((p) => p.estoqueId === this.estoqueId);
        linha.saldoDisponivel = porEstoque?.saldoDisponivel ?? 0;
        linha.semEstoque = linha.quantidade > linha.saldoDisponivel;
        this.linhas.update((a) => [...a]);
      },
      error: () => {
        linha.saldoDisponivel = 0;
        linha.semEstoque = linha.quantidade > 0;
        this.linhas.update((a) => [...a]);
      },
    });
  }

  recalcularSaldosDeTodos(): void {
    for (const l of this.linhas()) this.verificarSaldo(l);
  }

  subtotal(): number {
    return this.linhas().reduce((acc, l) => acc + l.quantidade * l.precoUnitario, 0);
  }

  total(): number {
    const desc = this.descontoPercentual ?? 0;
    return this.subtotal() * (1 - desc / 100);
  }

  podeSalvar(): boolean {
    if (!this.clienteId || !this.estoqueId) return false;
    const linhas = this.linhas();
    if (linhas.length === 0) return false;
    return linhas.every((l) => l.produtoId && l.quantidade > 0 && l.precoUnitario >= 0 && !l.semEstoque);
  }

  salvar(): void {
    if (!this.clienteId || !this.estoqueId) return;
    this.salvando.set(true);
    this.erro.set(null);
    const payload: CriarPedidoVendaPayload = {
      clienteId: this.clienteId,
      vendedorId: this.vendedorId,
      estoqueId: this.estoqueId,
      descontoPercentual: this.descontoPercentual,
      condicaoPagamento: this.condicaoPagamento || null,
      observacao: this.observacao || null,
      itens: this.linhas().map((l) => ({
        produtoId: l.produtoId,
        quantidade: Number(l.quantidade),
        precoUnitario: Number(l.precoUnitario),
      })),
    };
    this.pedidoService.criarComItens(payload).subscribe({
      next: (r) => {
        this.dialog.set({
          tipo: 'info',
          titulo: 'Pedido criado',
          mensagem: `Pedido ${r.numero} criado em Rascunho. Confirme-o em Vendas → Pedidos para reservar o estoque.`,
          estado: 'sucesso',
        });
      },
      error: (e) => {
        this.salvando.set(false);
        this.erro.set(e?.error?.message ?? 'Falha ao criar pedido.');
      },
    });
  }

  solicitarCompra(linha: LinhaItem): void {
    const faltante = linha.quantidade - (linha.saldoDisponivel ?? 0);
    this.solicitacaoService.criar({
      justificativa: `Solicitação gerada do pedido de venda — produto "${linha.produtoNome}" sem saldo suficiente.`,
      itens: [{
        produtoId: linha.produtoId,
        quantidade: faltante,
        precoEstimado: linha.precoUnitario,
        observacao: 'Faltante para atender pedido de venda',
      }],
      enviarParaAprovacao: false,
    }).subscribe({
      next: (r) => this.dialog.set({
        tipo: 'info',
        titulo: 'Solicitação enviada',
        mensagem: `Solicitação ${r.numero} criada. Acompanhe em Compras → Solicitações.`,
        estado: 'sucesso',
      }),
      error: (e) => this.dialog.set({
        tipo: 'info',
        titulo: 'Falha',
        mensagem: e?.error?.message ?? 'Não foi possível criar a solicitação.',
        estado: 'erro',
      }),
    });
  }

  voltar(): void {
    this.router.navigateByUrl('/vendas/pedidos');
  }
}
