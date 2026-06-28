import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { Router } from '@angular/router';
import { CrudListComponent } from '@shared/crud/crud-list.component';
import { ColunaTabela } from '@shared/data-table/data-table.component';
import { FuncionarioService, Funcionario } from '@features/cadastros/cadastros.services';

/**
 * Lista de funcionários no módulo RH. Reusa o FuncionarioService existente do módulo
 * Cadastros (mesma rota /api/v1/funcionarios) — em W2/W3 esta lista evoluirá para usar
 * filtros por cargoId/departamentoId/lotacaoId/status. Por ora replica a lista de cadastros
 * com rotaForm apontando para a Ficha do RH.
 */
@Component({
  selector: 'app-funcionario-rh-list',
  standalone: true,
  imports: [CrudListComponent],
  template: `
    <div class="d-flex justify-content-end mb-2">
      <button class="btn btn-success btn-sm" (click)="novoFuncionario()">+ Novo funcionário (wizard)</button>
    </div>
    <app-crud-list
      [titulo]="'Funcionários'"
      [colunas]="colunas"
      [servico]="servico"
      [rotaForm]="'/rh/funcionarios'" />
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FuncionarioRhListComponent {
  readonly servico = inject(FuncionarioService);
  private readonly router = inject(Router);

  readonly colunas: ColunaTabela<Funcionario>[] = [
    { campo: 'nomeCompleto', titulo: 'Nome' },
    { campo: 'cpf', titulo: 'CPF' },
    { campo: 'cargo', titulo: 'Cargo' },
    { campo: 'departamento', titulo: 'Departamento' },
    { campo: 'dataAdmissao', titulo: 'Admissão', tipo: 'data' },
  ];

  novoFuncionario(): void {
    this.router.navigate(['/rh/funcionarios/novo']);
  }
}
