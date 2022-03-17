import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../auth/auth-guard.service';

const routes: Routes = [
  {
      path: '',
      data: {
        title: 'Admin'
      },
      children: [
      {
        path: 'product',
        loadChildren: () => import('./product/product.module').then(m => m.ProductModule)
      }
    ],
    canActivate: [AuthGuard]
  }];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]

})
export class AdminRoutingModule { }
