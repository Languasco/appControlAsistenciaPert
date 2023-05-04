import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../guards/auth.guard';
import { PagesComponent } from './pages/pages.component';
import { RegistroAsistenciaComponent } from './pages/registro-asistencia/registro-asistencia.component';
 
const routes: Routes = [
  {
    path : '', component : PagesComponent,
    children : [
      { path: 'RegistroAsistencia', component: RegistroAsistenciaComponent, canActivate: [ AuthGuard] },
      // { path: 'TipoAsistencia', component: TipoAsistenciaComponent, canActivate: [ AuthGuard] },
      // { path: 'HorarioPersonal', component: HorarioPersonalComponent, canActivate: [ AuthGuard] },      
      { path: '**', redirectTo: 'home' }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ProcesosRoutingModule { }

