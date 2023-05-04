import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../guards/auth.guard';
import { HorarioPersonalComponent } from './pages/horario-personal/horario-personal.component';
import { PagesComponent } from './pages/pages.component';
import { TipoAsistenciaComponent } from './pages/tipo-asistencia/tipo-asistencia.component';
import { TurnosComponent } from './pages/turnos/turnos.component';
 

const routes: Routes = [
  {
    path : '', component : PagesComponent,
    children : [
        { path: 'Turnos', component: TurnosComponent, canActivate: [ AuthGuard] },
       { path: 'TipoAsistencia', component: TipoAsistenciaComponent, canActivate: [ AuthGuard] },
       { path: 'HorarioPersonal', component: HorarioPersonalComponent, canActivate: [ AuthGuard] },      
      { path: '**', redirectTo: 'home' }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class MantenimientosRoutingModule { }
