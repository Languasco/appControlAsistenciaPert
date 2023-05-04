using CapaDatos.Proceso;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = OfficeOpenXml;
using Style = OfficeOpenXml.Style;

namespace CapaNegocios.Reporte
{
    public class Reporte_Tareo_Diario_BL
    {
        string cadenaCnx = System.Configuration.ConfigurationManager.ConnectionStrings["conexionDsige"].ConnectionString;

        public List<Reporte_Tareo_Diariol_E> Listar_ReporteTareoDiario(int local, int ot, string fecha)
        {
            try
            {
                List<Reporte_Tareo_Diariol_E> obj_list = new List<Reporte_Tareo_Diariol_E>();
                using (SqlConnection cn = new SqlConnection(cadenaCnx))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_S_TAREO_DIARIO", cn))
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
                                Reporte_Tareo_Diariol_E Entidad = new Reporte_Tareo_Diariol_E();
 
                                Entidad.Fecha = row["Fecha"].ToString();
                                Entidad.dni = row["dni"].ToString();
                                Entidad.personal = row["personal"].ToString();
                                Entidad.id_tasi_codigo = row["id_tasi_codigo"].ToString();
                                Entidad.horaIngreso_Asistencia = row["horaIngreso_Asistencia"].ToString();
                                Entidad.horaSalida_Asistencia = row["horaSalida_Asistencia"].ToString();
                                Entidad.total_Asistencia = row["total_Asistencia"].ToString();
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

        public string ExportarExcel_ReporteTareo_Diario(int local, int ot, string fecha)
        {
            string Res = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cadenaCnx))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_S_TAREO_DIARIO", cn))
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
                            if (dt_detalle.Rows.Count <= 0)
                            {
                                Res = "0|No hay informacion disponible";
                            }
                            else {
                               Res= Generar_Excel(dt_detalle);     
                            }                           
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Res = "-1|" + ex.Message;
            }
            return Res;
        }

        public string Generar_Excel(DataTable DT_detalles)
        {
            int _filaInicio = 1;
            string Res = "";
            string _servidor;
            string _ruta;
            string resultado = "";
            int _fila = 2;
            string FileRuta = "";
            string FileExcel = "";
 
            try
            {
                _servidor = String.Format("{0:ddMMyyyy_hhmmss}.xlsx", DateTime.Now);
                FileRuta = System.Web.Hosting.HostingEnvironment.MapPath("~/Temp/Descargas/TareoDiario_" + _servidor);
                string rutaServer = ConfigurationManager.AppSettings["ArchivoTexto_IngresoTrabajador"];

                FileExcel = rutaServer + "TareoDiario_" + _servidor;
                FileInfo _file = new FileInfo(FileRuta);
                if (_file.Exists)
                {
                    _file.Delete();
                    _file = new FileInfo(FileRuta);
                }
                using (Excel.ExcelPackage oEx = new Excel.ExcelPackage(_file))
                {
                    Excel.ExcelWorksheet oWs = oEx.Workbook.Worksheets.Add("Tareo_Diario");
                    oWs.Cells.Style.Font.SetFromFont(new Font("Tahoma", 9));   
                    
                    for (int i = 1; i <= 7; i++)
                    {
                        oWs.Cells[_filaInicio, i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        oWs.Cells[_filaInicio, i].Style.Font.Size = 9; //letra tamaño   
                        oWs.Cells[_filaInicio, i].Style.Font.Bold = true; //Letra negrita
                    }

                    oWs.Cells[_filaInicio, 1].Value = "FECHA";
                    oWs.Cells[_filaInicio, 2].Value = "DNI";
                    oWs.Cells[_filaInicio, 3].Value = "PERSONAL";
                    oWs.Cells[_filaInicio, 4].Value = "TIPO ASISTENCIA";
                    oWs.Cells[_filaInicio, 5].Value = "HORA ENTRADA";
                    oWs.Cells[_filaInicio, 6].Value = "HORA SALIDA";
                    oWs.Cells[_filaInicio, 7].Value = "TOTAL HORAS";
       

                    foreach (DataRow oBj in DT_detalles.Rows)
                    {
                        for (int i = 1; i <= 7; i++)
                        {
                            oWs.Cells[_fila, i].Style.Font.Size = 8; //letra tamaño   
                        }

                        oWs.Cells[_fila, 1].Value = oBj["Fecha"].ToString();
                        oWs.Cells[_fila, 2].Value = oBj["dni"].ToString();
                        oWs.Cells[_fila, 3].Value = oBj["personal"].ToString();
                        oWs.Cells[_fila, 4].Value = oBj["id_tasi_codigo"].ToString();
                        oWs.Cells[_fila, 5].Value = oBj["horaIngreso_Asistencia"].ToString();
                        oWs.Cells[_fila, 6].Value = oBj["horaSalida_Asistencia"].ToString();
                        oWs.Cells[_fila, 7].Value = oBj["total_Asistencia"].ToString();
 
                        _fila++;
                    }

                    for (int i = 1; i <= 7; i++)
                    {
                        oWs.Column(i).AutoFit();
                    }

                    oEx.Save();
                }

                Res = "1|" + FileExcel;
            }
            catch (Exception ex)
            {
                Res = "-1|" + ex.Message;
            }
            return Res;
        }

    }
}
