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

export class AsistenciaCampoService {

  URL = environment.URL_API;
  constructor(private http:HttpClient) { }  

  get_reporteControlAsistenciaCampo({id_local, id_OT_contable, fecha, id_jefeCuadrilla}){
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '3');
    parametros = parametros.append('filtro', id_local + '|' + id_OT_contable + '|' + fecha + '|' + id_jefeCuadrilla );

    return this.http.get( this.URL + 'Reporte_ControlAsistenciaCampo' , {params: parametros});
  }

  get_reporteControlAsistenciaCampoGrupo({id_local, id_OT_contable, fecha, id_jefeCuadrilla}){
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '5');
    parametros = parametros.append('filtro', id_local + '|' + id_OT_contable + '|' + fecha + '|' + id_jefeCuadrilla );

    return this.http.get( this.URL + 'Reporte_ControlAsistenciaCampo' , {params: parametros});
  }
 

}
