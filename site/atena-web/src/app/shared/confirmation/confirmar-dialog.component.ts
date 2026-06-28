import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-confirmar-dialog',
  standalone: true,
  template: `
    <div class="modal d-block" tabindex="-1" style="background: rgba(0,0,0,0.5)">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">{{ titulo }}</h5>
            <button type="button" class="btn-close" (click)="cancelar.emit()"></button>
          </div>
          <div class="modal-body">
            <p class="mb-0" style="white-space: pre-wrap">{{ mensagem }}</p>
          </div>
          <div class="modal-footer">
            <button class="btn btn-secondary" (click)="cancelar.emit()">{{ textoCancelar }}</button>
            <button class="btn"
                    [class.btn-danger]="cor === 'perigo'"
                    [class.btn-success]="cor === 'sucesso'"
                    [class.btn-primary]="cor === 'primario' || !cor"
                    (click)="confirmar.emit()">
              {{ textoConfirmar }}
            </button>
          </div>
        </div>
      </div>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ConfirmarDialogComponent {
  @Input({ required: true }) titulo = '';
  @Input({ required: true }) mensagem = '';
  @Input() textoConfirmar = 'Confirmar';
  @Input() textoCancelar = 'Cancelar';
  /** Cor do botão confirmar. Default: 'primario'. */
  @Input() cor: 'primario' | 'sucesso' | 'perigo' = 'primario';
  @Output() readonly confirmar = new EventEmitter<void>();
  @Output() readonly cancelar = new EventEmitter<void>();
}
