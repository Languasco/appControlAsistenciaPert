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
    
    public partial class tbl_IngresoPersonal
    {
        public int id_Ingreso { get; set; }
        public Nullable<int> id_Local { get; set; }
        public Nullable<int> id_OTContable { get; set; }
        public Nullable<int> id_Personal_Responsable { get; set; }
        public Nullable<int> id_personal { get; set; }
        public Nullable<decimal> ImporteProduccion_Ingreso { get; set; }
        public string obsProduccion_Ingreso { get; set; }
        public Nullable<decimal> ImporteMovilidad_Ingreso { get; set; }
        public string obsMovilidad_Ingreso { get; set; }
        public Nullable<System.DateTime> fecha_ingreso { get; set; }
        public Nullable<int> estado { get; set; }
        public Nullable<int> usuario_creacion { get; set; }
        public Nullable<System.DateTime> fecha_creacion { get; set; }
        public Nullable<int> usuario_edicion { get; set; }
        public Nullable<System.DateTime> fecha_edicion { get; set; }
    
        public virtual tbl_Ges_Local tbl_Ges_Local { get; set; }
        public virtual tbl_Ges_OT_Contable tbl_Ges_OT_Contable { get; set; }
        public virtual tbl_Personal tbl_Personal { get; set; }
    }
}
