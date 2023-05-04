using CapaDatos;
using CapaDatos.Mantenimento;
using CapaNegocios.Mantenimiento;
using Entidades.Acceso;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using System.Web.Http.Cors;


namespace webApiFacturacion.Controllers.Mantenimientos
{
    [EnableCors("*", "*", "*")]
    public class LoginController : ApiController
    {
        private CAM_ControlAsistenciaEntities db = new CAM_ControlAsistenciaEntities();


        public object GetLogin(int opcion, string filtro)        {
            //filtro puede tomar cualquier valor
            db.Configuration.ProxyCreationEnabled = false;

            Resultado res = new Resultado();
            object resul = null;
            try
            {
                if (opcion == 1)
                {
                    string[] parametros = filtro.Split('|'); 
                    string user = parametros[0].ToString();
                    string password = parametros[1].ToString();


                    resul = (from a in db.tbl_Personal
                             where a.nombreUsario_personal == user &&  a.contrasenia_personal==password
                             select new
                             {
                                 id_Personal =a.id_personal,
                                 a.nroDoc_personal,
                                 a.apellidos_personal,
                                 a.nombres_personal,
                                 a.envio_enlinea_personal
                             }).ToList();   
                }
                else if (opcion == 2)
                { 
                        string[] parametros = filtro.Split('|');
                        string correo = parametros[0].ToString();


          
                        db.Configuration.ProxyCreationEnabled = false;
                        var verificacion = db.tbl_Personal.Where(x => x.email_personal == correo).FirstOrDefault<tbl_Personal>();

                        if (verificacion != null)
                        {
                            if (ModelState.IsValid)
                            {
                                var body = "<center><h1>Recuperación de Contraseña</h1></center> <p> Usuario : " + verificacion.nombreUsario_personal + ", su contraseña es : " + verificacion.contrasenia_personal +
                                    "</p><p>Ingrese al sistema : <h3><a href='http://www.cobraperu.com/controlasistencia'>Control de Asistencias</a></h3></p><p></p><p></p><p>Atte.</p><p>Administrador Web</p><p>Dsige</p>";
                                var message = new MailMessage();
                                message.To.Add(new MailAddress(verificacion.email_personal));
                                message.From = new MailAddress("cobralecturas@gmail.com");
                                message.Subject = "Recuperación de Correo - Sistema de Dsige";
                                message.Body = body;
                                message.IsBodyHtml = true;

                                using (var smtp = new SmtpClient())
                                {
                                    var credential = new NetworkCredential
                                    {
                                        UserName = "cobralecturas@gmail.com",
                                        Password = "A.123456"
                                    };
                                    smtp.Credentials = credential;
                                    smtp.Host = "smtp.gmail.com";
                                    smtp.Port = 587;
                                    smtp.EnableSsl = true;
                                    smtp.Send(message);

                                }
                                resul = "OK";
                            }
                            else
                            {
                                resul = "FALLO";
                            }
                        }
                        else
                        {
                            resul = "0";
                        }      
                }

                else if (opcion == 3)
                {
                    string[] parametros = filtro.Split('|');
                    string login = parametros[0].ToString();
                    string contra = parametros[1].ToString();

                    var flagLogin = db.tbl_Usuarios.Count(e => e.login_usuario == login && e.contrasenia_usuario == contra);

                    if (flagLogin == 0)
                    {
                        res.ok = false;
                        res.data = "El usuario y/o contraseña no son correctos, verifique ";
                        resul = res;
                    }
                    else
                    {
                        var Parents = new string[] { "1" };
                        tbl_Usuarios objUsuario = db.tbl_Usuarios.Where(p => p.login_usuario == login && p.contrasenia_usuario == contra).SingleOrDefault();

                        Menu listamenu = new Menu();
                        List<MenuPermisos> listaAccesos = new List<MenuPermisos>();

                        var listaMenu = (from w in db.tbl_AccesoOpciones
                                         join od in db.tbl_Definicion_Opciones on w.id_opcion equals od.id_opcion
                                         join u in db.tbl_Usuarios on w.id_personal equals u.id_Usuario
                                         where u.id_Usuario == objUsuario.id_Usuario && Parents.Contains(od.parentID.ToString()) && od.estado == 1
                                         orderby od.orden_Opcion ascending
                                         select new
                                         {
                                             id_opcion = w.id_opcion,
                                             id_usuarios = w.id_personal,
                                             nombre_principal = od.nombre_opcion,
                                             parent_id_principal = od.parentID,
                                             urlmagene_principal = od.urlImagen_Opcion
                                         }).Distinct();

                        foreach (var item in listaMenu)
                        {
                            MenuPermisos listaJsonObj = new MenuPermisos();

                            listaJsonObj.id_opcion = Convert.ToInt32(item.id_opcion);
                            listaJsonObj.id_usuarios = Convert.ToInt32(item.id_usuarios);
                            listaJsonObj.nombre_principal = item.nombre_principal;
                            listaJsonObj.parent_id_principal = Convert.ToInt32(item.parent_id_principal);
                            listaJsonObj.urlmagene_principal = item.urlmagene_principal;
                            listaJsonObj.listMenu = (from w in db.tbl_AccesoOpciones
                                                     join od in db.tbl_Definicion_Opciones on w.id_opcion equals od.id_opcion
                                                     join u in db.tbl_Usuarios on w.id_personal equals u.id_Usuario
                                                     where u.id_Usuario == objUsuario.id_Usuario && od.parentID == item.id_opcion && od.estado == 1
                                                     orderby od.orden_Opcion ascending
                                                     select new
                                                     {
                                                         nombre_page = od.nombre_opcion,
                                                         url_page = od.url_opcion,
                                                         orden = od.orden_Opcion
                                                     })
                                            .ToList()
                                            .Distinct();

                            listaAccesos.Add(listaJsonObj);
                        }

                        listamenu.menuPermisos = listaAccesos;
                        //listamenu.menuEventos = get_AccesoEventos(objUsuario.id_usuario);
                        listamenu.menuEventos = null;
                        listamenu.id_usuario = objUsuario.id_Usuario;
                        listamenu.nombre_usuario = objUsuario.apellidos_usuario + " " + objUsuario.nombres_usuario;
                        listamenu.id_perfil = Convert.ToInt32(objUsuario.id_Perfil);

                        res.ok = true;
                        res.data = listamenu;

                        resul = res;

                    }
                }
                else if (opcion == 4)
                {

                    var Parents = new string[] { "1" };
                    MenuAcceso listamenuAcceso = new MenuAcceso();
                    List<MenuPermisosAcceso> listaAccesos = new List<MenuPermisosAcceso>();

                    var listaMenu = (from od in db.tbl_Definicion_Opciones
                                     where Parents.Contains(od.parentID.ToString()) && od.estado == 1  
                                     select new
                                     {
                                         id_Opcion = od.id_opcion,
                                         od.nombre_opcion
                                     }).Distinct();

                    foreach (var item in listaMenu)
                    {
                        MenuPermisosAcceso listaJsonObj = new MenuPermisosAcceso();

                        listaJsonObj.text = item.nombre_opcion;
                        listaJsonObj.value = item.id_Opcion;
                        listaJsonObj.children = (from od in db.tbl_Definicion_Opciones
                                                 where od.parentID == item.id_Opcion && od.estado == 1
                                                 select new
                                                 {
                                                     text = od.nombre_opcion,
                                                     value = od.id_opcion,
                                                     Checked = false
                                                 })
                                        .Distinct()
                                        .ToList();
                        listaAccesos.Add(listaJsonObj);
                    }

                    listamenuAcceso.text = "SISTEMA PERT";
                    listamenuAcceso.value = "-1";
                    listamenuAcceso.children = listaAccesos;

                    res.ok = true;
                    res.data = listamenuAcceso;
 

                    resul = res;
                }
                else if (opcion == 5)
                {
                    res.ok = true;
                    res.data = (from a in db.tbl_Usuarios
                                where a.estado == 1
                                orderby a.id_Usuario ascending
                                select new
                                {
                                    checkeado = false,
                                    a.id_Usuario,
                                    a.nrodoc_usuario,
                                    apellidos_usuario = a.apellidos_usuario + " " + a.nombres_usuario,
                                    a.nombres_usuario
                                }).ToList();
                    resul = res;
                }
                else if (opcion == 6) //----- PERFILES
                {
                    res.ok = true;
                    res.data = (from a in db.tbl_Perfil
                                where a.estado == 1
                                select new
                                {
                                    checkeando = false,
                                    a.id_perfil,
                                    descripcion_perfil = a.des_perfil
                                }).ToList();
                    resul = res;
                }
                else if (opcion == 7)
                {
                    string[] parametros = filtro.Split('|');
                    string idOpciones = parametros[0].ToString();

                    Mantenimientos_BL obj_negocio = new Mantenimientos_BL();

                    res.ok = true;
                    res.data = obj_negocio.get_usuariosAccesos(idOpciones);
                    resul = res;
                }
                else if (opcion == 8)
                {
                    string[] parametros = filtro.Split('|');
                    string idOpciones = parametros[0].ToString();
                    int idUsuario = Convert.ToInt32(parametros[1].ToString());

                    Mantenimientos_BL obj_negocio = new Mantenimientos_BL();

                    res.ok = true;
                    res.data = obj_negocio.get_eventosUsuarioMarcados(idOpciones, idUsuario);
     
                    resul = res;
                }
                else if (opcion == 9)
                {

                    string[] parametros = filtro.Split('|');
                    string idOpciones = parametros[0].ToString();
                    string idEventos = parametros[1].ToString();
                    int idPrincipal = Convert.ToInt32(parametros[2].ToString());
                    string modalElegido = parametros[3].ToString();

                    Mantenimientos_BL obj_negocio = new Mantenimientos_BL();

                    res.ok = true;
                    if (modalElegido == "usuarios")
                    {
                        res.data = obj_negocio.set_grabandoEventos(idOpciones, idEventos, idPrincipal);
                    }
                    //if (modalElegido == "perfiles")
                    //{
                    //    res.data = obj_negocios.set_grabandoEventosPerfiles(idOpciones, idEventos, idPrincipal);
                    //}
 
                    resul = res;

                }
                else if (opcion == 10)
                {
                    string[] parametros = filtro.Split('|');
                    string idOpciones = parametros[0].ToString();
                    int idUsuario = Convert.ToInt32(parametros[1].ToString());

                    Mantenimientos_BL obj_negocio = new Mantenimientos_BL();

                    res.ok = true;
                    res.data = obj_negocio.set_eliminarAccesos(idOpciones, idUsuario);
 
                    resul = res;

                }
                else
                {
                    resul = "Opcion seleccionada invalida";
                }

            }
            catch (Exception ex)
            {
                resul = ex.Message;
            }
            return resul;
        }
         

    }
}
