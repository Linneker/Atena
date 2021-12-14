import { Component, OnInit } from '@angular/core';
import { CookieService, COOKIE_OPTIONS } from 'ngx-cookie';


@Component({
  selector: 'app-cookie-consert',
  templateUrl: './cookie-consert.component.html',
  styleUrls: ['./cookie-consert.component.scss']
})
export class CookieConsertComponent implements OnInit {


  private cookie_name='';
  private all_cookies:any='';

 constructor(private cookieService:CookieService){

 }
  getCookie(key: string){
    return this.cookieService.get(key);
  }

  getObjectCookie(key: string){
    return this.cookieService.getObject(key);
  }

  getAllCookie(key: string){
    return this.cookieService.getAll();
  }

 setObjectCookie(key: string,  valor:any){
   this.cookieService.putObject(key,valor);
 }
 setCookie(key: string,  valor:any){
  this.cookieService.put(key,valor);
}

 deleteCookie(key: string){
   this.cookieService.remove(key);
 }

 deleteAll(){
   this.cookieService.removeAll();
 }

  ngOnInit(): void {

  }
}
