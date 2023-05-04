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
    public class Reporte_LectorHuellasController : ApiController
    {
       private CAM_ControlAsistenciaEntities db = new CAM_ControlAsistenciaEntities();

       public object GetReporte_LectorHuellas(int opcion, string filtro)
       {
           //filtro puede tomar cualquier valor
           db.Configuration.ProxyCreationEnabled = false;
           object resul = null;

           try
           {
               if (opcion == 0)  // mostrando para la Web
               {
                   resul = (from c in db.tbl_Personal
                           where c.id_cargo_personal == 11
                            select new
                            {
                                c.id_personal,
                                c.apellidos_personal,
                                c.nombres_personal
                            }).ToList();

               }
              else if (opcion == 1)  // mostrando para la Web
               {
                   string[] parametros = filtro.Split('|');
                   int local = Convert.ToInt32(parametros[0].ToString());
                   int id_personal = Convert.ToInt32(parametros[1].ToString());
                   int id_turno = Convert.ToInt32(parametros[2].ToString());
                   string fecha_ini = parametros[3].ToString();
                   string fecha_fin = parametros[4].ToString();
                   int opcion_reporte = Convert.ToInt32(parametros[5].ToString());

                   Reporte_LectorHuellas_BL obj_negocio = new Reporte_LectorHuellas_BL();
                   resul = obj_negocio.Listar_ReporteDetalle(local, id_personal, id_turno, fecha_ini, fecha_fin, opcion_reporte);

               }
               else if (opcion == 2) /// Generando el excel
               {
                   string[] parametros = filtro.Split('|');
                   int local = Convert.ToInt32(parametros[0].ToString());
                   int id_personal = Convert.ToInt32(parametros[1].ToString());
                   int id_turno = Convert.ToInt32(parametros[2].ToString());
                   string fecha_ini = parametros[3].ToString();
                   string fecha_fin = parametros[4].ToString();
                   int opcion_reporte = Convert.ToInt32(parametros[5].ToString());

                   Reporte_LectorHuellas_BL obj_negocio = new Reporte_LectorHuellas_BL();
                   resul = obj_negocio.ExportarExcel_Reporte(local, id_personal, id_turno, fecha_ini, fecha_fin, opcion_reporte);

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
