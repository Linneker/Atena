import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { TenantsService } from './tenants.service';

@Component({
  selector: 'app-tenant-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  template: `
    <h3>{{ isNovo() ? 'Novo tenant' : 'Editar tenant' }}</h3>
    <form [formGroup]="form" (ngSubmit)="salvar()" class="mt-3" style="max-width:720px">
      <div class="row g-3">
        <div class="col-md-8">
          <label class="form-label">Razão social *</label>
          <input class="form-control" formControlName="razaoSocial" />
        </div>
        <div class="col-md-4">
          <label class="form-label">CNPJ *</label>
          <input class="form-control" formControlName="cnpj" [readonly]="!isNovo()" />
        </div>

        <div class="col-md-4">
          <label class="form-label">Plano *</label>
          <select class="form-select" formControlName="plano">
            <option value="FREE">FREE</option>
            <option value="BASIC">BASIC</option>
            <option value="PRO">PRO</option>
            <option value="ENTERPRISE">ENTERPRISE</option>
          </select>
        </div>
        <div class="col-md-4">
          <label class="form-label">Fuso horário</label>
          <input class="form-control" formControlName="fusoHorario" />
        </div>
        <div class="col-md-4">
          <label class="form-label">Status</label>
          <select class="form-select" formControlName="status">
            <option [ngValue]="1">Ativo</option>
            <option [ngValue]="0">Inativo</option>
            <option [ngValue]="2">Suspenso</option>
          </select>
        </div>

        <div class="col-md-4">
          <label class="form-label">Cor primária</label>
          <input class="form-control" formControlName="corPrimaria" placeholder="#321fdb" />
        </div>
        <div class="col-md-8">
          <label class="form-label">Logo URL</label>
          <input class="form-control" formControlName="logoUrl" />
        </div>

        @if (isNovo()) {
          <div class="col-12 mt-3">
            <h5>Administrador inicial</h5>
            <small class="text-muted">É criado junto com o tenant; recebe a role Administrador.</small>
          </div>
          <div class="col-md-6">
            <label class="form-label">Nome completo *</label>
            <input class="form-control" formControlName="adminNomeCompleto" />
          </div>
          <div class="col-md-6">
            <label class="form-label">E-mail *</label>
            <input class="form-control" formControlName="adminEmail" type="email" />
          </div>
          <div class="col-md-6">
            <label class="form-label">Senha *</label>
            <input class="form-control" formControlName="adminSenha" type="password" />
            <small class="text-muted">Mínimo 8 chars, com maiúscula, número e símbolo.</small>
          </div>
        }
      </div>

      @if (errorMsg()) {
        <div class="alert alert-danger mt-3">{{ errorMsg() }}</div>
      }

      <div class="mt-4">
        <button class="btn btn-primary" type="submit" [disabled]="form.invalid || salvando()">
          {{ salvando() ? 'Salvando...' : 'Salvar' }}
        </button>
        <button type="button" class="btn btn-link" (click)="voltar()">Cancelar</button>
      </div>
    </form>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TenantFormComponent implements OnInit {
  private readonly fb = inject(FormBuilder);
  private readonly servico = inject(TenantsService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);

  readonly salvando = signal(false);
  readonly errorMsg = signal<string | null>(null);
  readonly isNovo = signal(true);
  private id: string | null = null;

  readonly form = this.fb.nonNullable.group({
    razaoSocial: ['', [Validators.required, Validators.maxLength(255)]],
    cnpj: ['', [Validators.required]],
    plano: ['FREE', [Validators.required]],
    fusoHorario: ['America/Sao_Paulo'],
    status: [1],
    corPrimaria: [''],
    logoUrl: [''],
    adminNomeCompleto: [''],
    adminEmail: [''],
    adminSenha: [''],
  });

  ngOnInit(): void {
    const param = this.route.snapshot.paramMap.get('id');
    if (param && param !== 'novo') {
      this.id = param;
      this.isNovo.set(false);
      this.servico.obter(param).subscribe((t) => {
        this.form.patchValue({
          razaoSocial: t.razaoSocial,
          cnpj: t.cnpj,
          plano: t.plano,
          fusoHorario: t.fusoHorario,
          status: t.status,
          corPrimaria: t.corPrimaria ?? '',
          logoUrl: t.logoUrl ?? '',
        });
      });
    } else {
      this.form.controls.adminNomeCompleto.setValidators([Validators.required]);
      this.form.controls.adminEmail.setValidators([Validators.required, Validators.email]);
      this.form.controls.adminSenha.setValidators([Validators.required, Validators.minLength(8)]);
    }
  }

  salvar(): void {
    if (this.form.invalid) return;
    this.salvando.set(true);
    this.errorMsg.set(null);
    const v = this.form.getRawValue();

    const obs = this.isNovo()
      ? this.servico.registrar({
          razaoSocial: v.razaoSocial,
          cnpj: v.cnpj.replace(/\D/g, ''),
          plano: v.plano,
          fusoHorario: v.fusoHorario || 'America/Sao_Paulo',
          corPrimaria: v.corPrimaria || null,
          logoUrl: v.logoUrl || null,
          adminNomeCompleto: v.adminNomeCompleto,
          adminEmail: v.adminEmail,
          adminSenha: v.adminSenha,
        })
      : this.servico.alterar(this.id!, {
          razaoSocial: v.razaoSocial,
          plano: v.plano,
          status: v.status,
          logoUrl: v.logoUrl || null,
          corPrimaria: v.corPrimaria || null,
          fusoHorario: v.fusoHorario || 'America/Sao_Paulo',
        });

    obs.subscribe({
      next: () => this.router.navigateByUrl('/configuracao/tenants'),
      error: (e) => {
        this.salvando.set(false);
        this.errorMsg.set(e?.error?.message ?? 'Falha ao salvar.');
      },
    });
  }

  voltar(): void {
    this.router.navigateByUrl('/configuracao/tenants');
  }
}
