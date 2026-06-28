import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { of } from 'rxjs';
import { CampoFormulario, CrudFormComponent } from '@shared/crud/crud-form.component';
import { JornadaService } from '../rh.services';

@Component({
  selector: 'app-jornada-form',
  standalone: true,
  imports: [CrudFormComponent],
  template: `<app-crud-form [titulo]="'Jornada'" [campos]="campos" [servico]="servico" [rotaLista]="'/rh/jornadas'" />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class JornadaFormComponent {
  readonly servico = inject(JornadaService);
  readonly campos: CampoFormulario[] = [
    { nome: 'nome', rotulo: 'Nome', obrigatorio: true },
    {
      nome: 'tipo', rotulo: 'Tipo', tipo: 'select', obrigatorio: true,
      opcoes: () => of(['Fixa', 'Flexivel', 'Escala12x36', 'EscalaPersonalizada', 'Banco']
        .map((v) => ({ value: v, label: v }))),
    },
    { nome: 'cargaSemanalHoras', rotulo: 'Carga semanal (horas)', tipo: 'number', obrigatorio: true },
    { nome: 'cargaDiariaHoras', rotulo: 'Carga diária (horas)', tipo: 'number' },
    { nome: 'janelasJson', rotulo: 'Janelas (JSON)', obrigatorio: true,
      ajuda: 'Ex.: [{"dia":"seg","entrada":"08:00","saida":"12:00"}]' },
    { nome: 'permiteMarcarIntervalo', rotulo: 'Permite marcar intervalo', tipo: 'checkbox' },
    { nome: 'toleranciaMinutos', rotulo: 'Tolerância (minutos)', tipo: 'number' },
    { nome: 'ativo', rotulo: 'Ativa', tipo: 'checkbox' },
  ];
}
