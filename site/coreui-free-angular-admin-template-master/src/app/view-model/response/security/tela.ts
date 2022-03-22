import { AbstractEntity } from "../abstract-entity";
import { PermissaoUsuario } from "./permissao-usuario";

export class Tela extends AbstractEntity{
    name:string ;
    url:string;
    icon:string;
    title:boolean ;
    isPrincipal :boolean;
    class:string  ;
    variant:string  ;

    telaSistemaFilhaId:string;

    telaSistemaFilha:Tela;

    telasSistemasFilhas:Tela[] = [];
    permissaoUsuarios:PermissaoUsuario[]  = [];

}
