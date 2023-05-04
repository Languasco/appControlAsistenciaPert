 


import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
import { RespuestaServer } from 'src/app/models/respuestaServer.models';
import { of } from 'rxjs';
import { map } from 'rxjs/operators';

const httpOptions = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
const HttpUploadOptions = {
  headers: new HttpHeaders({ "Content-Type": "multipart/form-data" })
}
@Injectable({
  providedIn: 'root'
})
 
export class HorarioPersonalService {

  URL = environment.URL_API;
  personales:any[]=[]; 

  constructor(private http:HttpClient) { }  

  


  get_mostrar_informacion(): any{
    // return this.http.get( this.URL + 'tblHorario_Personal');
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '1');
    parametros = parametros.append('filtro', '' );

    return this.http.get( this.URL + 'tblHorario_Personal' , {params: parametros});
  }
 
  get_verificar_codigo(codigo:string){
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '2');
    parametros = parametros.append('filtro', codigo);

    return this.http.get( this.URL + 'Mantenimientos' , {params: parametros}).toPromise();
  }


  get_verificar_nombreTurno(descTurno:string){
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '1');
    parametros = parametros.append('filtro', descTurno);

    return this.http.get( this.URL + 'Mantenimientos' , {params: parametros}).toPromise();
  }

  set_save_horarioPersonal(obj:any):any{
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '2');
    parametros = parametros.append('filtro', obj.id_personal + '|' + obj.lunes_I + '|' + obj.lunes_S + '|' + obj.Martes_I + '|' + obj.Martes_S + '|' + obj.Miercoles_I + '|' + obj.Miercoles_S + '|' + obj.Jueves_I + '|' + obj.Jueves_S + '|' + obj.Viernes_I + '|' + obj.Viernes_S + '|' + obj.Sabado_I + '|' + obj.Sabado_S + '|' + obj.Domingo_I + '|' +  obj.Domingo_S);
    return this.http.get( this.URL + 'tblHorario_Personal' , {params: parametros});
  }

  set_edit_horarioPersonal(obj:any):any{
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '3');
    parametros = parametros.append('filtro', obj.id_personal + '|' + obj.lunes_I + '|' + obj.lunes_S + '|' + obj.Martes_I + '|' + obj.Martes_S + '|' + obj.Miercoles_I + '|' + obj.Miercoles_S + '|' + obj.Jueves_I + '|' + obj.Jueves_S + '|' + obj.Viernes_I + '|' + obj.Viernes_S + '|' + obj.Sabado_I + '|' + obj.Sabado_S + '|' + obj.Domingo_I + '|' +  obj.Domingo_S);
    return this.http.get( this.URL + 'tblHorario_Personal' , {params: parametros});
  }

  set_anular_tipoAsistencia(id : string):any{ 
    return this.http.delete(this.URL + 'tblAsis_TipoAsistencia/' + id, httpOptions);
  }

  get_personales():any{
    if (this.personales.length > 0) {
      return of( this.personales )
    }else{
      return this.http.get( this.URL + 'tblPersonal')
                 .pipe(map((res:any)=>{ 
                       this.personales = res;
                       return res;
                  }) );
    }
  }


}

