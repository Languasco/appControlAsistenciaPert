import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
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
import { TareoPersonalService } from 'src/app/services/reportes/tareo-personal.service';
import { AsistenciaCampoService } from 'src/app/services/reportes/asistencia-campo.service';
 

declare var $:any;
@Component({
  selector: 'app-asistencia-campo',
  templateUrl: './asistencia-campo.component.html',
  styleUrls: ['./asistencia-campo.component.css']
})
 
export class AsistenciaCampoComponent implements OnInit {

  formParamsFiltro : FormGroup;
  idUserGlobal :number = 0;
  flag_modoEdicion :boolean =false;
  datepiekerConfig:Partial<BsDatepickerConfig>  
  locales :any[]=[]; 
  jefeCuadrillas:any[]=[]; 
  listaContables :any[]=[]; 

    /// configuracion google maps
    @ViewChild('mapa', {static: false}) mapaElement: ElementRef;
    map : google.maps.Map;
    markers :google.maps.Marker[] = [];
    infowindows :google.maps.InfoWindow[] = [];
  
  constructor(private alertasService : AlertasService, private spinner: NgxSpinnerService, private loginService: LoginService, private asistenciaCampoService : AsistenciaCampoService,  
              private funcionGlobalServices : FuncionesglobalesService, private asignarPersonalOTService : AsignarPersonalOTService, private tareoPersonalService : TareoPersonalService  ) {         
    this.idUserGlobal = this.loginService.get_idUsuario();
    this.datepiekerConfig = Object.assign({}, { containerClass : 'theme-red',  dateInputFormat: 'DD/MM/YYYY' })
  }
 
 ngOnInit(): void {
   this.inicializarFormulario_filtro(); 
   this.getCargarCombos();
 }

 
 ngAfterViewInit() {
  this.InicializarMapa()
} 

InicializarMapa() {
  const latLng = new google.maps.LatLng(-12.046374, -77.0427934 );
  const  mapaOption : google.maps.MapOptions = {
    center : latLng,
    zoom : 13,
    mapTypeControl: true,
  }
  this.map = new google.maps.Map(this.mapaElement.nativeElement, mapaOption);
 };


 inicializarFormulario_filtro(){ 
  this.formParamsFiltro= new FormGroup({
    id_local: new FormControl('0'), 
    id_OT_contable: new FormControl('0'), 
    fecha: new FormControl( new Date()), 
    id_jefeCuadrilla: new FormControl('0'), 
  }) 
}


 getCargarCombos(){
  this.spinner.show();
  combineLatest([
   this.asignarPersonalOTService.get_locales(), this.asignarPersonalOTService.get_listasContables(),this.asignarPersonalOTService.get_jefeCuadrillas()
 ])
   .subscribe( ([_locales, _otContables, _jefeCuadrillas ]:any)=>{
    this.spinner.hide();
    this.locales = _locales; 
    this.listaContables = _otContables; 
    this.jefeCuadrillas = _jefeCuadrillas; 
  })
 }  

 descargarReporte(){
  
  if (this.formParamsFiltro.value.id_local == '' || this.formParamsFiltro.value.id_local == 0 || this.formParamsFiltro.value.id_local == null) {
    this.alertasService.Swal_alert('error', 'Por favor seleccione el Local');
    return
  }
  if (this.formParamsFiltro.value.fecha_ini == '' || this.formParamsFiltro.value.fecha_ini == null) {
    this.alertasService.Swal_alert('error', 'Por favor ingrese la fecha inicial');
    return
  }
  if (this.formParamsFiltro.value.fecha_fin == '' || this.formParamsFiltro.value.fecha_fin == null) {
    this.alertasService.Swal_alert('error', 'Por favor ingrese la fecha final');
    return
  }
 
  const fechaIni = this.funcionGlobalServices.formatoFecha(this.formParamsFiltro.value.fecha_ini);
  const fechaFin = this.funcionGlobalServices.formatoFecha(this.formParamsFiltro.value.fecha_fin);

  this.spinner.show();   
  this.tareoPersonalService.set_DescargarTareoPersonal({...this.formParamsFiltro.value , fecha_ini : fechaIni , fecha_fin : fechaFin })
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
  this.asistenciaCampoService.get_reporteControlAsistenciaCampo({...this.formParamsFiltro.value , fecha : fechaIni})
      .subscribe((data:any)=>{  
          this.spinner.hide();      

          if (data.length > 0) {

            this.asistenciaCampoService.get_reporteControlAsistenciaCampoGrupo({...this.formParamsFiltro.value , fecha : fechaIni})
            .subscribe((dataGrupo:any)=>{  
                this.spinner.hide();    

                if (dataGrupo.length > 0) {
                  this.MostrarUbicacionesMap( data, dataGrupo );
                }else{
                  this.alertasService.Swal_alert('info','No hay información para disponible para mostrar');
                  this.RemoveMarker(null);
                  this.markers = [];
                }
      
              }, error => {
                this.spinner.hide();
                alert(JSON.stringify(error));
              },)

          }else{
            this.alertasService.Swal_alert('error', 'No hay informacion disponible..');
            return
          }

        }, error => {
          this.spinner.hide();
          alert(JSON.stringify(error));
        },)
} 

RemoveMarker(map:any) {
  for (var i = 0; i < this.markers.length; i++) {
      this.markers[i].setMap(map);
  }
}

MostrarUbicacionesMap(obj_Lista :any, obj_Lista_Agrupado : any) {
  this.RemoveMarker(null);
  this.markers = [];

   for (const ubicaciones of obj_Lista_Agrupado) {
     this.createMarker( ubicaciones, obj_Lista );
   }
}

createMarker(infoGrupo :any, List_info : any ) {

  let ContenidoMarker = '';
  let llave = '';

  llave = '';
  llave = infoGrupo.Latitud_asistenciaCampo + '-' + infoGrupo.Longitud_asistenciaCampo

  if (infoGrupo.Cantidad == 1) {

    for (var i = 0; i < List_info.length; i++) {
      if (llave == List_info[i].Latitud_asistenciaCampo + '-' + List_info[i].Longitud_asistenciaCampo) {
          ContenidoMarker = '';
          ContenidoMarker += '<div style="width:460px;height:140px;position:relative;">';
          ContenidoMarker += '<table><tr><td id="idSuministro"><strong >Codigo </strong></td><td>: ' + List_info[i].codigo_personal + '</td></tr>';
          ContenidoMarker += '<tr><td><strong>D.N.I</strong></td><td>: ' + List_info[i].nroDoc_personal + '</td></tr>';
          ContenidoMarker += '<tr><td><strong>Personal</strong></td><td>: ' + List_info[i].personal + '</td></tr>';
          ContenidoMarker += '<tr><td><strong>E-mail</strong></td><td><div style="width:300px;"> ' + List_info[i].email_personal + ' </div></td></tr>';
          ContenidoMarker += '<tr><td><strong>OT contable</strong></td><td>: ' + List_info[i].codigo_OTContable + ' </td></tr>';
          ContenidoMarker += '<tr><td><div style="width:110px;"><strong>Fecha Asistencia</strong></div></td><td>: ' + List_info[i].fechaHora_asistenciaCampo + ' </td></tr></table>';
          break;
      }
    }

    const marker = new google.maps.Marker({
          map: this.map,
          position: new google.maps.LatLng( Number(infoGrupo.Latitud_asistenciaCampo), Number(infoGrupo.Longitud_asistenciaCampo)),
          title: 'UBICACIÓN DE PERSONAL',
          icon: './assets/img/mapa/operario.png'
    }); 


    this.markers.push(marker); 

    const infowindow = new google.maps.InfoWindow({
      content: ContenidoMarker
    });
  
    this.infowindows.push(infowindow); 
  
    google.maps.event.addListener(marker, 'click',()=>{
      //-----borrando los infowindows
      this.infowindows.forEach(infoW=>infoW.close());
      infowindow.setContent('<center><h4><b> UBICACIÓN DEL PERSONAL </b></h4></center>' + ContenidoMarker);
      infowindow.open(this.map, marker);
    })     

  }else{

    ContenidoMarker = '';
    ContenidoMarker += '<div style="width:800px!important ;height:200px;position:relative;">';
    ContenidoMarker += '<table class="table table-bordered tblDsige">';
    ContenidoMarker += '<tr"><th>Codigo</th><th>D.N.I</th><th>Personal</th><th>OT contable</th><th>Fecha Asistencia</th></tr>';

    for (var i = 0; i < List_info.length; i++) {
        if (llave == List_info[i].Latitud_asistenciaCampo + '-' + List_info[i].Longitud_asistenciaCampo) {
            ContenidoMarker += '<tr>';
            ContenidoMarker += '<td><div style="width:70px;"> ' + List_info[i].codigo_personal + ' </div></td>';
            ContenidoMarker += '<td><div style="width:60px;"> ' + List_info[i].nroDoc_personal + ' </div></td>';
            ContenidoMarker += '<td><div style="width:240px;"> ' + List_info[i].personal + ' </div></td>';
            ContenidoMarker += '<td><div style="width:60px;"> ' + List_info[i].codigo_OTContable + ' </div></td>';
            ContenidoMarker += '<td><div style="width:140px;"> ' + List_info[i].fechaHora_asistenciaCampo + ' </div></td>';
            ContenidoMarker += '</tr>';
        }
    }
    ContenidoMarker += '</table>';

    const marker = new google.maps.Marker({
          map: this.map,
          position: new google.maps.LatLng( Number(infoGrupo.Latitud_asistenciaCampo), Number(infoGrupo.Longitud_asistenciaCampo)),
          title: 'UBICACIÓN DE PERSONAL',
          icon: './assets/img/mapa/grupoOperario.png'
    });    
    
    this.markers.push(marker);

    const infowindow = new google.maps.InfoWindow({
      content: ContenidoMarker
    });
  
    this.infowindows.push(infowindow); 
    google.maps.event.addListener(marker, 'click',()=>{
        //-----borrando los infowindows
        this.infowindows.forEach(infoW=>infoW.close());
        infowindow.setContent('<center><h4><b> UBICACIÓN DE LOS PERSONALES </b></h4></center>' + ContenidoMarker);
        infowindow.open(this.map, marker);
    })  
  }


 



 
}



 


}

