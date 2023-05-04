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
    public class TareoMasivoController : ApiController
    {
       private CAM_ControlAsistenciaEntities db = new CAM_ControlAsistenciaEntities();

       public object GetTareoMasivo(int opcion, string filtro)
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
               else if (opcion == 3)
               {

                   string[] parametros = filtro.Split('|');
                 
                   int local = Convert.ToInt32(parametros[0].ToString());
                   int OT_contable = Convert.ToInt32(parametros[1].ToString());
                   int anio = Convert.ToInt32(parametros[2].ToString());
                   int mes = Convert.ToInt32(parametros[3].ToString());

                   TareaMasivo_BL obj_negocio = new TareaMasivo_BL();
                  resul = obj_negocio.Listar_TareoMasivo(local, OT_contable, anio, mes);
               }
               else if (opcion == 4)
               {
                   resul = (from a in db.tbl_Anio
                            select new
                            {
                                a.id_anio,
                                a.descripcion_anio
                            }).ToList();
               }
               else if (opcion == 5)
               {
                   resul = (from a in db.tbl_Mes 
                            select new
                            {
                                a.id_mes,
                                a.descripcion_mes,
 
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
