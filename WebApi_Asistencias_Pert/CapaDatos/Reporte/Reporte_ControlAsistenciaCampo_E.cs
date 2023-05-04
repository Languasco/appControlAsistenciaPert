using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Proceso
{
    public class Reporte_ControlAsistenciaCampo_E
    {
        public int id_personal { get; set; }
        public string codigo_personal { get; set; }
        public string nroDoc_personal { get; set; }
        public string email_personal { get; set; }
        public string personal { get; set; }
        public string SerieEquipo { get; set; }
        public int  id_OtContable { get; set; }
        public string codigo_OTContable { get; set; }
        public string descripcion_OTContable { get; set; }
        public string Latitud_asistenciaCampo { get; set; }
        public string Longitud_asistenciaCampo { get; set; }
        public string fechaHora_asistenciaCampo { get; set; }
        public int Cantidad { get; set; }

        
    }
}
