import {
  Directive,
  EmbeddedViewRef,
  Input,
  TemplateRef,
  ViewContainerRef,
  effect,
  inject,
} from '@angular/core';
import { AuthStore } from '@core/auth/auth.store';

@Directive({
  selector: '[temPermissao]',
  standalone: true,
})
export class TemPermissaoDirective {
  private readonly tpl = inject(TemplateRef<unknown>);
  private readonly vcr = inject(ViewContainerRef);
  private readonly auth = inject(AuthStore);

  private required: string[] = [];
  private viewRef: EmbeddedViewRef<unknown> | null = null;

  @Input({ required: true })
  set temPermissao(value: string | string[]) {
    this.required = Array.isArray(value) ? value : [value];
    this.update();
  }

  constructor() {
    effect(() => {
      this.auth.permissions();
      this.update();
    });
  }

  private update(): void {
    const ok = this.required.length === 0 || this.auth.hasAnyPermission(this.required);
    if (ok && !this.viewRef) {
      this.viewRef = this.vcr.createEmbeddedView(this.tpl);
    } else if (!ok && this.viewRef) {
      this.vcr.clear();
      this.viewRef = null;
    }
  }
}
