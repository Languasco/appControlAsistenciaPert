
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Ng2SearchPipeModule } from 'ng2-search-filter';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { NgSelect2Module } from 'ng-select2';
 
//----- fechas datetimePicker ---
import { BsDatepickerModule, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { esLocale } from 'ngx-bootstrap/locale';
defineLocale('es', esLocale);

import { PagesComponent } from './pages/pages.component';
import { ComponentsModule } from '../components/components.module';
import { ProcesosRoutingModule } from './procesos-routing.module'; 

import { RegistroAsistenciaComponent } from './pages/registro-asistencia/registro-asistencia.component';
import { AsignarPersonalOTComponent } from './pages/asignar-personal-ot/asignar-personal-ot.component';

@NgModule({
  declarations: [
    PagesComponent,
    RegistroAsistenciaComponent,
    AsignarPersonalOTComponent
  ],
  imports: [ 
    ProcesosRoutingModule,
    ComponentsModule,
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    Ng2SearchPipeModule,
    TooltipModule.forRoot(),
    NgSelect2Module,
    BsDatepickerModule.forRoot(),
  ]
})
export class ProcesosModule { 
      ///---definiendo la fecha Español global --
      constructor( private bsLocaleService: BsLocaleService){
        this.bsLocaleService.use('es');//fecha en español, datepicker
      }
      ///--- Fin de definiendo la fecha Español global --
}
 
