//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CapaDatos
{
    using System;
    using System.Collections.Generic;
    
    public partial class TBL_EstadoCelular
    {
        public long id_estadocelular { get; set; }
        public int id_operario { get; set; }
        public bool GpsActivo { get; set; }
        public int estadoBateria { get; set; }
        public System.DateTime FechaHoraAndroid { get; set; }
        public System.DateTime FechaAgregaRegistro { get; set; }
        public Nullable<int> ModoAvion { get; set; }
        public Nullable<bool> PlanDatos { get; set; }
        public string latitud { get; set; }
        public string longitud { get; set; }
    }
}
