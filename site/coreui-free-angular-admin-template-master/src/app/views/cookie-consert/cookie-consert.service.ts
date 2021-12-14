import { Component, OnInit, Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';


@Injectable({
  providedIn: 'root'
})
export class CookieConsertService {


  private cookie_name='';
  private all_cookies:any='';

 constructor(private cookieService:CookieService){

 }
  getCookie(key: string){
    return this.cookieService.get(key);
  }


  getAllCookie(key: string){
    return this.cookieService.getAll();
  }

 setObjectCookie(key: string,  valor:any){
   this.cookieService.set(key,valor);
 }
 setCookie(key: string,  valor:any){
  this.cookieService.set(key,valor);
}

 deleteCookie(key: string){
   this.cookieService.remove(key);
 }

 deleteAll(){
   this.cookieService.removeAll();
 }

}
