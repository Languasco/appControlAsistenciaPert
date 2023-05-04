using CapaDatos.Proceso;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocios.Proceso
{
    public class GenerarTxT_IngresoTrabajador_BL
    {
        string cadenaCnx = System.Configuration.ConfigurationManager.ConnectionStrings["conexionDsige"].ConnectionString;

        public string Listar_GenerarTxT_IngresosTrabajador(int local, int OT_contable, int id_personalResponsable, string fecha_ini, string fecha_fin, string  tipoReporte)
        {
            string _correlativo = "";
            string _correlativo2 = "";
            string _rutafile = "";
            string _nombreReporte = "";
            string _rutaServer = "";
            string Resultado = "";
            
            try
            {


                using (SqlConnection cn = new SqlConnection(cadenaCnx))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_S_GENERAR_TXT_INGRESOS_TRABAJADOR", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id_local", SqlDbType.Int).Value = local;
                        cmd.Parameters.Add("@ot_contable", SqlDbType.Int).Value = OT_contable;
                        cmd.Parameters.Add("@id_respondable", SqlDbType.Int).Value = id_personalResponsable;
                        cmd.Parameters.Add("@fecha_ini", SqlDbType.VarChar).Value = fecha_ini;
                        cmd.Parameters.Add("@fecha_fin", SqlDbType.VarChar).Value = fecha_fin;
                        cmd.Parameters.Add("@tipoReporte", SqlDbType.Int).Value = tipoReporte; 
                        
                        DataTable dt_detalle = new DataTable();

                        _nombreReporte="MOVILIDAD_";
                        if (Convert.ToInt32(tipoReporte) == 2)
                        {
                            _nombreReporte = "PRODUCCION_";
                        }          

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt_detalle);

                            string[] linesArchivo = new string[dt_detalle.Rows.Count];
                            int i = 0;

                            foreach (DataRow Fila in dt_detalle.Rows)
                            {
                                linesArchivo[i] = Fila["descripcionArchivo"].ToString();
                                i = i + 1;
                            }


                            if (dt_detalle.Rows.Count <= 0)
                            {
                                Resultado = "0|No hay informacion disponible";
                            }
                            else
                            {
                                _correlativo = String.Format("{0:ddMMyyyy_hhmmss}.txt", DateTime.Now);
                                _rutafile = System.Web.Hosting.HostingEnvironment.MapPath("~/Temp/Descargas/" + _nombreReporte + _correlativo);
                                _rutaServer = ConfigurationManager.AppSettings["ArchivoTexto_IngresoTrabajador"];

                                System.IO.File.WriteAllLines(_rutafile, linesArchivo);
                                Resultado = "1|" + _rutaServer + _nombreReporte + _correlativo;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Resultado = "-1|" + ex.Message;
            }

            return Resultado;
        }

    }
}
