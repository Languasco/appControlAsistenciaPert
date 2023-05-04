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
 

declare var $:any;

@Component({
  selector: 'app-registro-asistencia',
  templateUrl: './registro-asistencia.component.html',
  styleUrls: ['./registro-asistencia.component.css']
})
 
 
export class RegistroAsistenciaComponent implements OnInit {

  formParamsFiltro : FormGroup;
  formParams: FormGroup;

  idUserGlobal :number = 0;
  flag_modoEdicion :boolean =false;
  datepiekerConfig:Partial<BsDatepickerConfig>
  
  locales :any[]=[]; 
  listaContables :any[]=[]; 

  turnos :any[]=[]; 
  filtrarMantenimiento = "";
 
  constructor(private alertasService : AlertasService, private spinner: NgxSpinnerService, private loginService: LoginService,
              private funcionGlobalServices : FuncionesglobalesService, private asignarPersonalOTService : AsignarPersonalOTService  ) {         
    this.idUserGlobal = this.loginService.get_idUsuario();
    this.datepiekerConfig = Object.assign({}, { containerClass : 'theme-red',  dateInputFormat: 'DD/MM/YYYY' })
  }
 
 ngOnInit(): void {
   this.mostrarInformacion();
   this.inicializarFormulario_filtro(); 
   this.inicializarFormulario(); 
   this.getCargarCombos();
 }
 inicializarFormulario_filtro(){ 
  this.formParamsFiltro= new FormGroup({
    id_local: new FormControl('0'), 
    id_OT_contable: new FormControl('0'), 
    fecha: new FormControl( new Date()), 
  }) 
}

 inicializarFormulario(){ 
    this.formParams= new FormGroup({
      id_turno: new FormControl('0'), 
      nombre_turno: new FormControl(''), 
      simbolo_turno: new FormControl(''), 
      horaInicio_turno: new FormControl('00:00'), 
      horaTermino_turno: new FormControl('00:00'), 
      tiempoMaxInicio_turno: new FormControl(''), 
      estado : new FormControl('1'),   
      usuario_creacion : new FormControl('')
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
      this.spinner.show();
      this.asignarPersonalOTService.get_mostrar_informacion()
          .subscribe((res:any)=>{  
              this.spinner.hide();      
              this.turnos = res;        
            }, error => {
              this.spinner.hide();
              alert(JSON.stringify(error));
            },)
 }   
  
 cerrarModal(){
    setTimeout(()=>{ // 
      $('#modal_mantenimiento').modal('hide');  
    },0); 
 }

 nuevo(){
    this.flag_modoEdicion = false;
    this.inicializarFormulario();  
    setTimeout(()=>{ // 
      $('#txtcodigo').removeClass('disabledForm');
      $('#modal_mantenimiento').modal('show');  
    },0); 
 } 

 async saveUpdate(){ 

  if (this.formParams.value.nombre_turno == '' || this.formParams.value.nombre_turno == null) {
    this.alertasService.Swal_alert('error','Por favor ingrese la descripcion del turno');
    return 
  } 
  if (this.formParams.value.horaInicio_turno == '' || this.formParams.value.horaInicio_turno == null) {
    this.alertasService.Swal_alert('error','Por favor ingrese la Hora de Inicio del Turno');
    return 
  } 
  if (this.formParams.value.horaTermino_turno == '' || this.formParams.value.horaTermino_turno == null) {
    this.alertasService.Swal_alert('error','Por favor ingrese la Hora de término del Turno');
    return 
  } 
  if (this.formParams.value.tiempoMaxInicio_turno == '' || this.formParams.value.tiempoMaxInicio_turno == null) {
    this.alertasService.Swal_alert('error','Por favor ingrese la Hora Maxima del Turno');
    return 
  } 
  this.formParams.patchValue({ "usuario_creacion" : this.idUserGlobal });
 
  if ( this.flag_modoEdicion==false) { //// nuevo  

     Swal.fire({  icon: 'info', allowOutsideClick: false, allowEscapeKey: false, text: 'Espere por favor'  })
     Swal.showLoading();

     const  descTurno  = await this.asignarPersonalOTService.get_verificar_nombreTurno(this.formParams.value.nombre_turno);
     if (descTurno) {
      Swal.close();
      this.alertasService.Swal_alert('error','El nombre del Turno ya esta registrada, verifique..');
      return;
     }   
 
    this.asignarPersonalOTService.set_save_turno(this.formParams.value)
      .subscribe((res:any)=>{  
        Swal.close();    
         this.alertasService.Swal_Success('Se agrego correctamente..'); 
         this.flag_modoEdicion = true;
         this.mostrarInformacion();
         this.cerrarModal();      
      }, error => {
        Swal.close();
        alert(JSON.stringify(error));
      },)
     
   }else{ /// editar

     Swal.fire({  icon: 'info', allowOutsideClick: false, allowEscapeKey: false, text: 'Actualizando, espere por favor'  })
     Swal.showLoading();    
    this.asignarPersonalOTService.set_edit_turno(this.formParams.value , this.formParams.value.id_turno)
      .subscribe((res:any)=>{  
        Swal.close();    
         this.mostrarInformacion();
         this.alertasService.Swal_Success('Se actualizó correctamente..');  
         this.cerrarModal();      
      }, error => {
        Swal.close();
        alert(JSON.stringify(error));
      },)

   }

 } 

 editar({id_turno, nombre_turno, simbolo_turno, horaInicio_turno, horaTermino_turno, tiempoMaxInicio_turno, estado }){
   this.flag_modoEdicion=true;
   this.formParams.patchValue({ "id_turno" : id_turno,"nombre_turno" : nombre_turno, "simbolo_turno" : simbolo_turno, "horaInicio_turno" : horaInicio_turno ,
     "horaTermino_turno" : horaTermino_turno,"tiempoMaxInicio_turno" : tiempoMaxInicio_turno, "estado" : estado 
  });
   setTimeout(()=>{ // 
    $('#modal_mantenimiento').modal('show');  
  },0);  
 } 

 anular(objBD:any){

   if (objBD.estado ===0 || objBD.estado =='0') {      
     return;      
   }

   this.alertasService.Swal_Question('Sistemas', 'Esta seguro de anular ?')
   .then((result)=>{
     if(result.value){


      Swal.fire({  icon: 'info', allowOutsideClick: false, allowEscapeKey: false, text: 'Anulando, espere por favor'  })
      Swal.showLoading();    
     this.asignarPersonalOTService.set_anular_turno(objBD.id_turno)
       .subscribe((res:any)=>{  
         Swal.close();    
           for (const user of this.turnos) {
             if (user.id_turno == objBD.id_turno ) {
                 user.estado = 0;
                 break;
             }
           }
           this.alertasService.Swal_Success('Se anulo correctamente..')     
       }, error => {
         Swal.close();
         alert(JSON.stringify(error));
       },)
        
     }
   }) 

 }


}

