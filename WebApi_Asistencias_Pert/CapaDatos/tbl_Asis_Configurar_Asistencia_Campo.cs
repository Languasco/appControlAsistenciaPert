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
    
    public partial class tbl_Asis_Configurar_Asistencia_Campo
    {
        public int id_ConfigPersonal { get; set; }
        public Nullable<int> id_OtContable { get; set; }
        public Nullable<int> id_Personal_Coordinador { get; set; }
        public Nullable<int> id_Personal { get; set; }
        public Nullable<int> estado { get; set; }
        public int usuario_creacion { get; set; }
        public System.DateTime fecha_creacion { get; set; }
    
        public virtual tbl_Personal tbl_Personal { get; set; }
    }
}