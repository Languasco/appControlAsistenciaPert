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
    
    public partial class tbl_Usuarios
    {
        public int id_Usuario { get; set; }
        public string nrodoc_usuario { get; set; }
        public string apellidos_usuario { get; set; }
        public string nombres_usuario { get; set; }
        public string email_usuario { get; set; }
        public Nullable<int> id_Cargo { get; set; }
        public string tipo_usuario { get; set; }
        public byte[] fotobinary { get; set; }
        public string fotourl { get; set; }
        public string login_usuario { get; set; }
        public string contrasenia_usuario { get; set; }
        public Nullable<int> id_Perfil { get; set; }
        public Nullable<int> estado { get; set; }
        public Nullable<int> usuario_creacion { get; set; }
        public Nullable<System.DateTime> fecha_creacion { get; set; }
        public Nullable<int> usuario_edicion { get; set; }
        public Nullable<System.DateTime> fecha_edicion { get; set; }
    }
}