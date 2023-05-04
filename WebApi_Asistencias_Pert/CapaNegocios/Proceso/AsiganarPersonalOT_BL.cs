using CapaDatos.Proceso;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocios.Proceso
{
    public class AsiganarPersonalOT_BL
    {
        string cadenaCnx = System.Configuration.ConfigurationManager.ConnectionStrings["conexionDsige"].ConnectionString;

        public string Insert_AsignarPersonalOT(string codPersonal, int id_ot, int id_coordinador, int id_usuario)
        {
            string Resultado = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cadenaCnx))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand("SP_I_U_ASIGNAR_PERSONAL_OT", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@codPersonal", SqlDbType.VarChar).Value = codPersonal;
                        cmd.Parameters.Add("@id_ot_Asignar", SqlDbType.Int).Value = id_ot;
                        cmd.Parameters.Add("@id_coordinador", SqlDbType.Int).Value = id_coordinador;
                        cmd.Parameters.Add("@id_usuario", SqlDbType.Int).Value = id_usuario;
                        cmd.ExecuteNonQuery();
                    }  
                }
                Resultado = "OK";
            }
            catch (Exception e)
            {
                Resultado = e.Message;
            }
            return Resultado;
        }


    }
}
