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
    public class Registro_Produccion_BL
    {
        string cadenaCnx = System.Configuration.ConfigurationManager.ConnectionStrings["conexionDsige"].ConnectionString;

        public List<Registro_Produccion_E> Listar_RegistroProduccion(int local, int ot, string fecha)
        {
            try
            {
                List<Registro_Produccion_E> obj_list = new List<Registro_Produccion_E>();
                using (SqlConnection cn = new SqlConnection(cadenaCnx))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_REGISTRO_PRODUCCION", cn))
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
                                Registro_Produccion_E Entidad = new Registro_Produccion_E();
                                Entidad.id_Ingreso = Convert.ToInt32(row["id_Ingreso"]);
                                Entidad.id_Local = Convert.ToInt32(row["id_Local"]);
                                Entidad.id_OTContable = Convert.ToInt32(row["id_OTContable"]);
                                Entidad.descripcion_OTContable = row["descripcion_OTContable"].ToString();

                                Entidad.id_Personal_Responsable = Convert.ToInt32(row["id_Personal_Responsable"]);
                                Entidad.id_personal = Convert.ToInt32(row["id_personal"]);
                                Entidad.nroDoc_personal = row["nroDoc_personal"].ToString();

                                Entidad.personal = row["personal"].ToString();
                                Entidad.cargo = row["cargo"].ToString();
                                Entidad.ImporteProduccion_Ingreso = Convert.ToDecimal(row["ImporteProduccion_Ingreso"].ToString());
                                Entidad.obsProduccion_Ingreso = row["obsProduccion_Ingreso"].ToString();

                                Entidad.ImporteMovilidad_Ingreso =Convert.ToDecimal(row["ImporteMovilidad_Ingreso"].ToString());
                                Entidad.obsMovilidad_Ingreso = row["obsMovilidad_Ingreso"].ToString();
                                Entidad.fecha_ingreso = row["fecha_ingreso"].ToString();
                                Entidad.estado = Convert.ToInt32(row["estado"]);
                                Entidad.usuario_creacion = Convert.ToInt32(row["usuario_creacion"]);
                                Entidad.fecha_creacion = row["fecha_creacion"].ToString();
                                Entidad.usuario_edicion = Convert.ToInt32(row["usuario_edicion"]);
                                Entidad.fecha_edicion = row["fecha_edicion"].ToString();
                                                              
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
