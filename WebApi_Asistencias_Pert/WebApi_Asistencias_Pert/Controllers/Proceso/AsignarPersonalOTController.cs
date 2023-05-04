using CapaDatos;
using CapaNegocios.Proceso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApi_Asistencias_Pert.Controllers.Reporte
{
   [EnableCors("*", "*", "*")]
    public class AsignarPersonalOTController : ApiController
    {
       private CAM_ControlAsistenciaEntities db = new CAM_ControlAsistenciaEntities();

       public object GetAsignarPersonalOT(int opcion, string filtro)
       {
           //filtro puede tomar cualquier valor
           db.Configuration.ProxyCreationEnabled = false;
           object resul = null;

           try
           {
               if (opcion == 1)
               {
                   resul = (from a in db.tbl_Ges_Local
                            select new
                            {
                                a.id_local,
                                a.nombre_local,
                                a.estado
                            }).ToList();
               }
               else if (opcion == 2)
               {
                   resul = (from a in db.tbl_Ges_OT_Contable
                            select new
                            {
                                a.id_OtContable,
                                a.codigo_OTContable,
                                a.descripcion_OTContable
                            }).ToList();
               }
               else if (opcion == 4)
               {

                   string[] parametros = filtro.Split('|');
                   int ot = Convert.ToInt32(parametros[0].ToString());

                   resul = (from a in db.tbl_Personal
                            where (a.id_cargo_personal == 9 || a.id_cargo_personal == 10 || a.id_cargo_personal == 11) && a.id_OtContable == ot
                            select new
                            {
                                a.id_personal,
                                personal = a.apellidos_personal + " " + a.nombres_personal

                            }).ToList();
               }
               else if (opcion == 5)
               {
                   string[] parametros = filtro.Split('|');
                   int ot = Convert.ToInt32(parametros[0].ToString());

                   resul = (from a in db.tbl_Personal
                            join b in db.Tbl_Cargo_Personal on a.id_cargo_personal equals b.id_Cargo
                            where a.id_OtContable == ot
                            select new
                            {
                                a.id_personal,
                                dni= a.nroDoc_personal,
                                personal = a.apellidos_personal + " " + a.nombres_personal,
                                a.id_cargo_personal ,
                                cargo = b.nombre_cargo

                            }).ToList();
               }
               else if (opcion == 6)
               {
                   resul = (from a in db.tbl_Ges_OT_Contable
                            select new
                            {
                                a.id_OtContable,
                                a.codigo_OTContable,
                                a.descripcion_OTContable
                            }).ToList();
               }
               else if (opcion == 7)
               {
                    string[] parametros = filtro.Split('|');        
                    string codPersonal = parametros[0].ToString();
                    int id_ot = Convert.ToInt32(parametros[1].ToString());
                    int id_coordinador= Convert.ToInt32(parametros[2].ToString());
                    int id_usuario = Convert.ToInt32(parametros[3].ToString());

                   AsiganarPersonalOT_BL obj_negocio = new AsiganarPersonalOT_BL();
                   resul = obj_negocio.Insert_AsignarPersonalOT(codPersonal, id_ot, id_coordinador, id_usuario);

               }
               else if (opcion == 8)
               {
                   string[] parametros = filtro.Split('|');
                   int ot = Convert.ToInt32(parametros[0].ToString());
                   int id_coordinador = Convert.ToInt32(parametros[1].ToString());

                   resul = (from c in db.tbl_Asis_Configurar_Asistencia_Campo 
                            join a in db.tbl_Personal on c.id_Personal equals a.id_personal
                            join b in db.Tbl_Cargo_Personal on a.id_cargo_personal equals b.id_Cargo
                            where c.id_OtContable == ot && c.id_Personal_Coordinador == id_coordinador
                            select new
                            {
                                id_personal = c.id_Personal,
                                dni = a.nroDoc_personal,
                                personal = a.apellidos_personal + " " + a.nombres_personal,
                                a.id_cargo_personal,
                                cargo = b.nombre_cargo

                            }).ToList();
               }

               else
               {
                   resul = "Opcion selecciona invalida";
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
