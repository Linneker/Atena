import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CrudFormComponent, CampoFormulario } from '@shared/crud/crud-form.component';
import { InventarioService } from '../estoque.services';

@Component({
  selector: 'app-inventario-form',
  standalone: true,
  imports: [CrudFormComponent],
  template: `
    <app-crud-form [titulo]="'Inventário'" [campos]="campos" [servico]="servico" [rotaLista]="'/estoque/inventario'" />
    @if (id) {
      <button class="btn btn-danger mt-3" (click)="fechar()">Fechar Inventário</button>
    }
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class InventarioFormComponent {
  readonly servico = inject(InventarioService);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);
  readonly id = this.route.snapshot.paramMap.get('id');

  readonly campos: CampoFormulario[] = [
    { nome: 'descricao', rotulo: 'Descrição', obrigatorio: true },
    { nome: 'status', rotulo: 'Status' },
  ];

  fechar(): void {
    if (!this.id || this.id === 'novo') return;
    this.servico.fechar(this.id).subscribe(() => this.router.navigateByUrl('/estoque/inventario'));
  }
}
