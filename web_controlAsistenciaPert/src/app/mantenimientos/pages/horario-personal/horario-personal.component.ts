import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { AlertasService } from '../../../services/alertas/alertas.service';
import { FuncionesglobalesService } from '../../../services/funciones/funcionesglobales.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { LoginService } from '../../../services/login/login.service';
import Swal from 'sweetalert2'; 
import { HorarioPersonalService } from 'src/app/services/mantenimientos/horario-personal.service';
import { combineLatest } from 'rxjs';
 
declare var $:any;

@Component({
  selector: 'app-horario-personal',
  templateUrl: './horario-personal.component.html',
  styleUrls: ['./horario-personal.component.css']
})
 
export class HorarioPersonalComponent implements OnInit {

  formParamsFiltro : FormGroup;
  formParams: FormGroup;

  idUserGlobal :number = 0;
  flag_modoEdicion :boolean =false;

  horarios :any[]=[]; 
  personales:any[]=[]; 
  filtrarMantenimiento = "";
  bloquearPersonal= "";
 
  constructor(private alertasService : AlertasService, private spinner: NgxSpinnerService, private loginService: LoginService,
              private funcionGlobalServices : FuncionesglobalesService, private horarioPersonalService : HorarioPersonalService  ) {         
    this.idUserGlobal = this.loginService.get_idUsuario();
  }
 
 ngOnInit(): void {
   this.mostrarInformacion();
   this.getCargarCombos();
   this.inicializarFormulario(); 
   setTimeout(()=>{ //      
    // $('.select2').select2();
      $('.select2').select2({
        dropdownParent: $('#modal_mantenimiento')
      }); 
    },0);


 }


 inicializarFormulario(){ 
    this.formParams= new FormGroup({
      id_personal : new FormControl('0'),  
      lunes_I : new FormControl(''), 
      lunes_S : new FormControl(''), 
      Martes_I : new FormControl(''), 
      Martes_S : new FormControl(''), 
      Miercoles_I : new FormControl(''), 
      Miercoles_S : new FormControl(''), 
      Jueves_I : new FormControl(''), 
      Jueves_S : new FormControl(''), 
      Viernes_I : new FormControl(''), 
      Viernes_S : new FormControl(''), 
      Sabado_I : new FormControl(''), 
      Sabado_S : new FormControl(''), 
      Domingo_I : new FormControl(''), 
      Domingo_S: new FormControl(''), 
    }) 
 }

 getCargarCombos(){
  this.spinner.show();
  combineLatest([
   this.horarioPersonalService.get_personales(),
 ])
   .subscribe( ([_personales ]:any)=>{
    this.spinner.hide();
    this.personales = _personales; 
  })
 } 

 mostrarInformacion(){ 
      this.spinner.show();
      this.horarioPersonalService.get_mostrar_informacion()
          .subscribe((res:any)=>{  
              this.spinner.hide();      
              this.horarios = res;        
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
      this.bloquearPersonal = "";
      $('#cboPersonal').val("0").trigger('change');
      $('#modal_mantenimiento').modal('show');  
    },0);  
 } 

 saveUpdate():void{ 

  const idPersonal= $('#cboPersonal').val(); 
  this.formParams.patchValue({ "id_personal" :  idPersonal }); 

  if (this.formParams.value.id_personal == '0' || this.formParams.value.id_personal == 0) {
    this.alertasService.Swal_alert('error','Por favor seleccione el Personal');
    return 
  } 

  if (this.formParams.value.lunes_I == '' || this.formParams.value.lunes_I == null ||  this.formParams.value.lunes_I == undefined ) {
    this.alertasService.Swal_alert('error','Debe de Completar con el Formato HH:MM la Hora de Ingreso del dia Lunes');
    return 
  } 
  if (this.formParams.value.lunes_S == '' || this.formParams.value.lunes_S == null ||  this.formParams.value.lunes_S == undefined ) {
    this.alertasService.Swal_alert('error','Debe de Completar con el Formato HH:MM la Hora de Salida del dia Lunes');
    return 
  } 
  if (this.formParams.value.Martes_I == '' || this.formParams.value.Martes_I == null ||  this.formParams.value.Martes_I == undefined ) {
    this.alertasService.Swal_alert('error','Debe de Completar con el Formato HH:MM la Hora de Ingreso del dia Martes');
    return 
  } 
  if (this.formParams.value.Martes_S == '' || this.formParams.value.Martes_S == null ||  this.formParams.value.Martes_S == undefined ) {
    this.alertasService.Swal_alert('error','Debe de Completar con el Formato HH:MM la Hora de Salida del dia Martes');
    return 
  } 
  if (this.formParams.value.Miercoles_I == '' || this.formParams.value.Miercoles_I == null ||  this.formParams.value.Miercoles_I == undefined ) {
    this.alertasService.Swal_alert('error','Debe de Completar con el Formato HH:MM la Hora de Ingreso del dia Miercoles');
    return 
  } 
  if (this.formParams.value.Miercoles_S == '' || this.formParams.value.Miercoles_S == null ||  this.formParams.value.Miercoles_S == undefined ) {
    this.alertasService.Swal_alert('error','Debe de Completar con el Formato HH:MM la Hora de Salida del dia Miercoles');
    return 
  } 

  if (this.formParams.value.Jueves_I == '' || this.formParams.value.Jueves_I == null ||  this.formParams.value.Jueves_I == undefined ) {
    this.alertasService.Swal_alert('error','Debe de Completar con el Formato HH:MM la Hora de Ingreso del dia Jueves');
    return 
  } 
  if (this.formParams.value.Jueves_S == '' || this.formParams.value.Jueves_S == null ||  this.formParams.value.Jueves_S == undefined ) {
    this.alertasService.Swal_alert('error','Debe de Completar con el Formato HH:MM la Hora de Salida del dia Jueves');
    return 
  } 
  if (this.formParams.value.Viernes_I == '' || this.formParams.value.Viernes_I == null ||  this.formParams.value.Viernes_I == undefined ) {
    this.alertasService.Swal_alert('error','Debe de Completar con el Formato HH:MM la Hora de Ingreso del dia Viernes');
    return 
  } 
  if (this.formParams.value.Viernes_S == '' || this.formParams.value.Viernes_S == null ||  this.formParams.value.Viernes_S == undefined ) {
    this.alertasService.Swal_alert('error','Debe de Completar con el Formato HH:MM la Hora de Salida del dia Viernes');
    return 
  } 
  if (this.formParams.value.Sabado_I == '' || this.formParams.value.Sabado_I == null ||  this.formParams.value.Sabado_I == undefined ) {
    this.alertasService.Swal_alert('error','Debe de Completar con el Formato HH:MM la Hora de Ingreso del dia Sabado');
    return 
  } 
  if (this.formParams.value.Sabado_S == '' || this.formParams.value.Sabado_S == null ||  this.formParams.value.Sabado_S == undefined ) {
    this.alertasService.Swal_alert('error','Debe de Completar con el Formato HH:MM la Hora de Salida del dia Sabado');
    return 
  } 
  if (this.formParams.value.Domingo_I == '' || this.formParams.value.Domingo_I == null ||  this.formParams.value.Domingo_I == undefined ) {
    this.alertasService.Swal_alert('error','Debe de Completar con el Formato HH:MM la Hora de Ingreso del dia Domingo');
    return 
  } 
  if (this.formParams.value.Domingo_S == '' || this.formParams.value.Domingo_S == null ||  this.formParams.value.Domingo_S == undefined ) {
    this.alertasService.Swal_alert('error','Debe de Completar con el Formato HH:MM la Hora de Salida del dia Domingo');
    return 
  } 

 
  this.formParams.patchValue({ "usuario_creacion" : this.idUserGlobal });
 
  if ( this.flag_modoEdicion==false) { //// nuevo  


     const valor = this.validarPersonal(this.formParams.value.id_personal)
     if (valor == true) { 
         this.alertasService.Swal_alert('error','El personal seleccionado ya se encuentra Registrado..');
         return;
     }
 
    Swal.fire({  icon: 'info', allowOutsideClick: false, allowEscapeKey: false, text: 'Espere por favor'  })
    Swal.showLoading();
    this.horarioPersonalService.set_save_horarioPersonal(this.formParams.value)
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
    this.horarioPersonalService.set_edit_horarioPersonal(this.formParams.value)
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

 
 validarPersonal(obj_id_personal) {
  var resultado = false; 
  this.horarios.forEach(function (item, index) {
      if (item.id_personal == obj_id_personal) {
          resultado = true;
      }
  })
  return resultado;
}


 editar({id_personal, lunes_I, lunes_S, Martes_I, Martes_S, Miercoles_I, Miercoles_S, Jueves_I, Jueves_S, Viernes_I, Viernes_S, Sabado_I, Sabado_S, Domingo_I, Domingo_S}){ 
   this.flag_modoEdicion=true;
   this.formParams.patchValue({ "id_personal" : id_personal,"lunes_I" : lunes_I, "lunes_S" : lunes_S, 
                                "Martes_I" : Martes_I ,"Martes_S" : Martes_S, "Miercoles_I" : Miercoles_I ,"Miercoles_S" : Miercoles_S ,
                                "Jueves_I" : Jueves_I ,"Jueves_S" : Jueves_S , "Viernes_I" : Viernes_I ,"Viernes_S" : Viernes_S ,
                                "Sabado_I" : Sabado_I ,"Sabado_S" : Sabado_S , "Domingo_I" : Domingo_I ,"Domingo_S" : Domingo_S 
                              });
   setTimeout(()=>{ // 
    this.bloquearPersonal = "disabledForm";
    $('#cboPersonal').val(id_personal).trigger('change');
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
     this.horarioPersonalService.set_anular_tipoAsistencia(objBD.id_tasi_codigo)
       .subscribe((res:any)=>{  
         Swal.close();    
           for (const user of this.horarios) {
             if (user.id_tasi_codigo == objBD.id_tasi_codigo ) {
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

 keyPress(event: any) {
    this.funcionGlobalServices.verificar_soloNumeros(event)  ;
  }

  changeFocusInput (id) {
    let doc:any = document.getElementById(id);
    doc.focus();
}


}
