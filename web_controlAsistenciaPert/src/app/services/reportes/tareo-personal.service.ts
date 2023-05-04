import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { RespuestaServer } from 'src/app/models/respuestaServer.models';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
const HttpUploadOptions = {
  headers: new HttpHeaders({ "Content-Type": "multipart/form-data" })
}

@Injectable({
  providedIn: 'root'
})
export class TareoPersonalService {

  URL = environment.URL_API;
  constructor(private http:HttpClient) { }  

  set_DescargarTareoPersonal({id_local, id_OT_contable, fecha_ini, fecha_fin  }){
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '3');
    parametros = parametros.append('filtro', id_local + '|' + id_OT_contable + '|' + fecha_ini + '|' +  fecha_fin);

    return this.http.get( this.URL + 'Reporte_TareoPersonal' , {params: parametros});
  } 
 
  get_mostrarInformacion_reporteInicioFin({id_local, id_personal, id_turno, fecha_ini, fecha_fin, tipo }){
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '1');
    parametros = parametros.append('filtro', id_local + '|' + id_personal + '|' + id_turno + '|' + fecha_ini  + '|' + fecha_fin + '|' + tipo);

    return this.http.get( this.URL + 'Reporte_LectorHuellas' , {params: parametros});
  }

  set_Descargar_reporteInicioFin({ id_local, id_personal, id_turno, fecha_ini, fecha_fin, tipo  }){
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '2');
    parametros = parametros.append('filtro', id_local + '|' + id_personal + '|' + id_turno + '|' + fecha_ini  + '|' + fecha_fin + '|' + tipo);

    return this.http.get( this.URL + 'Reporte_LectorHuellas' , {params: parametros});
  }
 

}
