import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { ProdutoService, type Produto } from '@features/cadastros/cadastros.services';
import { SolicitacaoCompraService, type SolicitacaoDetalhe } from '../compras.services';

@Component({
  selector: 'app-solicitacao-form',
  standalone: true,
  imports: [CommonModule],
  template: `
    @if (sol(); as s) {
      <h3>Solicitação {{ s.numero }}</h3>
      <div class="row mb-3">
        <div class="col-md-3"><strong>Status:</strong> <span class="badge bg-info">{{ s.status }}</span></div>
        <div class="col-md-3"><strong>Data:</strong> {{ s.dataSolicitacao | date:'dd/MM/yyyy' }}</div>
        <div class="col-md-3"><strong>Valor:</strong> R$ {{ s.valorTotal.toFixed(2) }}</div>
        <div class="col-md-3">
          @if (s.aprovadoEm) {
            <strong>Aprovada em:</strong> {{ s.aprovadoEm | date:'dd/MM/yyyy HH:mm' }}
          }
        </div>
      </div>

      @if (s.justificativa) {
        <div class="alert alert-secondary">
          <strong>Justificativa:</strong> {{ s.justificativa }}
        </div>
      }
      @if (s.motivoRejeicao) {
        <div class="alert alert-danger">
          <strong>Rejeitada:</strong> {{ s.motivoRejeicao }}
        </div>
      }

      <h5>Itens</h5>
      <table class="table table-sm">
        <thead class="table-light">
          <tr>
            <th>Produto</th>
            <th class="text-end">Quantidade</th>
            <th class="text-end">Preço estimado</th>
            <th>Observação</th>
            <th class="text-end">Subtotal</th>
          </tr>
        </thead>
        <tbody>
          @for (item of s.itens; track item.id) {
            <tr>
              <td>{{ nomeProduto(item.produtoId) }}</td>
              <td class="text-end">{{ item.quantidade }}</td>
              <td class="text-end">{{ item.precoEstimado != null ? 'R$ ' + item.precoEstimado.toFixed(2) : '—' }}</td>
              <td>{{ item.observacao ?? '' }}</td>
              <td class="text-end">
                {{ item.precoEstimado != null ? 'R$ ' + (item.quantidade * item.precoEstimado).toFixed(2) : '—' }}
              </td>
            </tr>
          } @empty {
            <tr><td colspan="5" class="text-center text-muted">Sem itens.</td></tr>
          }
        </tbody>
      </table>

      <div class="mt-3">
        <button class="btn btn-link" (click)="voltar()">Voltar</button>
      </div>
    } @else {
      <p>Carregando solicitação...</p>
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class SolicitacaoFormComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly servico = inject(SolicitacaoCompraService);
  private readonly produtoService = inject(ProdutoService);

  readonly sol = signal<SolicitacaoDetalhe | null>(null);
  readonly produtos = signal<Produto[]>([]);

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id || id === 'novo') {
      this.router.navigateByUrl('/compras/solicitacoes');
      return;
    }
    this.servico.obterDetalhe(id).subscribe((s) => this.sol.set(s));
    this.produtoService.listar({ pagina: 1, tamanhoPagina: 200 })
      .subscribe((p) => this.produtos.set(p.itens));
  }

  nomeProduto(id: string): string {
    const p = this.produtos().find((x) => x.id === id);
    return p ? `${p.codigo} — ${p.nome}` : id;
  }

  voltar(): void { this.router.navigateByUrl('/compras/solicitacoes'); }
}
