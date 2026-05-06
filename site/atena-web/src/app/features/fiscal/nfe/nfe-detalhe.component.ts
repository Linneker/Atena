import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { NFe, NFeService } from '../fiscal.services';

@Component({
  selector: 'app-nfe-detalhe',
  standalone: true,
  imports: [FormsModule],
  template: `
    @if (nfe(); as n) {
      <h3>NF-e {{ n.numero }}</h3>
      <p>Chave: <code>{{ n.chave }}</code></p>
      <p>Cliente: {{ n.cliente }} | Valor: {{ n.valor.toFixed(2) }} | Status: <strong>{{ n.status }}</strong></p>
      <p>Emissão: {{ n.emissao }}</p>

      <div class="card p-3 mt-3">
        <h5>Cancelar NF-e</h5>
        <textarea class="form-control mb-2" placeholder="Justificativa (mín 15 caracteres)" [(ngModel)]="justificativa"></textarea>
        <button class="btn btn-danger" [disabled]="justificativa.length < 15" (click)="cancelar()">Cancelar NF-e</button>
      </div>

      <div class="card p-3 mt-3">
        <h5>Carta de Correção (CC-e)</h5>
        <textarea class="form-control mb-2" placeholder="Texto de correção" [(ngModel)]="correcao"></textarea>
        <button class="btn btn-warning" [disabled]="correcao.length < 15" (click)="emitirCce()">Emitir CC-e</button>
      </div>
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class NFeDetalheComponent implements OnInit {
  private readonly servico = inject(NFeService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  readonly nfe = signal<NFe | null>(null);
  justificativa = '';
  correcao = '';

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) this.servico.obter(id).subscribe((r) => this.nfe.set(r));
  }

  cancelar(): void {
    const id = this.nfe()?.id;
    if (!id) return;
    this.servico.cancelar(id, this.justificativa).subscribe(() => this.router.navigateByUrl('/fiscal/nfe'));
  }

  emitirCce(): void {
    const id = this.nfe()?.id;
    if (!id) return;
    this.servico.emitirCce(id, this.correcao).subscribe(() => { this.correcao = ''; });
  }
}
