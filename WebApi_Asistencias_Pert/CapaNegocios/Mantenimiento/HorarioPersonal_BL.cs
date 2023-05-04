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
    public class HorarioPersonal_BL  
    {
        string cadenaCnx = System.Configuration.ConfigurationManager.ConnectionStrings["conexionDsige"].ConnectionString;
        

        public string Insert_Horario_Personal(int id_personal, string lunes_I , string lunes_S , string Martes_I , string  Martes_S , string  Miercoles_I, string  Miercoles_S , string  Jueves_I , string  Jueves_S , string Viernes_I,  string Viernes_S,  string Sabado_I, string  Sabado_S ,  string Domingo_I, string Domingo_S)
        {
            string Resultado = "";
            int id_asist = 0;
            try
            {
                using (SqlConnection cn = new SqlConnection(cadenaCnx))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand("SP_I_HORARIO_PERSONAL", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id_personal", SqlDbType.Int).Value = id_personal;
                        cmd.Parameters.Add("@lunes_I", SqlDbType.VarChar).Value = lunes_I;
                        cmd.Parameters.Add("@lunes_S", SqlDbType.VarChar).Value = lunes_S;
                        cmd.Parameters.Add("@Martes_I", SqlDbType.VarChar).Value = Martes_I;
                        cmd.Parameters.Add("@Martes_S", SqlDbType.VarChar).Value = Martes_S;
                        cmd.Parameters.Add("@Miercoles_I", SqlDbType.VarChar).Value = Miercoles_I;
                        cmd.Parameters.Add("@Miercoles_S", SqlDbType.VarChar).Value = Miercoles_S;
                        cmd.Parameters.Add("@Jueves_I", SqlDbType.VarChar).Value = Jueves_I;
                        cmd.Parameters.Add("@Jueves_S", SqlDbType.VarChar).Value = Jueves_S;
                        cmd.Parameters.Add("@Viernes_I", SqlDbType.VarChar).Value = Viernes_I;
                        cmd.Parameters.Add("@Viernes_S", SqlDbType.VarChar).Value = Viernes_S;
                        cmd.Parameters.Add("@Sabado_I", SqlDbType.VarChar).Value = Sabado_I;
                        cmd.Parameters.Add("@Sabado_S", SqlDbType.VarChar).Value = Sabado_S;
                        cmd.Parameters.Add("@Domingo_I", SqlDbType.VarChar).Value = Domingo_I;
                        cmd.Parameters.Add("@Domingo_S", SqlDbType.VarChar).Value = Domingo_S;
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

        public string Update_Horario_Personal(int id_personal, string lunes_I, string lunes_S, string Martes_I, string Martes_S, string Miercoles_I, string Miercoles_S, string Jueves_I, string Jueves_S, string Viernes_I, string Viernes_S, string Sabado_I, string Sabado_S, string Domingo_I, string Domingo_S)
        {
            string Resultado = "";
            int id_asist = 0;
            try
            {
                using (SqlConnection cn = new SqlConnection(cadenaCnx))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand("SP_U_HORARIO_PERSONAL", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id_personal", SqlDbType.Int).Value = id_personal;
                        cmd.Parameters.Add("@lunes_I", SqlDbType.VarChar).Value = lunes_I;
                        cmd.Parameters.Add("@lunes_S", SqlDbType.VarChar).Value = lunes_S;
                        cmd.Parameters.Add("@Martes_I", SqlDbType.VarChar).Value = Martes_I;
                        cmd.Parameters.Add("@Martes_S", SqlDbType.VarChar).Value = Martes_S;
                        cmd.Parameters.Add("@Miercoles_I", SqlDbType.VarChar).Value = Miercoles_I;
                        cmd.Parameters.Add("@Miercoles_S", SqlDbType.VarChar).Value = Miercoles_S;
                        cmd.Parameters.Add("@Jueves_I", SqlDbType.VarChar).Value = Jueves_I;
                        cmd.Parameters.Add("@Jueves_S", SqlDbType.VarChar).Value = Jueves_S;
                        cmd.Parameters.Add("@Viernes_I", SqlDbType.VarChar).Value = Viernes_I;
                        cmd.Parameters.Add("@Viernes_S", SqlDbType.VarChar).Value = Viernes_S;
                        cmd.Parameters.Add("@Sabado_I", SqlDbType.VarChar).Value = Sabado_I;
                        cmd.Parameters.Add("@Sabado_S", SqlDbType.VarChar).Value = Sabado_S;
                        cmd.Parameters.Add("@Domingo_I", SqlDbType.VarChar).Value = Domingo_I;
                        cmd.Parameters.Add("@Domingo_S", SqlDbType.VarChar).Value = Domingo_S;
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
