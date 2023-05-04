using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Proceso
{
    public class Reporte_Produccion_E
    {
        public string personal { get; set; }
        public string codigo_personal { get; set; }
        
        public string fecha_ingreso { get; set; }
        public string cargo { get; set; }
        public decimal ImporteProduccion_Ingreso { get; set; }
        public decimal ImporteMovilidad_Ingreso { get; set; }
        public string descripcion_OTContable { get; set; }
        public string obsProduccion_Ingreso { get; set; }
        public string obsMovilidad_Ingreso { get; set; }
    }
}
