import { AuthGuard } from './views/auth/auth-guard.service';
import { PipeModule } from './views/pipe/pipe.module';
import { ModalModule } from 'ngx-bootstrap/modal';
import { TokenInterceptorService } from './views/auth/token-interceptor.service';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { LocationStrategy, HashLocationStrategy } from '@angular/common';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';

import { PerfectScrollbarModule } from 'ngx-perfect-scrollbar';
import { PERFECT_SCROLLBAR_CONFIG } from 'ngx-perfect-scrollbar';
import { PerfectScrollbarConfigInterface } from 'ngx-perfect-scrollbar';

import { IconModule, IconSetModule, IconSetService } from '@coreui/icons-angular';

const DEFAULT_PERFECT_SCROLLBAR_CONFIG: PerfectScrollbarConfigInterface = {
  suppressScrollX: true
};

import { AppComponent } from './app.component';

// Import containers
import { DefaultLayoutComponent } from './containers';

import { P404Component } from './views/error/404.component';
import { P500Component } from './views/error/500.component';
import { RegisterComponent } from './views/register/register.component';

const APP_CONTAINERS = [
  DefaultLayoutComponent
];


// Import routing module
import { AppRoutingModule } from './app.routing';

// Import 3rd party components
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { ChartsModule } from 'ng2-charts';
import { UsuarioModule } from './views/usuario/usuario.module';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { CookieConsertService } from './views/cookie-consert/cookie-consert.service';
import { CookieService } from 'ngx-cookie-service';
import { AppAsideModule, AppBreadcrumbModule, AppFooterModule, AppHeaderModule, AppSidebarModule } from '@coreui/angular';
import { SharedModule } from './views/shared/shared.module';
import { TipoProdutoComponent } from './views/admin/product/tipo-produto/tipo-produto.component';
import { AdminModule } from './views/admin/admin.module';

@NgModule({
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    AppRoutingModule,
    AppAsideModule,
    UsuarioModule,
    AppBreadcrumbModule.forRoot(),
    AppFooterModule,
    AppHeaderModule,
    AppSidebarModule,
    PerfectScrollbarModule,
    BsDropdownModule.forRoot(),
    TabsModule.forRoot(),
    ChartsModule,
    IconModule,
    IconSetModule.forRoot(),
    HttpClientModule,
    ModalModule.forRoot(),
    PipeModule,
    SharedModule,
    AdminModule
  ],
  declarations: [
    AppComponent,
    ...APP_CONTAINERS,
    P404Component,
    P500Component,
    RegisterComponent
  ],
  providers: [
    CookieService,
    CookieConsertService,
    TokenInterceptorService,
    AuthGuard,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: TokenInterceptorService,
      multi: true,
    },
    {
      provide: LocationStrategy,
      useClass: HashLocationStrategy
    },
    IconSetService,
  ],
  bootstrap: [ AppComponent ]
})
export class AppModule { }
