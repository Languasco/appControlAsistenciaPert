import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ComponentsModule } from '../components/components.module';
import { RouterModule, Routes } from '@angular/router';
import { PagesComponent } from './pages/pages.component';
import { AccesosComponent } from './pages/accesos/accesos.component';
import { AuthGuard } from '../guards/auth.guard';

const routes: Routes = [
  {
    path : '', component : PagesComponent,
    children : [
      { path: 'Acceso', component: AccesosComponent, canActivate: [ AuthGuard] },
      { path: '**', redirectTo: 'home' }
    ]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class SeguridadRoutingModule { }
