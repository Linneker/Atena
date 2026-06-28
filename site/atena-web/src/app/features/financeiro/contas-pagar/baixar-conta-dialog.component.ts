import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { BaixarContaPagarPayload, ContaPagar } from '../financeiro.services';

@Component({
  selector: 'app-baixar-conta-dialog',
  standalone: true,
  imports: [ReactiveFormsModule],
  template: `
    <div class="modal d-block" tabindex="-1" style="background: rgba(0,0,0,0.5)">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Baixar Conta a Pagar</h5>
            <button type="button" class="btn-close" (click)="fechar.emit()"></button>
          </div>
          <div class="modal-body">
            <p class="mb-3">
              <strong>{{ conta.descricao }}</strong><br />
              <small class="text-muted">Saldo atual: R$ {{ (conta.saldo ?? 0).toFixed(2) }}</small>
            </p>
            <form [formGroup]="form" (ngSubmit)="salvar()">
              <div class="mb-3">
                <label class="form-label">Valor pago *</label>
                <input class="form-control" type="number" step="0.01" formControlName="valorPago" />
              </div>
              <div class="mb-3">
                <label class="form-label">Data do pagamento *</label>
                <input class="form-control" type="date" formControlName="dataPagamento" />
              </div>
              <div class="mb-3">
                <label class="form-label">Forma de pagamento *</label>
                <select class="form-select" formControlName="formaPagamento">
                  <option value="Dinheiro">Dinheiro</option>
                  <option value="Pix">Pix</option>
                  <option value="CartaoCredito">Cartão de Crédito</option>
                  <option value="CartaoDebito">Cartão de Débito</option>
                  <option value="Boleto">Boleto</option>
                  <option value="Transferencia">Transferência</option>
                  <option value="Cheque">Cheque</option>
                  <option value="Outro">Outro</option>
                </select>
              </div>
              <div class="mb-3">
                <label class="form-label">Observação</label>
                <input class="form-control" formControlName="observacao" />
              </div>
              @if (errorMsg()) {
                <div class="alert alert-danger">{{ errorMsg() }}</div>
              }
            </form>
          </div>
          <div class="modal-footer">
            <button class="btn btn-secondary" (click)="fechar.emit()">Cancelar</button>
            <button class="btn btn-primary" [disabled]="form.invalid || salvando()" (click)="salvar()">
              {{ salvando() ? 'Salvando...' : 'Dar baixa' }}
            </button>
          </div>
        </div>
      </div>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BaixarContaDialogComponent implements OnInit {
  @Input({ required: true }) conta!: ContaPagar;
  @Output() readonly fechar = new EventEmitter<void>();
  @Output() readonly confirmar = new EventEmitter<BaixarContaPagarPayload>();

  private readonly fb = inject(FormBuilder);
  readonly salvando = signal(false);
  readonly errorMsg = signal<string | null>(null);

  readonly form = this.fb.nonNullable.group({
    valorPago: [0, [Validators.required, Validators.min(0.01)]],
    dataPagamento: [new Date().toISOString().substring(0, 10), [Validators.required]],
    formaPagamento: ['Pix', [Validators.required]],
    observacao: [''],
  });

  ngOnInit(): void {
    this.form.patchValue({ valorPago: this.conta.saldo ?? this.conta.valorOriginal });
  }

  salvar(): void {
    if (this.form.invalid) return;
    this.salvando.set(true);
    this.errorMsg.set(null);
    const v = this.form.getRawValue();
    this.confirmar.emit({
      valorPago: Number(v.valorPago),
      dataPagamento: v.dataPagamento,
      formaPagamento: v.formaPagamento,
      observacao: v.observacao || null,
    });
  }

  reset(): void {
    this.salvando.set(false);
  }

  mostrarErro(msg: string): void {
    this.salvando.set(false);
    this.errorMsg.set(msg);
  }
}
