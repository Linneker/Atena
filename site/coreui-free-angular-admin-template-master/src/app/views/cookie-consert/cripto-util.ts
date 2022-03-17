import { environment } from './../../../environments/environment.prod';
import * as CryptoJS from 'crypto-js';


export class CriptoUtil  {


   public Criptografa(elemento: string){

    var key = CryptoJS.enc.Utf8.parse(environment.keySync);
    var iv = CryptoJS.enc.Utf8.parse(environment.keySync);

    var encrypted = new CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(elemento), key,
        {
            keySize: 128 / 8,
            iv: iv,
            mode: CryptoJS.mode.CBC,
            padding: CryptoJS.pad.Pkcs7
        });
        return encrypted;
    }
    public CriptografaList(elemento: string[]){

      var key = CryptoJS.enc.Utf8.parse(environment.keySync);
      var iv = CryptoJS.enc.Utf8.parse(environment.keySync);

      var encrypted = new CryptoJS.AES.encrypt(CryptoJS.enc.Utf8.parse(elemento), key,
          {
              keySize: 128 / 8,
              iv: iv,
              mode: CryptoJS.mode.CBC,
              padding: CryptoJS.pad.Pkcs7
          });
          return encrypted;
    }

    public Descriptografa(encrypted: string){

      var key = CryptoJS.enc.Utf8.parse(environment.keySync);
      var iv = CryptoJS.enc.Utf8.parse(environment.keySync);
      var decrypted = CryptoJS.AES.decrypt(encrypted, key, {
        keySize: 128 / 8,
        iv: iv,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
      });
      return decrypted.toString(CryptoJS.enc.Utf8);
    }
    public DescriptografaList(encrypted: string[]){

      var key = CryptoJS.enc.Utf8.parse(environment.keySync);
      var iv = CryptoJS.enc.Utf8.parse(environment.keySync);
      var decrypted = CryptoJS.AES.decrypt(encrypted, key, {
        keySize: 128 / 8,
        iv: iv,
        mode: CryptoJS.mode.CBC,
        padding: CryptoJS.pad.Pkcs7
      });
      return decrypted;
    }

   // public criptAssimetric(data: string)
  //  {
   //   let forge = require('node-forge');
   //   let rsa = forge.pki.publicKeyFromPem("-----BEGIN PUBLIC KEY-----"+     "MIIBI..."+     "-----END PUBLIC KEY-----");
   //   return window.btoa(rsa.encrypt(data));

   // }
}
