import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { forkJoin } from 'rxjs';
import { EstoquesService, type EstoqueResumo } from '@features/estoque/estoque.services';
import { InfoDialogComponent } from '@shared/confirmation/info-dialog.component';
import {
  DevolucaoVendaService, FaturamentoDetalhe, FaturamentoItemDetalhe,
  FaturamentoService, RegistrarDevolucaoItem,
} from '../vendas.services';

interface LinhaDevolucao {
  item: FaturamentoItemDetalhe;
  selecionar: boolean;
  quantidade: number;
}

@Component({
  selector: 'app-registrar-devolucao',
  standalone: true,
  imports: [CommonModule, FormsModule, InfoDialogComponent],
  template: `
    @if (fat(); as f) {
      <h3>Registrar Devolução — Faturamento {{ f.numero }}</h3>
      <p class="text-muted">
        Data: {{ f.dataFaturamento | date:'dd/MM/yyyy' }} · Valor total: R$ {{ f.valorTotal.toFixed(2) }}
      </p>

      <div class="row g-2 mb-3">
        <div class="col-md-4">
          <label class="form-label small">Estoque de retorno *</label>
          <select class="form-select form-select-sm" [(ngModel)]="estoqueDestinoId">
            <option [ngValue]="null">— selecione —</option>
            @for (e of estoques(); track e.id) {
              <option [ngValue]="e.id">{{ e.codigo }} — {{ e.nome }}</option>
            }
          </select>
        </div>
        <div class="col-md-8">
          <label class="form-label small">Motivo</label>
          <input class="form-control form-control-sm" [(ngModel)]="motivo"
                 placeholder="ex: produto com defeito, desistência do cliente, etc." />
        </div>
      </div>

      <h5>Itens faturados</h5>
      <table class="table table-sm">
        <thead class="table-light">
          <tr>
            <th><input type="checkbox" [(ngModel)]="todosSelecionados" (ngModelChange)="alternarTodos($event)" /></th>
            <th>Produto</th>
            <th class="text-end">Qtd faturada</th>
            <th class="text-end">Qtd a devolver</th>
            <th class="text-end">Preço unit.</th>
            <th class="text-end">Subtotal</th>
          </tr>
        </thead>
        <tbody>
          @for (l of linhas(); track l.item.id) {
            <tr>
              <td>
                <input type="checkbox" [(ngModel)]="l.selecionar" (ngModelChange)="recalcular()" />
              </td>
              <td>{{ l.item.produtoNome ?? l.item.produtoId }}</td>
              <td class="text-end">{{ l.item.quantidade }}</td>
              <td class="text-end" style="width:120px">
                <input type="number" class="form-control form-control-sm text-end"
                       [(ngModel)]="l.quantidade"
                       [disabled]="!l.selecionar"
                       [max]="l.item.quantidade" min="0" step="0.001"
                       (ngModelChange)="recalcular()" />
              </td>
              <td class="text-end">{{ l.item.precoUnitario.toFixed(2) }}</td>
              <td class="text-end">{{ (l.selecionar ? l.quantidade * l.item.precoUnitario : 0).toFixed(2) }}</td>
            </tr>
          }
        </tbody>
        <tfoot>
          <tr>
            <th colspan="5" class="text-end">Total a devolver:</th>
            <th class="text-end">R$ {{ total().toFixed(2) }}</th>
          </tr>
        </tfoot>
      </table>

      @if (erro()) {
        <div class="alert alert-danger mt-3">{{ erro() }}</div>
      }

      <div class="mt-4 d-flex gap-2">
        <button class="btn btn-danger" [disabled]="!podeSalvar() || salvando()" (click)="salvar()">
          {{ salvando() ? 'Registrando...' : 'Registrar devolução' }}
        </button>
        <button class="btn btn-link" (click)="voltar()">Cancelar</button>
      </div>

      @if (dialog(); as d) {
        <app-info-dialog [titulo]="d.titulo" [mensagem]="d.mensagem" [tipo]="d.estado" (fechar)="onDialogFechar(d)" />
      }
    } @else {
      <p>Carregando faturamento...</p>
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class RegistrarDevolucaoComponent implements OnInit {
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  private readonly fatService = inject(FaturamentoService);
  private readonly devolService = inject(DevolucaoVendaService);
  private readonly estoquesService = inject(EstoquesService);

  readonly fat = signal<FaturamentoDetalhe | null>(null);
  readonly estoques = signal<EstoqueResumo[]>([]);
  readonly linhas = signal<LinhaDevolucao[]>([]);
  readonly salvando = signal(false);
  readonly erro = signal<string | null>(null);
  readonly dialog = signal<{ titulo: string; mensagem: string; estado: 'sucesso' | 'erro' } | null>(null);

  todosSelecionados = false;
  estoqueDestinoId: string | null = null;
  motivo = '';

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (!id) return;
    forkJoin({
      fat: this.fatService.obterDetalhe(id),
      estoques: this.estoquesService.listar(),
    }).subscribe(({ fat, estoques }) => {
      this.fat.set(fat);
      this.estoques.set(estoques.items.filter((e) => e.ativo));
      this.linhas.set(fat.itens.map((i) => ({
        item: i, selecionar: false, quantidade: i.quantidade,
      })));
    });
  }

  alternarTodos(valor: boolean): void {
    this.linhas.update((arr) => arr.map((l) => ({ ...l, selecionar: valor })));
  }

  recalcular(): void { this.linhas.update((a) => [...a]); }

  total(): number {
    return this.linhas().reduce((acc, l) =>
      acc + (l.selecionar ? l.quantidade * l.item.precoUnitario : 0), 0);
  }

  podeSalvar(): boolean {
    if (!this.fat() || !this.estoqueDestinoId) return false;
    return this.linhas().some((l) => l.selecionar && l.quantidade > 0);
  }

  salvar(): void {
    const f = this.fat();
    if (!f || !this.estoqueDestinoId) return;
    const itens: RegistrarDevolucaoItem[] = this.linhas()
      .filter((l) => l.selecionar && l.quantidade > 0)
      .map((l) => ({ faturamentoItemId: l.item.id, quantidade: Number(l.quantidade) }));

    this.salvando.set(true);
    this.erro.set(null);
    this.devolService.registrar({
      faturamentoId: f.id,
      estoqueDestinoId: this.estoqueDestinoId,
      motivo: this.motivo || null,
      itens,
    }).subscribe({
      next: (r) => this.dialog.set({
        titulo: 'Devolução registrada',
        mensagem: `Devolução ${r.numero} criada. Estoque retornado.`,
        estado: 'sucesso',
      }),
      error: (e) => {
        this.salvando.set(false);
        this.erro.set(e?.error?.message ?? 'Falha ao registrar devolução.');
      },
    });
  }

  onDialogFechar(d: { estado: 'sucesso' | 'erro' }): void {
    this.dialog.set(null);
    if (d.estado === 'sucesso') this.router.navigateByUrl('/vendas/devolucoes');
  }

  voltar(): void { this.router.navigateByUrl('/vendas/faturamentos'); }
}
