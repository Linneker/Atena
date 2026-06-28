import { ChangeDetectionStrategy, Component, Input, OnInit, inject, signal } from '@angular/core';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { CrudService } from './crud.service';

export interface OpcaoSelect {
  value: string | number | null;
  label: string;
}

export interface CampoFormulario {
  nome: string;
  rotulo: string;
  tipo?: 'text' | 'number' | 'date' | 'email' | 'select' | 'checkbox';
  obrigatorio?: boolean;
  /** Para tipo 'select'. Recebe Observable porque normalmente vem de HTTP. */
  opcoes?: () => Observable<OpcaoSelect[]>;
  /** Placeholder do select quando nenhuma opção está selecionada. */
  placeholderSelect?: string;
  /** Texto auxiliar exibido abaixo do campo. */
  ajuda?: string;
}

@Component({
  selector: 'app-crud-form',
  standalone: true,
  imports: [ReactiveFormsModule],
  template: `
    <h3 class="mb-3">{{ titulo }}</h3>
    <form [formGroup]="form" (ngSubmit)="salvar()" class="row g-3" novalidate>
      @for (c of campos; track c.nome) {
        <div class="col-md-6">
          @if (c.tipo === 'checkbox') {
            <div class="form-check mt-4">
              <input class="form-check-input" type="checkbox" [id]="'cf-' + c.nome" [formControlName]="c.nome" />
              <label class="form-check-label" [for]="'cf-' + c.nome">{{ c.rotulo }}</label>
            </div>
            @if (c.ajuda) {
              <small class="text-muted d-block">{{ c.ajuda }}</small>
            }
          } @else {
            <label class="form-label">{{ c.rotulo }}{{ c.obrigatorio ? ' *' : '' }}</label>
            @if (c.tipo === 'select') {
              <select class="form-select" [formControlName]="c.nome">
                <option [ngValue]="null">{{ c.placeholderSelect ?? '— selecione —' }}</option>
                @for (op of opcoesPorCampo()[c.nome] ?? []; track op.value) {
                  <option [ngValue]="op.value">{{ op.label }}</option>
                }
              </select>
            } @else {
              <input class="form-control" [type]="c.tipo ?? 'text'" [formControlName]="c.nome" />
            }
            @if (c.ajuda) {
              <small class="text-muted d-block">{{ c.ajuda }}</small>
            }
          }
        </div>
      }
      <div class="col-12 d-flex gap-2">
        <button type="submit" class="btn btn-primary" [disabled]="form.invalid || salvando()">Salvar</button>
        <button type="button" class="btn btn-secondary" (click)="voltar()">Cancelar</button>
      </div>
    </form>
  `,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class CrudFormComponent<T extends { id?: string }> implements OnInit {
  @Input({ required: true }) titulo = '';
  @Input({ required: true }) campos: CampoFormulario[] = [];
  @Input({ required: true }) servico!: CrudService<T>;
  @Input({ required: true }) rotaLista = '';

  private readonly fb = inject(FormBuilder);
  private readonly route = inject(ActivatedRoute);
  private readonly router = inject(Router);

  readonly salvando = signal(false);
  readonly opcoesPorCampo = signal<Record<string, OpcaoSelect[]>>({});
  form: FormGroup = this.fb.group({});
  private id: string | null = null;

  ngOnInit(): void {
    const controls: Record<string, unknown[]> = {};
    for (const c of this.campos) {
      let valorInicial: unknown = '';
      if (c.tipo === 'select') valorInicial = null;
      else if (c.tipo === 'checkbox') valorInicial = false;
      controls[c.nome] = [valorInicial, c.obrigatorio ? [Validators.required] : []];
    }
    this.form = this.fb.group(controls);

    for (const c of this.campos) {
      if (c.tipo === 'select' && c.opcoes) {
        c.opcoes().subscribe((lista) => {
          this.opcoesPorCampo.update((m) => ({ ...m, [c.nome]: lista }));
        });
      }
    }

    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam && idParam !== 'novo') {
      this.id = idParam;
      this.servico.obter(idParam).subscribe((r) => this.form.patchValue(r as object));
    }
  }

  salvar(): void {
    if (this.form.invalid) return;
    this.salvando.set(true);
    const op = this.id ? this.servico.alterar(this.id, this.form.value) : this.servico.criar(this.form.value);
    op.subscribe({
      next: () => this.router.navigateByUrl(this.rotaLista),
      complete: () => this.salvando.set(false),
      error: () => this.salvando.set(false),
    });
  }

  voltar(): void {
    this.router.navigateByUrl(this.rotaLista);
  }
}
