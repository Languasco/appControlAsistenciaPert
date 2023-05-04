using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos.Proceso
{
   public  class Registro_Produccion_E
    {
       public int id_Ingreso { get; set; }
       public int id_Local { get; set; }
       public int id_OTContable { get; set; }
       public string descripcion_OTContable { get; set; }
       public int id_Personal_Responsable { get; set; }
       public int id_personal { get; set; }
       public string nroDoc_personal { get; set; }
       public string personal { get; set; }
       public string cargo { get; set; }
       public decimal ImporteProduccion_Ingreso { get; set; }
       public string obsProduccion_Ingreso { get; set; }
       public decimal ImporteMovilidad_Ingreso { get; set; }
       public string obsMovilidad_Ingreso { get; set; }
       public string fecha_ingreso { get; set; }
       public int usuario_creacion { get; set; }
       public int estado { get; set; }
       public string fecha_creacion { get; set; }
       public int usuario_edicion  { get; set; }
       public string fecha_edicion { get; set; }


    }
}
