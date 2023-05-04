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
    public class Reporte_ControlAsistenciaCampo_BL 
    {
        string cadenaCnx = System.Configuration.ConfigurationManager.ConnectionStrings["conexionDsige"].ConnectionString;

        public List<Reporte_ControlAsistenciaCampo_E> Listar_ControlAsistencia_Mapa(int local, int ot, string fecha, int id_personal_responsable)
        {
            try
            {
                List<Reporte_ControlAsistenciaCampo_E> obj_list = new List<Reporte_ControlAsistenciaCampo_E>();
                using (SqlConnection cn = new SqlConnection(cadenaCnx))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_S_ASISTENCIA_CAMPO_MAPA", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id_local", SqlDbType.Int).Value = local;
                        cmd.Parameters.Add("@ot_contable", SqlDbType.Int).Value = ot;
                        cmd.Parameters.Add("@fecha_asistencia", SqlDbType.VarChar).Value = fecha;
                        cmd.Parameters.Add("@id_responsable", SqlDbType.Int).Value = id_personal_responsable; 


                        DataTable dt_detalle = new DataTable();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt_detalle);
                            foreach (DataRow row in dt_detalle.Rows)
                            {
                                Reporte_ControlAsistenciaCampo_E Entidad = new Reporte_ControlAsistenciaCampo_E();
 
                                Entidad.id_personal = Convert.ToInt32(row["id_personal"].ToString());           
                                Entidad.codigo_personal = row["codigo_personal"].ToString();
                                Entidad.nroDoc_personal = row["nroDoc_personal"].ToString();
                                Entidad.email_personal = row["email_personal"].ToString();
                                Entidad.personal = row["personal"].ToString();
                                Entidad.SerieEquipo = row["SerieEquipo"].ToString();
                                Entidad.id_OtContable = Convert.ToInt32(row["id_OtContable"].ToString());   
                                Entidad.codigo_OTContable = row["codigo_OTContable"].ToString();
                                Entidad.descripcion_OTContable = row["descripcion_OTContable"].ToString();
                                Entidad.Latitud_asistenciaCampo = row["Latitud_asistenciaCampo"].ToString();
                                Entidad.Longitud_asistenciaCampo = row["Longitud_asistenciaCampo"].ToString();
                                Entidad.fechaHora_asistenciaCampo = row["fechaHora_asistenciaCampo"].ToString();
                                 
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

        public List<Reporte_ControlAsistenciaCampo_E> Listar_ControlAsistencia_Mapa_Agrupado(int local, int ot, string fecha, int id_personal_responsable)
        {
            try
            {
                List<Reporte_ControlAsistenciaCampo_E> obj_list = new List<Reporte_ControlAsistenciaCampo_E>();
                using (SqlConnection cn = new SqlConnection(cadenaCnx))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_S_ASISTENCIA_CAMPO_MAPA_AGRUPADO", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id_local", SqlDbType.Int).Value = local;
                        cmd.Parameters.Add("@ot_contable", SqlDbType.Int).Value = ot;
                        cmd.Parameters.Add("@fecha_asistencia", SqlDbType.VarChar).Value = fecha;
                        cmd.Parameters.Add("@id_responsable", SqlDbType.Int).Value = id_personal_responsable;


                        DataTable dt_detalle = new DataTable();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt_detalle);
                            foreach (DataRow row in dt_detalle.Rows)
                            {
                                Reporte_ControlAsistenciaCampo_E Entidad = new Reporte_ControlAsistenciaCampo_E();           
 
                                Entidad.Longitud_asistenciaCampo = row["Longitud_asistenciaCampo"].ToString();
                                Entidad.Latitud_asistenciaCampo = row["Latitud_asistenciaCampo"].ToString();
                                Entidad.Cantidad = Convert.ToInt32(row["Cantidad"].ToString());

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


    }
}
