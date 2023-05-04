using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Reporte
{
    public class Reporte_LectorHuellas_E
    {
        public string contrato { get; set; }
        public string fecha { get; set; }
        public string DNI { get; set; }
        public string nombre { get; set; }
        public string CECO { get; set; }
        public string jefeCuadrilla { get; set; }
        
        public string Hora_Inicio { get; set; }
        public string Posicion_Inicio { get; set; }
        public string Hora_Salida { get; set; }
        public string Posicion_Fin { get; set; }
        public string Horas { get; set; }
        public string ID { get; set; }
        public string CECO_Tareo { get; set; }
        
    }
}
