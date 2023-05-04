using CapaDatos;
using CapaNegocios.Proceso;
using CapaNegocios.Reporte;
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
    public class Reporte_ControlAsistenciaCampoController : ApiController
    {
       private CAM_ControlAsistenciaEntities db = new CAM_ControlAsistenciaEntities();

       public object GetRegistro_Produccion(int opcion, string filtro)
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
                   string fechaAsistencia = parametros[2].ToString();
                   int id_personalResponsable = Convert.ToInt32(parametros[3].ToString());

                   Reporte_ControlAsistenciaCampo_BL obj_negocio = new Reporte_ControlAsistenciaCampo_BL();
                   resul = obj_negocio.Listar_ControlAsistencia_Mapa(local, OT_contable, fechaAsistencia, id_personalResponsable);

               }
               else if (opcion == 4)
               {
                   resul = (from a in db.tbl_Personal
                            where a.id_cargo_personal == 11
                            select new
                            {
                                a.id_personal,
                                personal = a.apellidos_personal + " " + a.nombres_personal
                            }).ToList();
               }
               else if (opcion == 5)
               {

                   string[] parametros = filtro.Split('|');

                   int local = Convert.ToInt32(parametros[0].ToString());
                   int OT_contable = Convert.ToInt32(parametros[1].ToString());
                   string fechaAsistencia = parametros[2].ToString();
                   int id_personalResponsable = Convert.ToInt32(parametros[3].ToString());

                   Reporte_ControlAsistenciaCampo_BL obj_negocio = new Reporte_ControlAsistenciaCampo_BL();
                   resul = obj_negocio.Listar_ControlAsistencia_Mapa_Agrupado(local, OT_contable, fechaAsistencia, id_personalResponsable);

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
