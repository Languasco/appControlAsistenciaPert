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
 

export class AnomaliasService {

  URL = environment.URL_API;
  constructor(private http:HttpClient) { }  

  get_mostrar_informacion(): any{
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '1');
    parametros = parametros.append('filtro', '');

    return this.http.get( this.URL + 'tblVehiculo' , {params: parametros});
  }

  get_verificar_codigoVehiculo(codRol:string){
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '3');
    parametros = parametros.append('filtro', codRol);

    return this.http.get( this.URL + 'tblVehiculo' , {params: parametros}).toPromise();
  }

  get_verificar_descripcionVehiculo(descRol:string){
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '4');
    parametros = parametros.append('filtro', descRol);

    return this.http.get( this.URL + 'tblVehiculo' , {params: parametros}).toPromise();
  }


  set_save_vehiculo(objMantenimiento:any):any{
    return this.http.post(this.URL + 'tblVehiculo', JSON.stringify(objMantenimiento), httpOptions);
  }

  set_edit_vehiculo(objMantenimiento:any, id_perfil :number):any{
    return this.http.put(this.URL + 'tblVehiculo/' + id_perfil , JSON.stringify(objMantenimiento), httpOptions);
  }

  set_anular_vehiculo(id_perfil : number):any{ 
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '2');
    parametros = parametros.append('filtro',  String(id_perfil));

    return this.http.get( this.URL + 'tblVehiculo' , {params: parametros});
  }

}

