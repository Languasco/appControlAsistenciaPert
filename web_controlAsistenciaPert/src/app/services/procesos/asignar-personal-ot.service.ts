 
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
 
export class AsignarPersonalOTService {

  URL = environment.URL_API;
  locales :any[]=[]; 
  listaContables :any[]=[]; 
  jefesCuadrillas :any[]=[]; 
  tiposTurnos :any[]=[]; 

  constructor(private http:HttpClient) { }  

  get_locales():any{
    if (this.locales.length > 0) {
      return of( this.locales )
    }else{
      let parametros = new HttpParams();
      parametros = parametros.append('opcion', '1');
      parametros = parametros.append('filtro', '');

      return this.http.get( this.URL + 'RegistroAsistencia', {params: parametros})
                 .pipe(map((res:any)=>{ 
                       this.locales = res;
                       return res;
                  }) );
    }
  }
  get_listasContables():any{
    if (this.listaContables.length > 0) {
      return of( this.listaContables )
    }else{
      let parametros = new HttpParams();
      parametros = parametros.append('opcion', '2');
      parametros = parametros.append('filtro', '');
      return this.http.get( this.URL + 'RegistroAsistencia', {params: parametros})
                 .pipe(map((res:any)=>{ 
                       this.listaContables = res;
                       return res;
                  }) );
    }
  }
  get_jefeCuadrillas():any{
    if (this.jefesCuadrillas.length > 0) {
      return of( this.jefesCuadrillas )
    }else{
      let parametros = new HttpParams();
      parametros = parametros.append('opcion', '4');
      parametros = parametros.append('filtro', '');
      return this.http.get( this.URL + 'Reporte_ControlAsistenciaCampo', {params: parametros})
                 .pipe(map((res:any)=>{ 
                       this.jefesCuadrillas = res;
                       return res;
                  }) );
    }
  }
  get_tiposTurnos():any{
    if (this.tiposTurnos.length > 0) {
      return of( this.tiposTurnos )
    }else{
      return this.http.get( this.URL + 'tblAsis_Turno')
                 .pipe(map((res:any)=>{ 
                       this.tiposTurnos = res;
                       return res;
                  }) );
    }
  }

  get_mostrar_informacion(): any{
    return this.http.get( this.URL + 'tblAsis_Turno');
  }
 
  get_verificar_nombreTurno(descTurno:string){
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '1');
    parametros = parametros.append('filtro', descTurno);

    return this.http.get( this.URL + 'Mantenimientos' , {params: parametros}).toPromise();
  }

  set_save_turno(objMantenimiento:any):any{
    return this.http.post(this.URL + 'tblAsis_Turno', JSON.stringify(objMantenimiento), httpOptions);
  }

  set_edit_turno(objMantenimiento:any, id :number):any{
    return this.http.put(this.URL + 'tblAsis_Turno/' + id , JSON.stringify(objMantenimiento), httpOptions);
  }

  set_anular_turno(id : number):any{ 
    return this.http.delete(this.URL + 'tblAsis_Turno/' + id, httpOptions);
  }

}

