import { CriptoUtil } from './cripto-util';
import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';


@Injectable({
  providedIn: 'root'
})
export class CookieConsertService {


  private cookie_name='';
  private all_cookies:any='';
  private cripto: CriptoUtil = new CriptoUtil();

 constructor(private cookieService:CookieService ){

 }
  getCookie(key: string){
    return this.cripto.Descriptografa(this.cookieService.get(key));
  }


  getAllCookie(key: string){
    return this.cookieService.getAll();
  }

 setObjectCookie(key: string,  valor:any){
   this.cookieService.set(key, this.cripto.Criptografa(valor));
 }
 setCookie(key: string,  valor:any){
  this.cookieService.set(key,this.cripto.Criptografa(valor));
}

 deleteCookie(key: string){
   this.cookieService.delete(key);
 }

 deleteAll(){
   this.cookieService.deleteAll();
 }

}
