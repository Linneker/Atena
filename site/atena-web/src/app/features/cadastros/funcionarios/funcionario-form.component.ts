import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { map } from 'rxjs';
import { CrudFormComponent, CampoFormulario } from '@shared/crud/crud-form.component';
import { CentroCustoService, FuncionarioService } from '../cadastros.services';

@Component({
  selector: 'app-funcionario-form',
  standalone: true,
  imports: [CrudFormComponent],
  template: `<app-crud-form [titulo]="'Funcionário'" [campos]="campos" [servico]="servico" [rotaLista]="'/cadastros/funcionarios'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class FuncionarioFormComponent {
  readonly servico = inject(FuncionarioService);
  private readonly centros = inject(CentroCustoService);
  readonly campos: CampoFormulario[] = [
    { nome: 'nomeCompleto', rotulo: 'Nome completo', obrigatorio: true },
    { nome: 'cpf', rotulo: 'CPF (somente números)', obrigatorio: true },
    { nome: 'email', rotulo: 'E-mail', tipo: 'email' },
    { nome: 'telefone', rotulo: 'Telefone' },
    { nome: 'cargo', rotulo: 'Cargo' },
    { nome: 'departamento', rotulo: 'Departamento' },
    { nome: 'dataAdmissao', rotulo: 'Data de admissão', tipo: 'date' },
    {
      nome: 'centroDeCustoId',
      rotulo: 'Centro de Custo',
      tipo: 'select',
      placeholderSelect: '— sem centro de custo —',
      opcoes: () => this.centros.listar({ pagina: 1, tamanhoPagina: 100 }).pipe(
        map((p) => p.itens.map((c) => ({ value: c.id ?? '', label: `${c.codigo} — ${c.nome}` }))),
      ),
    },
  ];
}
