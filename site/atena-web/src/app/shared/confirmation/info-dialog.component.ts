import { ChangeDetectionStrategy, Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-info-dialog',
  standalone: true,
  template: `
    <div class="modal d-block" tabindex="-1" style="background: rgba(0,0,0,0.5)">
      <div class="modal-dialog">
        <div class="modal-content">
          <div class="modal-header">
            <h5 class="modal-title">{{ titulo }}</h5>
            <button type="button" class="btn-close" (click)="fechar.emit()"></button>
          </div>
          <div class="modal-body">
            <p class="mb-0" style="white-space: pre-wrap">{{ mensagem }}</p>
          </div>
          <div class="modal-footer">
            <button class="btn"
                    [class.btn-success]="tipo === 'sucesso'"
                    [class.btn-danger]="tipo === 'erro'"
                    [class.btn-primary]="!tipo || tipo === 'info'"
                    (click)="fechar.emit()">OK</button>
          </div>
        </div>
      </div>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class InfoDialogComponent {
  @Input({ required: true }) titulo = '';
  @Input({ required: true }) mensagem = '';
  @Input() tipo: 'info' | 'sucesso' | 'erro' = 'info';
  @Output() readonly fechar = new EventEmitter<void>();
}
