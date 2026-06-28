import { ChangeDetectionStrategy, Component, EventEmitter, Input, OnInit, Output, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ContaReceber, ReceberContaReceberPayload } from '../financeiro.services';

@Component({
  selector: 'app-receber-conta-dialog',
  standalone: true,
  imports: [ReactiveFormsModule],
  template: `
    <div class="modal d-block" tabindex="-1" style="background: rgba(0,0,0,0.5)">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">Receber Conta a Receber</h5>
            <button type="button" class="btn-close" (click)="fechar.emit()"></button>
          </div>
          <div class="modal-body">
            <p class="mb-3">
              <strong>{{ conta.descricao }}</strong><br />
              <small class="text-muted">Saldo atual: R$ {{ (conta.saldo ?? 0).toFixed(2) }}</small>
            </p>
            <form [formGroup]="form" (ngSubmit)="salvar()">
              <div class="mb-3">
                <label class="form-label">Valor recebido *</label>
                <input class="form-control" type="number" step="0.01" formControlName="valorRecebido" />
              </div>
              <div class="mb-3">
                <label class="form-label">Data do recebimento *</label>
                <input class="form-control" type="date" formControlName="dataRecebimento" />
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
            <button class="btn btn-success" [disabled]="form.invalid || salvando()" (click)="salvar()">
              {{ salvando() ? 'Salvando...' : 'Confirmar recebimento' }}
            </button>
          </div>
        </div>
      </div>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ReceberContaDialogComponent implements OnInit {
  @Input({ required: true }) conta!: ContaReceber;
  @Output() readonly fechar = new EventEmitter<void>();
  @Output() readonly confirmar = new EventEmitter<ReceberContaReceberPayload>();

  private readonly fb = inject(FormBuilder);
  readonly salvando = signal(false);
  readonly errorMsg = signal<string | null>(null);

  readonly form = this.fb.nonNullable.group({
    valorRecebido: [0, [Validators.required, Validators.min(0.01)]],
    dataRecebimento: [new Date().toISOString().substring(0, 10), [Validators.required]],
    observacao: [''],
  });

  ngOnInit(): void {
    this.form.patchValue({ valorRecebido: this.conta.saldo ?? this.conta.valorOriginal });
  }

  salvar(): void {
    if (this.form.invalid) return;
    this.salvando.set(true);
    this.errorMsg.set(null);
    const v = this.form.getRawValue();
    this.confirmar.emit({
      valorRecebido: Number(v.valorRecebido),
      dataRecebimento: v.dataRecebimento,
      observacao: v.observacao || null,
    });
  }

  mostrarErro(msg: string): void {
    this.salvando.set(false);
    this.errorMsg.set(msg);
  }
}
