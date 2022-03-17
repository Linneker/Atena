import { NgModule } from "@angular/core";
import { RouterModule, Routes } from "@angular/router";
import { ProdutoComponent } from "./produto/produto.component";
import { TipoProdutoComponent } from "./tipo-produto/tipo-produto.component";
import { TipoValorProdutoComponent } from "./tipo-valor-produto/tipo-valor-produto.component";

const routes: Routes = [
  {
    path: '',
    data: {
      title: 'Configuração Produto'
    },
    children: [
      {
        path: '',
        redirectTo: 'product',
        pathMatch: 'full'
      },
      {
        path: 'produto',
        component: ProdutoComponent,
        data: {
          title: 'Produto'
        }
      },
      {
        path: 'tipo-produto',
        component: TipoProdutoComponent,
        data: {
          title: 'Tipo Produto'
        }
      },
      {
        path: 'tipo-valor-produto',
        component: TipoValorProdutoComponent,
        data: {
          title: 'Tipo Produto Produto'
        }
      }
    ]
  }
];
@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]

})
export class ProductRoutingModule { }
