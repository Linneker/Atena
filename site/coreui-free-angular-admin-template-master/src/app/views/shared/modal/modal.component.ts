import { Router } from '@angular/router';
import { StringToHtmlPipe } from './../../pipe/html/string-to-html.pipe';
import { BsModalService, ModalDirective } from 'ngx-bootstrap/modal';
import { Component, OnInit, ViewChild, Input } from '@angular/core';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.scss'],
})
export class ModalComponent implements OnInit {
  @ViewChild('largeModal') public largeModal: ModalDirective;

  @Input() mensagem : string = "";
  @Input() tipo: string ="success";
  @Input() redirectTo: string = "/";
  @Input() abrindoDentroDeUmModal: boolean =true;

  constructor(private modalService: BsModalService,private router: Router) { }

  ngOnInit(): void {
  }

  redirect(evento){
  this.modalService.hide();
  if(this.redirectTo!="")
      this.router.navigate([this.redirectTo])
  }
  ngOnDestroy()
  {
    if(this.abrindoDentroDeUmModal)
     document.querySelector("body").classList.add("modal-open");
  }
}
