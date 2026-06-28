import { ChangeDetectionStrategy, Component, OnInit, inject, signal } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { DatePipe } from '@angular/common';
import { Tenant, TenantsService } from './tenants.service';

@Component({
  selector: 'app-tenant-list',
  standalone: true,
  imports: [RouterLink, FormsModule, DatePipe],
  template: `
    <div class="d-flex justify-content-between align-items-center mb-3">
      <h3 class="m-0">Tenants</h3>
      <a class="btn btn-primary btn-sm" routerLink="/configuracao/tenants/novo">Novo tenant</a>
    </div>
    <div class="d-flex mb-2">
      <input class="form-control form-control-sm me-2" placeholder="Buscar por razão social / CNPJ..."
             [(ngModel)]="termoBusca" (ngModelChange)="onBusca()" />
    </div>
    <div class="table-responsive">
      <table class="table table-sm table-hover">
        <thead class="table-light">
          <tr>
            <th>Razão Social</th>
            <th>CNPJ</th>
            <th>Plano</th>
            <th>Status</th>
            <th>Criado em</th>
            <th class="text-end">Ações</th>
          </tr>
        </thead>
        <tbody>
          @for (t of itens(); track t.id) {
            <tr>
              <td>{{ t.razaoSocial }}</td>
              <td>{{ t.cnpj }}</td>
              <td>{{ t.plano }}</td>
              <td>{{ rotuloStatus(t.status) }}</td>
              <td>{{ t.createdAt | date:'short' }}</td>
              <td class="text-end">
                <a class="btn btn-sm btn-link" [routerLink]="['/configuracao/tenants', t.id]">Editar</a>
                <button class="btn btn-sm btn-link text-danger" (click)="remover(t)">Excluir</button>
              </td>
            </tr>
          } @empty {
            <tr><td colspan="6" class="text-center text-muted">Nenhum tenant cadastrado.</td></tr>
          }
        </tbody>
      </table>
    </div>
    <small>Total: {{ total() }}</small>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class TenantListComponent implements OnInit {
  private readonly servico = inject(TenantsService);
  private readonly router = inject(Router);

  readonly itens = signal<Tenant[]>([]);
  readonly total = signal(0);
  termoBusca = '';

  private debounceHandle: ReturnType<typeof setTimeout> | null = null;

  ngOnInit(): void {
    this.recarregar();
  }

  onBusca(): void {
    if (this.debounceHandle) clearTimeout(this.debounceHandle);
    this.debounceHandle = setTimeout(() => this.recarregar(), 300);
  }

  remover(t: Tenant): void {
    if (!confirm(`Excluir o tenant "${t.razaoSocial}"?`)) return;
    this.servico.excluir(t.id).subscribe({
      next: () => this.recarregar(),
      error: () => alert('Falha ao excluir.'),
    });
  }

  rotuloStatus(s: number): string {
    return s === 1 ? 'Ativo' : s === 2 ? 'Suspenso' : s === 3 ? 'Pendente' : 'Inativo';
  }

  private recarregar(): void {
    this.servico.listar(this.termoBusca).subscribe((r) => {
      this.itens.set(r.itens);
      this.total.set(r.total);
    });
  }
}
