import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { CrudListComponent } from '@shared/crud/crud-list.component';
import { ColunaTabela } from '@shared/data-table/data-table.component';
import { Receita, ReceitaService } from '../financeiro.services';

@Component({
  selector: 'app-receita-list',
  standalone: true,
  imports: [CrudListComponent],
  template: `<app-crud-list [titulo]="'Receitas'" [colunas]="colunas" [servico]="servico" [rotaForm]="'/financeiro/receitas'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ReceitaListComponent {
  readonly servico = inject(ReceitaService);
  readonly colunas: ColunaTabela<Receita>[] = [
    { campo: 'descricao', titulo: 'Descrição' },
    { campo: 'valor', titulo: 'Valor', formato: (l) => l.valor.toFixed(2) },
    { campo: 'vencimento', titulo: 'Vencimento' },
    { campo: 'status', titulo: 'Status' },
  ];
}
