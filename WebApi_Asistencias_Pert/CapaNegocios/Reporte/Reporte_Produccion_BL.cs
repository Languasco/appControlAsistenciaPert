using CapaDatos.Proceso;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocios.Reporte
{
    public class Reporte_Produccion_BL
    {
        string cadenaCnx = System.Configuration.ConfigurationManager.ConnectionStrings["conexionDsige"].ConnectionString;

        public List<Reporte_Produccion_E> Listar_ReporteProduccion(int local, int ot, string fecha, int id_personal_responsable)
        {
            try
            {
                List<Reporte_Produccion_E> obj_list = new List<Reporte_Produccion_E>();
                using (SqlConnection cn = new SqlConnection(cadenaCnx))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_REPORTE_REGISTRO_PRODUCCION", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id_local", SqlDbType.Int).Value = local;
                        cmd.Parameters.Add("@id_ot_contable", SqlDbType.Int).Value = ot;
                        cmd.Parameters.Add("@fecha_Asistencia", SqlDbType.VarChar).Value = fecha;
                        cmd.Parameters.Add("@id_personal_resp", SqlDbType.Int).Value = id_personal_responsable; 


                        DataTable dt_detalle = new DataTable();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt_detalle);
                            foreach (DataRow row in dt_detalle.Rows)
                            {
                                Reporte_Produccion_E Entidad = new Reporte_Produccion_E();

                                Entidad.codigo_personal = row["codigo_personal"].ToString();
                                Entidad.personal = row["personal"].ToString();
                                Entidad.fecha_ingreso = row["fecha_ingreso"].ToString();
                                Entidad.cargo = row["cargo"].ToString();
                                Entidad.ImporteProduccion_Ingreso = Convert.ToDecimal(row["ImporteProduccion_Ingreso"].ToString());
                                Entidad.ImporteMovilidad_Ingreso = Convert.ToDecimal(row["ImporteMovilidad_Ingreso"].ToString());
 
                                Entidad.descripcion_OTContable = row["descripcion_OTContable"].ToString();
                                Entidad.obsProduccion_Ingreso = row["obsProduccion_Ingreso"].ToString();
                                Entidad.obsMovilidad_Ingreso = row["obsMovilidad_Ingreso"].ToString();
                                                              
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
