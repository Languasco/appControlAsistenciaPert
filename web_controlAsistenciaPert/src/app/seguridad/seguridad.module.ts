import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ComponentsModule } from '../components/components.module';

import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { Ng2SearchPipeModule } from 'ng2-search-filter';
import { TooltipModule } from 'ngx-bootstrap/tooltip';
import { NgSelect2Module } from 'ng-select2';

// treeview Boostrap
import { TreeviewModule } from 'ngx-treeview';

import { SeguridadRoutingModule } from './seguridad-routing.module';
import { AccesosComponent } from './pages/accesos/accesos.component';
import { UsuariosComponent } from './pages/usuarios/usuarios.component';
import { PagesComponent } from './pages/pages.component';

@NgModule({
  declarations: [
    AccesosComponent,
    UsuariosComponent,
    PagesComponent
  ],
  imports: [
    CommonModule,
    SeguridadRoutingModule,
    ComponentsModule,
    FormsModule,
    ReactiveFormsModule,
    Ng2SearchPipeModule,
    TooltipModule.forRoot(),
    NgSelect2Module,
    TreeviewModule.forRoot(),
  ]
})
export class SeguridadModule { }
