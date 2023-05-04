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
    public class GenerarTXT_IngresoTrabajadorController : ApiController
    {
       private CAM_ControlAsistenciaEntities db = new CAM_ControlAsistenciaEntities();

       public object GetIngresoTrabajadorTxT(int opcion, string filtro)
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
                   int id_personalResponsable = Convert.ToInt32(parametros[2].ToString());
                   string fecha_ini = parametros[3].ToString();
                   string fecha_fin  = parametros[4].ToString();
                   string tipoReporte = parametros[5].ToString();

                   GenerarTxT_IngresoTrabajador_BL obj_negocio = new GenerarTxT_IngresoTrabajador_BL();
                   resul = obj_negocio.Listar_GenerarTxT_IngresosTrabajador(local, OT_contable, id_personalResponsable, fecha_ini , fecha_fin, tipoReporte );

               }

               else if (opcion == 4)
               {

                   resul = (from a in db.tbl_Personal
                            where a.id_cargo_personal == 10 || a.id_cargo_personal == 8
                            select new
                            {
                                a.id_personal,
                                personal = a.apellidos_personal + " " + a.nombres_personal

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
