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
    
    public partial class tbl_Asis_Asistencia_Dispositivos
    {
        public int id_asistencia_dispo { get; set; }
        public int id_dispositivo { get; set; }
        public int USERID { get; set; }
        public System.DateTime CHECKTIME { get; set; }
        public string CHECKTYPE { get; set; }
        public int VERIFYCODE { get; set; }
        public string SENSORID { get; set; }
        public int LOGID { get; set; }
        public int MachineId { get; set; }
        public int UserExtFmt { get; set; }
        public int WorkCode { get; set; }
        public string Memoinfo { get; set; }
        public string sn { get; set; }
        public Nullable<int> Badgenumber { get; set; }
    }
}
