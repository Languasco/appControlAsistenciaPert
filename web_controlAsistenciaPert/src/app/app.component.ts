import { Component } from '@angular/core';
import { BsDatepickerConfig, BsLocaleService  } from 'ngx-bootstrap/datepicker';
 

declare var $:any;

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'webInspecciones';
  colorTheme = 'theme-green';

  datepiekerConfig:Partial<BsDatepickerConfig> 
 
  bsConfig: Partial<BsDatepickerConfig>;
  constructor(private localeService: BsLocaleService ) {
 
    this.datepiekerConfig = Object.assign({}, { containerClass : 'theme-orange',  dateInputFormat: 'DD/MM/YYYY' })  
 

    this.bsConfig = Object.assign({}, { containerClass: this.colorTheme });
   }

  nuevo(){
    setTimeout(()=>{ // 
      $('#staticBackdrop').modal('show');  
    },0); 
  } 

 

}


