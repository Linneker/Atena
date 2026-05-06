import { Injectable, inject, signal } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { tap } from 'rxjs';
import { environment } from '@env/environment';

export interface TenantBranding {
  tenantId: string;
  razaoSocial: string;
  logoUrl: string | null;
  corPrimaria: string;
  corSecundaria: string;
  corAccent: string;
}

const DEFAULT_BRANDING: Omit<TenantBranding, 'tenantId' | 'razaoSocial'> = {
  logoUrl: null,
  corPrimaria: '#321fdb',
  corSecundaria: '#3c4b64',
  corAccent: '#2eb85c',
};

@Injectable({ providedIn: 'root' })
export class TenantBrandingService {
  private readonly http = inject(HttpClient);
  private readonly brandingSig = signal<TenantBranding | null>(null);

  readonly branding = this.brandingSig.asReadonly();

  load(): void {
    this.http
      .get<TenantBranding>(`${environment.apiUrl}/${environment.apiVersion}/tenants/me/branding`)
      .pipe(tap((b) => this.apply(b)))
      .subscribe({
        error: () => this.applyDefaults(),
      });
  }

  private apply(branding: TenantBranding): void {
    this.brandingSig.set(branding);
    const root = document.documentElement;
    root.style.setProperty('--tenant-primary', branding.corPrimaria || DEFAULT_BRANDING.corPrimaria);
    root.style.setProperty('--tenant-secondary', branding.corSecundaria || DEFAULT_BRANDING.corSecundaria);
    root.style.setProperty('--tenant-accent', branding.corAccent || DEFAULT_BRANDING.corAccent);
  }

  private applyDefaults(): void {
    const root = document.documentElement;
    root.style.setProperty('--tenant-primary', DEFAULT_BRANDING.corPrimaria);
    root.style.setProperty('--tenant-secondary', DEFAULT_BRANDING.corSecundaria);
    root.style.setProperty('--tenant-accent', DEFAULT_BRANDING.corAccent);
  }
}
