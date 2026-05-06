import { ChangeDetectionStrategy, Component, OnInit, inject } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { AuthStore } from '@core/auth/auth.store';
import { TenantBrandingService } from '@core/branding/tenant-branding.service';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  template: `<router-outlet />`,
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AppComponent implements OnInit {
  private readonly auth = inject(AuthStore);
  private readonly branding = inject(TenantBrandingService);

  ngOnInit(): void {
    this.auth.bootstrap();
    if (this.auth.isAuthenticated()) {
      this.branding.load();
    }
  }
}
