import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../guards/auth.guard';
import { PagesComponent } from './pages/pages.component';
import { TareoPersonalComponent } from './pages/tareo-personal/tareo-personal.component';
import { TareoDiarioComponent } from './pages/tareo-diario/tareo-diario.component';
import { AsistenciaCampoComponent } from './pages/asistencia-campo/asistencia-campo.component';
import { LectorHuellasComponent } from './pages/lector-huellas/lector-huellas.component';

const routes: Routes = [
  {
    path : '', component : PagesComponent,
    children : [
      { path: 'ReporteTareoPersonal', component: TareoPersonalComponent, canActivate: [ AuthGuard] },
      { path: 'ReporteTareoDiario', component: TareoDiarioComponent, canActivate: [ AuthGuard] },
      { path: 'AsistenciaCampo', component: AsistenciaCampoComponent, canActivate: [ AuthGuard] },      
      { path: 'LectorHuellas', component: LectorHuellasComponent, canActivate: [ AuthGuard] },   
      { path: '**', redirectTo: 'home' }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ReportesRoutingModule { }
