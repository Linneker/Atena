import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudListComponent } from '@shared/crud/crud-list.component';
import { ColunaTabela } from '@shared/data-table/data-table.component';
import { BeneficioCatalogo, BeneficioCatalogoService } from '../rh.services';

@Component({
  selector: 'app-beneficio-list',
  standalone: true,
  imports: [CrudListComponent],
  template: `<app-crud-list [titulo]="'Catálogo de benefícios'" [colunas]="colunas" [servico]="servico" [rotaForm]="'/rh/beneficios'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BeneficioListComponent {
  readonly servico = inject(BeneficioCatalogoService);
  readonly colunas: ColunaTabela<BeneficioCatalogo>[] = [
    { campo: 'codigo', titulo: 'Código' },
    { campo: 'descricao', titulo: 'Descrição' },
    { campo: 'tipo', titulo: 'Tipo' },
    { campo: 'custoEmpresaPadrao', titulo: 'Custo empresa', tipo: 'moeda' },
    { campo: 'descontoFuncionarioPct', titulo: 'Desconto func. (%)' },
    { campo: 'ativo', titulo: 'Ativo', formato: (b) => (b.ativo ? 'Sim' : 'Não') },
  ];
}
