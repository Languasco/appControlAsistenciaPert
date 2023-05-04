using CapaDatos.Reporte;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocios.Reporte
{
    public class Reporte_Asistencias_BL
    {
        string cadenaCnx = System.Configuration.ConfigurationManager.ConnectionStrings["conexionDsige"].ConnectionString;

        public object Lista_ReporteAsistenciasMesAño(int mes, int año)
        {
            try
            {
                List<Reporte_Asistencias_E> obj_list = new List<Reporte_Asistencias_E>();
                using (SqlConnection cn = new SqlConnection(cadenaCnx))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_GET_REPORTE_ASISTENCIAS_MESAÑO", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@MES", SqlDbType.Int).Value = mes;
                        cmd.Parameters.Add("@AÑO", SqlDbType.Int).Value = año;

                        DataTable dt_detalle = new DataTable();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt_detalle);
                            foreach (DataRow row in dt_detalle.Rows)
                            {
                                Reporte_Asistencias_E Entidad = new Reporte_Asistencias_E();
                                Entidad.Personal = row["PERSONAL"].ToString();
                                Entidad.Cargo = row["CARGO"].ToString();
                                Entidad.Fecha_Asis = row["FECHA_ASIS"].ToString();
                                Entidad.Codigo_Asis = row["CODIGO_ASIS"].ToString();
                                Entidad.id_Personal = Convert.ToInt32(row["ID_PERSONAL"]);
                                Entidad.Mes = row["MES"].ToString();
                                Entidad.Año = row["AÑO"].ToString();
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
