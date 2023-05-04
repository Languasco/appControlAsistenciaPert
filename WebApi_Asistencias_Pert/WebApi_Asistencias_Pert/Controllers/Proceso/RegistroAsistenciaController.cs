using CapaDatos;
using CapaNegocios.Proceso;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApi_Asistencias_Pert.Controllers.Proceso
{
    [EnableCors("*", "*", "*")]
    public class RegistroAsistenciaController : ApiController
    {
        private CAM_ControlAsistenciaEntities db = new CAM_ControlAsistenciaEntities();
        public object GetRegistroAsistencia(int opcion, string filtro)
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
                                 a.id_OtContable ,
                                 a.codigo_OTContable,
                                 a.descripcion_OTContable
                             }).ToList();
                }
                else if (opcion == 3)
                {

                    string[] parametros = filtro.Split('|');
 
                    int local =Convert.ToInt32(parametros[0].ToString());
                    int OT_contable = Convert.ToInt32(parametros[1].ToString());
                    string fechaAsistencia = parametros[2].ToString();

                    RegistroAsistencia_BL obj_negocio = new RegistroAsistencia_BL();
                    resul = obj_negocio.ListarRegistroAsistencia(local, OT_contable, fechaAsistencia);

                }
                else if (opcion == 4)
                {
                    string[] parametros = filtro.Split('|');

                    int id_asistencia = Convert.ToInt32(parametros[0].ToString());
                    int id_Local = Convert.ToInt32(parametros[1].ToString());
                    int id_OTContable = Convert.ToInt32(parametros[2].ToString());
                    string Fecha = parametros[3].ToString();
                    int id_personal = Convert.ToInt32(parametros[4].ToString());
                    string id_tasi_codigo = parametros[5].ToString();
                    string horaIngreso = parametros[6].ToString();
                    string horaSalida = parametros[7].ToString();
                    int id_turno = Convert.ToInt32(parametros[8].ToString());
                    string observacion = parametros[9].ToString();
                    int usuario = Convert.ToInt32(parametros[10].ToString());

                    RegistroAsistencia_BL obj_negocio = new RegistroAsistencia_BL();
                    resul = obj_negocio.Insert_Asistencia(id_asistencia, id_Local, id_OTContable, Fecha, id_personal, id_tasi_codigo, horaIngreso, horaSalida, id_turno, observacion, usuario);

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
