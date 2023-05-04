import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { AlertasService } from '../../../services/alertas/alertas.service';
import { RespuestaServer } from '../../../models/respuestaServer.models';
import { FuncionesglobalesService } from '../../../services/funciones/funcionesglobales.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { LoginService } from '../../../services/login/login.service';
import Swal from 'sweetalert2'; 
 
import { BsDatepickerConfig } from 'ngx-bootstrap';
import { combineLatest } from 'rxjs';
import { AsignarPersonalOTService } from 'src/app/services/procesos/asignar-personal-ot.service';
import { TareoDiarioService } from 'src/app/services/reportes/tareo-diario.service';
 
 
declare var $:any;
@Component({
  selector: 'app-tareo-diario',
  templateUrl: './tareo-diario.component.html',
  styleUrls: ['./tareo-diario.component.css']
})

export class TareoDiarioComponent implements OnInit {

  formParamsFiltro : FormGroup;
  idUserGlobal :number = 0;
  flag_modoEdicion :boolean =false;
  datepiekerConfig:Partial<BsDatepickerConfig>
  
  locales :any[]=[]; 
  listaContables :any[]=[];  
 
  listaTareos :any[]=[];  
  filtrarMantenimiento :string = "";

  constructor(private alertasService : AlertasService, private spinner: NgxSpinnerService, private loginService: LoginService,
              private funcionGlobalServices : FuncionesglobalesService, private asignarPersonalOTService : AsignarPersonalOTService, private tareoDiarioService : TareoDiarioService  ) {         
    this.idUserGlobal = this.loginService.get_idUsuario();
    this.datepiekerConfig = Object.assign({}, { containerClass : 'theme-red',  dateInputFormat: 'DD/MM/YYYY' })
  }
 
 ngOnInit(): void {
   this.inicializarFormulario_filtro(); 
   this.getCargarCombos();
 }
 inicializarFormulario_filtro(){ 
  this.formParamsFiltro= new FormGroup({
    id_local: new FormControl('0'), 
    id_OT_contable: new FormControl('0'), 
    fecha: new FormControl( new Date()), 
  }) 
}


 getCargarCombos(){
  this.spinner.show();
  combineLatest([
   this.asignarPersonalOTService.get_locales(), this.asignarPersonalOTService.get_listasContables(),
 ])
   .subscribe( ([_locales, _otContables ]:any)=>{
    this.spinner.hide();
    this.locales = _locales; 
    this.listaContables = _otContables; 
  })
 }  

 mostrarInformacion(){ 
    
  if (this.formParamsFiltro.value.id_local == '' || this.formParamsFiltro.value.id_local == 0 || this.formParamsFiltro.value.id_local == null) {
    this.alertasService.Swal_alert('error', 'Por favor seleccione el Local');
    return
  }
  if (this.formParamsFiltro.value.fecha == '' || this.formParamsFiltro.value.fecha == null) {
    this.alertasService.Swal_alert('error', 'Por favor ingrese la fecha ');
    return
  }
  const fechaIni = this.funcionGlobalServices.formatoFecha(this.formParamsFiltro.value.fecha);

  this.spinner.show();
  this.tareoDiarioService.get_mostrarInformacion({...this.formParamsFiltro.value , fecha : fechaIni})
      .subscribe((res:any)=>{  
          this.spinner.hide();      
          this.listaTareos = res;        
        }, error => {
          this.spinner.hide();
          alert(JSON.stringify(error));
        },)
} 


 descargarReporte(){
  
  if (this.formParamsFiltro.value.id_local == '' || this.formParamsFiltro.value.id_local == 0 || this.formParamsFiltro.value.id_local == null) {
    this.alertasService.Swal_alert('error', 'Por favor seleccione el Local');
    return
  }
  if (this.formParamsFiltro.value.fecha == '' || this.formParamsFiltro.value.fecha == null) {
    this.alertasService.Swal_alert('error', 'Por favor ingrese la fecha ');
    return
  }
  const fechaIni = this.funcionGlobalServices.formatoFecha(this.formParamsFiltro.value.fecha);
 
  this.spinner.show();   
  this.tareoDiarioService.set_DescargarTareoDiario({...this.formParamsFiltro.value , fecha : fechaIni})
    .subscribe((data:any)=>{
      this.spinner.hide(); 
      const res = data.split('|');

      if (res[0] == 0 || res[0] == "0") {
        this.alertasService.Swal_alert('error', 'No hay informacion para mostrar');
      } else if (res[0] == -1 || res[0] == "-1") {
        alert(JSON.stringify(res[1]));
      }
      else {
        window.open(String(res[1]),'_blank');
      } 
  }) 
} 
 


}
