import { NgModule } from "@angular/core";

@NgModule({
  declarations: [],
  imports: []
})
export class AutorizacaoApiResponse{
  authenticated: boolean=false;
  created: string = "";
  expiration:  string = "";
  accessToken: string = "";
  message: string = "OK";
}
