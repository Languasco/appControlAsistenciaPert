import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { AlertasService } from '../../../services/alertas/alertas.service';
import { FuncionesglobalesService } from '../../../services/funciones/funcionesglobales.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { LoginService } from '../../../services/login/login.service';
import Swal from 'sweetalert2'; 
import { TipoAsistenciaService } from 'src/app/services/mantenimientos/tipo-asistencia.service';
 
declare var $:any;
@Component({
  selector: 'app-tipo-asistencia',
  templateUrl: './tipo-asistencia.component.html',
  styleUrls: ['./tipo-asistencia.component.css']
})
 
export class TipoAsistenciaComponent implements OnInit {

  formParamsFiltro : FormGroup;
  formParams: FormGroup;

  idUserGlobal :number = 0;
  flag_modoEdicion :boolean =false;

  tiposAsistencias :any[]=[]; 
  filtrarMantenimiento = "";
 
  constructor(private alertasService : AlertasService, private spinner: NgxSpinnerService, private loginService: LoginService,
              private funcionGlobalServices : FuncionesglobalesService, private tiposAsistenciasService : TipoAsistenciaService  ) {         
    this.idUserGlobal = this.loginService.get_idUsuario();
  }
 
 ngOnInit(): void {
   this.mostrarInformacion();
   this.inicializarFormulario(); 
 }


 inicializarFormulario(){ 
    this.formParams= new FormGroup({
      id_tasi_codigo: new FormControl(''), 
      descripcion_tasi: new FormControl(''), 
      abreviatura_tasi: new FormControl(''), 
      valor_tasi: new FormControl(''), 
      estado : new FormControl('1'),   
      usuario_creacion : new FormControl('')
    }) 
 }

 mostrarInformacion(){ 
      this.spinner.show();
      this.tiposAsistenciasService.get_mostrar_informacion()
          .subscribe((res:any)=>{  
              this.spinner.hide();      
              this.tiposAsistencias = res;        
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

  if (this.formParams.value.id_tasi_codigo == '' || this.formParams.value.id_tasi_codigo == null) {
    this.alertasService.Swal_alert('error','Por favor ingrese el codigo');
    return 
  } 
  if (this.formParams.value.descripcion_tasi == '' || this.formParams.value.descripcion_tasi == null) {
    this.alertasService.Swal_alert('error','Por favor ingrese la Descripcion');
    return 
  } 
  if (this.formParams.value.valor_tasi == '' || this.formParams.value.valor_tasi == null || this.formParams.value.valor_tasi == '0' ) {
    this.alertasService.Swal_alert('error','Por favor ingrese el Valor');
    return 
  } 
 
  this.formParams.patchValue({ "usuario_creacion" : this.idUserGlobal });
 
  if ( this.flag_modoEdicion==false) { //// nuevo  

     Swal.fire({  icon: 'info', allowOutsideClick: false, allowEscapeKey: false, text: 'Espere por favor'  })
     Swal.showLoading();

     const  descTurno  = await this.tiposAsistenciasService.get_verificar_codigo(this.formParams.value.id_tasi_codigo);
     if (descTurno) {
      Swal.close();
      this.alertasService.Swal_alert('error','El codigo ya esta registrada, verifique..');
      return;
     }   
 
    this.tiposAsistenciasService.set_save_tipoAsistencia(this.formParams.value)
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
    this.tiposAsistenciasService.set_edit_tipoAsistencia(this.formParams.value , this.formParams.value.id_tasi_codigo)
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

 editar({ id_tasi_codigo, descripcion_tasi, abreviatura_tasi, valor_tasi, estado}){
   this.flag_modoEdicion=true;
   this.formParams.patchValue({ "id_tasi_codigo" : id_tasi_codigo,"descripcion_tasi" : descripcion_tasi, "abreviatura_tasi" : abreviatura_tasi, "valor_tasi" : valor_tasi ,"estado" : estado });
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
     this.tiposAsistenciasService.set_anular_tipoAsistencia(objBD.id_tasi_codigo)
       .subscribe((res:any)=>{  
         Swal.close();    
           for (const user of this.tiposAsistencias) {
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


}
