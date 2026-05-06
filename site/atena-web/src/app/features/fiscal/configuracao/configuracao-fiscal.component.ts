import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ConfiguracaoFiscal, ConfiguracaoFiscalService } from '../fiscal.services';

@Component({
  selector: 'app-configuracao-fiscal',
  standalone: true,
  imports: [FormsModule],
  template: `
    <h3>Configuração Fiscal</h3>
    @if (config(); as c) {
      <div class="card p-3 mb-3">
        <div class="row g-3">
          <div class="col-md-4">
            <label class="form-label">Ambiente</label>
            <select class="form-select" [(ngModel)]="ambiente">
              <option value="HOMOLOGACAO">Homologação</option>
              <option value="PRODUCAO">Produção</option>
            </select>
          </div>
          <div class="col-md-2">
            <label class="form-label">Série NF-e</label>
            <input class="form-control" [value]="c.serieNFe" disabled />
          </div>
          <div class="col-md-3">
            <label class="form-label">Certificado</label>
            <input class="form-control" [value]="c.certificadoNome ?? '—'" disabled />
          </div>
          <div class="col-md-3">
            <label class="form-label">Vencimento</label>
            <input class="form-control" [value]="c.certificadoVencimento ?? '—'" disabled />
          </div>
          <div class="col-12">
            <button class="btn btn-primary me-2" (click)="salvarAmbiente()">Salvar Ambiente</button>
          </div>
        </div>
      </div>
    }
    <div class="card p-3">
      <h5>Importar Certificado A1</h5>
      <input type="file" class="form-control mb-2" accept=".pfx" (change)="onFile($event)" />
      <input class="form-control mb-2" type="password" placeholder="Senha do certificado" [(ngModel)]="senha" />
      <button class="btn btn-success" [disabled]="!arquivo()" (click)="importar()">Importar</button>
    </div>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ConfiguracaoFiscalComponent implements OnInit {
  private readonly servico = inject(ConfiguracaoFiscalService);
  readonly config = signal<ConfiguracaoFiscal | null>(null);
  readonly arquivo = signal<File | null>(null);
  ambiente: ConfiguracaoFiscal['ambiente'] = 'HOMOLOGACAO';
  senha = '';

  ngOnInit(): void {
    this.servico.obter().subscribe((c) => {
      this.config.set(c);
      this.ambiente = c.ambiente;
    });
  }

  onFile(ev: Event): void {
    const file = (ev.target as HTMLInputElement).files?.[0];
    if (file) this.arquivo.set(file);
  }

  salvarAmbiente(): void {
    this.servico.alterarAmbiente(this.ambiente).subscribe(() => this.ngOnInit());
  }

  importar(): void {
    const f = this.arquivo();
    if (!f) return;
    this.servico.importarCertificado(f, this.senha).subscribe(() => {
      this.arquivo.set(null);
      this.senha = '';
      this.ngOnInit();
    });
  }
}
