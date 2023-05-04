using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Proceso
{
    public class Reporte_Tareo_Diariol_E
    {
        public int id_asistencia { get; set; }
        public int id_Local { get; set; }
        public int id_OTContable { get; set; }
        public string Fecha { get; set; }
        public int id_personal { get; set; }
        public string dni { get; set; }
        public string personal { get; set; }
        public string id_tasi_codigo { get; set; }
        public string horaIngreso_Asistencia { get; set; }
        public string horaSalida_Asistencia { get; set; }
        public string observacion_asistencia { get; set; }
        public string id_turno { get; set; }
        public string total_Asistencia { get; set; }
        
    }
}
