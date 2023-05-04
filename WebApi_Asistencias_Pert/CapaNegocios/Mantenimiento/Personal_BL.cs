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
    public class Personal_BL
    {
        string cadenaCnx = System.Configuration.ConfigurationManager.ConnectionStrings["conexionDsige"].ConnectionString;
        

        public string Insert_BD_Asistencia(int id_personal)
        {
            string Resultado = "";
            int id_asist = 0;
            try
            {
                using (SqlConnection cn = new SqlConnection(cadenaCnx))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand("SP_S_EMPLEADO_REGISTRO_BD_ASISTENCIA", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id_personal", SqlDbType.Int).Value = id_personal; 
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
