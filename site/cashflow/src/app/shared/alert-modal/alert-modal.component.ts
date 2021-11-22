import { BsModalService } from 'ngx-bootstrap/modal';
import {Router} from "@angular/router"
import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'app-alert-modal',
  templateUrl: './alert-modal.component.html',
  styleUrls: ['./alert-modal.component.css']
})
export class AlertModalComponent implements OnInit {

  @Input() mensagem : string = "";
  @Input() tipo: string ="success";
  @Input() redirectTo: string = "/";
  constructor(private modalService: BsModalService,private router: Router) { }

  ngOnInit(): void {
  }

  redirect(){
    this.modalService.hide();
    this.router.navigate([this.redirectTo])

  }
}
