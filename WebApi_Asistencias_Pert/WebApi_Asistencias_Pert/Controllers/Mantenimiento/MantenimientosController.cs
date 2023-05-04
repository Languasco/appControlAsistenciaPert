using CapaDatos;
using CapaDatos.Mantenimento;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace WebApi_Asistencias_Pert.Controllers.Mantenimiento
{
    [EnableCors("*", "*", "*")]
    public class MantenimientosController : ApiController
    {
        private CAM_ControlAsistenciaEntities db = new CAM_ControlAsistenciaEntities();
        public object Gettbl_Vehiculo(int opcion, string filtro)
        {
            Resultado res = new Resultado();
            object resul = null;
            try
            {
                if (opcion == 1)
                {
                    string[] parametros = filtro.Split('|');
                    string turno = parametros[0].ToString().ToUpper();

                    if (db.tbl_Asis_Turno.Count(e => e.nombre_turno.ToUpper() == turno) > 0)
                    {
                        resul = true;
                    }
                    else
                    {
                        resul = false;
                    }
                }
                else if (opcion == 2)
                {
                    string[] parametros = filtro.Split('|');
                    string codigo = parametros[0].ToString().ToUpper();

                    if (db.tbl_Asis_TipoAsistencia.Count(e => e.id_tasi_codigo.ToUpper() == codigo) > 0)
                    {
                        resul = true;
                    }
                    else
                    {
                        resul = false;
                    }
                }
                else
                {
                    res.ok = false;
                    res.data = "Opcion seleccionada invalida";

                    resul = res;
                }
            }
            catch (Exception ex)
            {
                res.ok = false;
                res.data = ex.Message;
                resul = res;
            }
            return resul;
        }



    }
}
