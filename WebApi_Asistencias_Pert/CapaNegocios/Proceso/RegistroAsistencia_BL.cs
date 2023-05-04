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
    public class RegistroAsistencia_BL
    {
        string cadenaCnx = System.Configuration.ConfigurationManager.ConnectionStrings["conexionDsige"].ConnectionString;

        public List<ControlAsistencia_E> ListarRegistroAsistencia(int local, int ot, string fecha )
        {
            try
            {
                List<ControlAsistencia_E> obj_list = new List<ControlAsistencia_E>();
                using (SqlConnection cn = new SqlConnection(cadenaCnx))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_S_REGISTRO_ASISTENCIA", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id_local", SqlDbType.Int).Value = local;
                        cmd.Parameters.Add("@id_ot_contable", SqlDbType.Int).Value = ot;
                        cmd.Parameters.Add("@fecha_Asistencia", SqlDbType.VarChar).Value = fecha;
 

                        DataTable dt_detalle = new DataTable();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt_detalle);
                            foreach (DataRow row in dt_detalle.Rows)
                            {
                                ControlAsistencia_E Entidad = new ControlAsistencia_E();         
                                Entidad.id_asistencia = Convert.ToInt32(row["id_asistencia"]);
                                Entidad.id_Local = Convert.ToInt32(row["id_Local"]);
                                Entidad.id_OTContable = Convert.ToInt32(row["id_OTContable"]);
                                Entidad.Fecha = row["Fecha"].ToString();
                                Entidad.id_personal = Convert.ToInt32(row["id_personal"]);
                                Entidad.dni = row["dni"].ToString();
                                Entidad.personal = row["personal"].ToString();
                                Entidad.id_tasi_codigo = row["id_tasi_codigo"].ToString();
                                Entidad.horaIngreso_Asistencia = row["horaIngreso_Asistencia"].ToString();
                                Entidad.horaSalida_Asistencia = row["horaSalida_Asistencia"].ToString();
                                Entidad.id_turno =row["id_turno"].ToString();
                                Entidad.observacion_asistencia = row["observacion_asistencia"].ToString();
                               obj_list.Add(Entidad);
                            }
                        }
                    }
                }

                return obj_list;
            }
            catch (Exception e)
            {
                throw e;
            }
        }


        public string Insert_Asistencia(int id_asistencia, int id_Local, int id_OTContable, string Fecha, int id_personal, string id_tasi_codigo, string horaIngreso, string horaSalida, int id_turno, string observacion, int usuario)
        {
            string Resultado = "";
            int id_asist = 0;
            try
            {
                using (SqlConnection cn = new SqlConnection(cadenaCnx))
                {
                    cn.Open();

                    using (SqlCommand cmd = new SqlCommand("SP_I_U_REGISTRO_ASISTENCIA", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id_asistencia", SqlDbType.Int).Value = id_asistencia;
                        cmd.Parameters.Add("@id_Local", SqlDbType.Int).Value = id_Local;
                        cmd.Parameters.Add("@id_OTContable", SqlDbType.Int).Value = id_OTContable;
                        cmd.Parameters.Add("@Fecha", SqlDbType.VarChar).Value = Fecha;
                        cmd.Parameters.Add("@id_personal", SqlDbType.Int).Value = id_personal;
                        cmd.Parameters.Add("@id_tasi_codigo", SqlDbType.VarChar).Value = id_tasi_codigo;
                        cmd.Parameters.Add("@horaIngreso", SqlDbType.VarChar).Value = horaIngreso;
                        cmd.Parameters.Add("@horaSalida", SqlDbType.VarChar).Value = horaSalida;
                        cmd.Parameters.Add("@id_turno", SqlDbType.Int).Value = id_turno;
                        cmd.Parameters.Add("@observacion", SqlDbType.VarChar).Value = observacion;
                        cmd.Parameters.Add("@id_usuario", SqlDbType.VarChar).Value = usuario;
                        cmd.Parameters.Add("@Return_idAsistencia", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.ExecuteNonQuery();
                        id_asist = Convert.ToInt32(cmd.Parameters["@Return_idAsistencia"].Value);      

                    }  
                }
                Resultado = "OK|" + id_asist;
            }
            catch (Exception e)
            {
                Resultado = e.Message;
            }
            return Resultado;
        }


    }
}
