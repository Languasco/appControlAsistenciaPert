import { Injectable } from '@angular/core';
import { environment } from '../../../environments/environment';
import { HttpClient, HttpParams, HttpHeaders } from '@angular/common/http';
 

@Injectable({
  providedIn: 'root'
})

export class UsuarioService {

  URL = environment.URL_API;
  constructor(private http:HttpClient) { }  

  get_mostrar_informacion(): any{
    return this.http.get( this.URL + 'tblAsis_Turno');
  }

  get_accesosMenu():any{
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '4');
    parametros = parametros.append('filtro',  '' );

    return this.http.get( this.URL + 'Login' , {params: parametros});
  }
  get_mostrarUsuarios_generalAccesos():any{
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '5');
    parametros = parametros.append('filtro',  '' );
    return this.http.get( this.URL + 'Login' , {params: parametros});
  }

  get_mostrarPerfiles_generalAccesos():any{
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '6');
    parametros = parametros.append('filtro',  '' );
    return this.http.get( this.URL + 'Login' , {params: parametros});
  }

  get_permisosUsuarioAcceso(idOpciones:string ):any{
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '7');
    parametros = parametros.append('filtro',  idOpciones );
    return this.http.get( this.URL + 'Login' , {params: parametros});
  }

  get_permisosPerfilAcceso(idOpciones:string ):any{
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '10');
    parametros = parametros.append('filtro',  idOpciones );
    return this.http.get( this.URL + 'tblUsuarios' , {params: parametros});
  }
 
        
  get_eventosPerfilMarcados(idOpciones:string, idPerfil:number ):any{
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '11');
    parametros = parametros.append('filtro',  idOpciones +'|'+ idPerfil );
    return this.http.get( this.URL + 'tblUsuarios' , {params: parametros});
  }

  set_grabarEventos(idOpciones:string,idEventos :string , id_usuario:number, modalElegido:string ):any {
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '9');
    parametros = parametros.append('filtro',  idOpciones +'|'+ idEventos +'|'+ id_usuario +'|'+ modalElegido );
    return this.http.get( this.URL + 'Login' , {params: parametros});
  }

  get_eventosUsuarioMarcados(idOpciones:string,id_usuario:number ):any {
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '8');
    parametros = parametros.append('filtro',  idOpciones +'|'+ id_usuario );
    return this.http.get( this.URL + 'Login' , {params: parametros});
  }


  set_eliminarAccesos(idOpciones:string, id_usuario:number ):any{
    let parametros = new HttpParams();
    parametros = parametros.append('opcion', '10');
    parametros = parametros.append('filtro',  idOpciones +'|'+ id_usuario );
    return this.http.get( this.URL + 'Login' , {params: parametros});
  }


}