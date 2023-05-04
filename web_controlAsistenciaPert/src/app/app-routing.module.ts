import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './guards/auth.guard';
import { HomeComponent } from './home/home.component';

const router:Routes = [
  {
     path : '', component : HomeComponent , canActivate: [ AuthGuard]
  },
  {
    path : 'Autentificacion',
    loadChildren :()=> import('./autentificacion/autentificacion.module').then(m => m.AutentificacionModule )
  },
  {
    path : 'Mantenimiento',
    loadChildren :()=> import('./mantenimientos/mantenimientos.module').then(m => m.MantenimientosModule )
  },
  {
    path : 'Procesos',
    loadChildren :()=> import('./procesos/procesos.module').then(m => m.ProcesosModule )
  },  
  {
    path : 'Reportes',
    loadChildren :()=> import('./reportes/reportes.module').then(m => m.ReportesModule )
  }, 
  {
    path : 'Seguridad',
    loadChildren :()=> import('./seguridad/seguridad.module').then(m => m.SeguridadModule )
  },   
   { 
    path: '**', redirectTo: '' 
  }

];

@NgModule({
  imports: [
    RouterModule.forRoot(router,{useHash:true})
 ],
 exports : [
   RouterModule
 ]
})
export class AppRoutingModule { }

