import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StringToHtmlPipe } from './html/string-to-html.pipe';



@NgModule({
  declarations: [
    StringToHtmlPipe
  ],
  imports: [
    CommonModule
  ],
  exports: [
    StringToHtmlPipe
  ]
})
export class PipeModule { }
