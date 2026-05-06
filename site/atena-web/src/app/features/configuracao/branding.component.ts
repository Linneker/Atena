import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { BrandingService, BrandingTenant } from './configuracao.services';

@Component({
  selector: 'app-branding',
  standalone: true,
  imports: [FormsModule],
  template: `
    <h3>Identidade Visual</h3>
    @if (branding(); as b) {
      <form class="row g-3" (ngSubmit)="salvar()">
        <div class="col-md-6"><label>Razão Social</label><input class="form-control" [(ngModel)]="b.razaoSocial" name="rs" /></div>
        <div class="col-md-6"><label>URL do Logo</label><input class="form-control" [(ngModel)]="b.logoUrl" name="logo" /></div>
        <div class="col-md-3"><label>Cor Primária</label><input class="form-control" type="color" [(ngModel)]="b.corPrimaria" name="cp" /></div>
        <div class="col-md-3"><label>Cor Secundária</label><input class="form-control" type="color" [(ngModel)]="b.corSecundaria" name="cs" /></div>
        <div class="col-md-3"><label>Cor de Destaque</label><input class="form-control" type="color" [(ngModel)]="b.corAccent" name="ca" /></div>
        <div class="col-md-3"><label>Fuso Horário</label><input class="form-control" [(ngModel)]="b.fusoHorario" name="fh" /></div>
        <div class="col-12"><button class="btn btn-primary">Salvar</button></div>
      </form>
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BrandingComponent implements OnInit {
  private readonly servico = inject(BrandingService);
  readonly branding = signal<BrandingTenant | null>(null);

  ngOnInit(): void { this.servico.obter().subscribe((b) => this.branding.set(b)); }

  salvar(): void {
    const b = this.branding();
    if (b) this.servico.salvar(b).subscribe();
  }
}
