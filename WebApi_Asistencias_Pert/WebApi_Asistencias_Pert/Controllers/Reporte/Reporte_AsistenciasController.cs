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
    public class Reporte_AsistenciasController : ApiController
    {
         public object GetReporteAsistenciasMesAño(int tipo,int mes,int año)
         {
             object listReport = null;
             Reporte_Asistencias_BL ReporteAsistenciasBL = new Reporte_Asistencias_BL();
             listReport = ReporteAsistenciasBL.Lista_ReporteAsistenciasMesAño(mes,año);
             return listReport;
         }
    }
}
