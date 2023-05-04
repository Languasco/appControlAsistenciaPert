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

export class TareoDiarioService {

  URL = environment.URL_API;
  constructor(private http:HttpClient) { }  

  get_mostrarInformacion({id_local, id_OT_contable, fecha  }){
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '2');
    parametros = parametros.append('filtro', id_local + '|' + id_OT_contable + '|' + fecha );

    return this.http.get( this.URL + 'Reporte_Tareo_diario' , {params: parametros});
  }

  set_DescargarTareoDiario({id_local, id_OT_contable, fecha  }){
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '3');
    parametros = parametros.append('filtro', id_local + '|' + id_OT_contable + '|' + fecha );

    return this.http.get( this.URL + 'Reporte_Tareo_diario' , {params: parametros});
  }

}
