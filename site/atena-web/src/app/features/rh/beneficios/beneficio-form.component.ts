import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { of } from 'rxjs';
import { CampoFormulario, CrudFormComponent } from '@shared/crud/crud-form.component';
import { BeneficioCatalogoService } from '../rh.services';

@Component({
  selector: 'app-beneficio-form',
  standalone: true,
  imports: [CrudFormComponent],
  template: `<app-crud-form [titulo]="'Benefício'" [campos]="campos" [servico]="servico" [rotaLista]="'/rh/beneficios'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class BeneficioFormComponent {
  readonly servico = inject(BeneficioCatalogoService);
  readonly campos: CampoFormulario[] = [
    { nome: 'codigo', rotulo: 'Código' },
    { nome: 'descricao', rotulo: 'Descrição', obrigatorio: true },
    {
      nome: 'tipo', rotulo: 'Tipo', tipo: 'select', obrigatorio: true,
      opcoes: () => of(['ValeTransporte', 'ValeRefeicao', 'ValeAlimentacao', 'PlanoSaude', 'PlanoOdonto',
                        'AuxilioCreche', 'SeguroVida', 'AuxilioHomeOffice', 'GymPass', 'Outro']
        .map((v) => ({ value: v, label: v }))),
    },
    { nome: 'descontoFuncionarioPct', rotulo: 'Desconto do funcionário (%)', tipo: 'number' },
    { nome: 'custoEmpresaPadrao', rotulo: 'Custo padrão para a empresa', tipo: 'number' },
    { nome: 'naturezaRubricaEsocial', rotulo: 'Natureza rubrica eSocial' },
    { nome: 'ativo', rotulo: 'Ativo', tipo: 'checkbox' },
  ];
}
