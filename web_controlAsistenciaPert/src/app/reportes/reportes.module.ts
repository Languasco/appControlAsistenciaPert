import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ComponentsModule } from '../components/components.module';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Ng2SearchPipeModule } from 'ng2-search-filter';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { NgSelect2Module } from 'ng-select2';
 
//----- fechas datetimePicker ---
import { BsDatepickerModule, BsLocaleService } from 'ngx-bootstrap/datepicker';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { esLocale } from 'ngx-bootstrap/locale';
defineLocale('es', esLocale);

import { ReportesRoutingModule } from './reportes-routing.module';
import { PagesComponent } from './pages/pages.component';
import { TareoPersonalComponent } from './pages/tareo-personal/tareo-personal.component';
import { TareoDiarioComponent } from './pages/tareo-diario/tareo-diario.component';
import { AsistenciaCampoComponent } from './pages/asistencia-campo/asistencia-campo.component';
import { LectorHuellasComponent } from './pages/lector-huellas/lector-huellas.component';

@NgModule({
  declarations: [
    PagesComponent,
    TareoPersonalComponent,
    TareoDiarioComponent,
    AsistenciaCampoComponent,
    LectorHuellasComponent
  ],
  imports: [
    CommonModule,
    ReportesRoutingModule,
     ComponentsModule,
    FormsModule,
    ReactiveFormsModule,
    Ng2SearchPipeModule,
    TooltipModule.forRoot(),
    NgSelect2Module,
    BsDatepickerModule.forRoot(),
  ]
})
export class ReportesModule { 
        ///---definiendo la fecha Español global --
        constructor( private bsLocaleService: BsLocaleService){
          this.bsLocaleService.use('es');//fecha en español, datepicker
        }
        ///--- Fin de definiendo la fecha Español global --
}
