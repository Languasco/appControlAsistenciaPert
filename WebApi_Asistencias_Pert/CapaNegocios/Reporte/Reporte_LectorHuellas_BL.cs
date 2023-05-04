using CapaDatos.Proceso;
using CapaDatos.Reporte;
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

namespace CapaNegocios.Proceso
{
    public class Reporte_LectorHuellas_BL
    {
        string cadenaCnx = System.Configuration.ConfigurationManager.ConnectionStrings["conexionDsige"].ConnectionString;

        public List<Reporte_LectorHuellas_E> Listar_ReporteDetalle(int id_local, int id_personal, int id_turno, string fecha_ini, string fecha_fin, int opcion_reporte)
        {
            try
            {
                List<Reporte_LectorHuellas_E> obj_list = new List<Reporte_LectorHuellas_E>();
                using (SqlConnection cn = new SqlConnection(cadenaCnx))
                {
                    cn.Open();

                    if (opcion_reporte ==1)   // Reporte inicio-fin en pantalla
                    {
                        using (SqlCommand cmd = new SqlCommand("SP_S_REP_LECTOR_HUELLAS_REPORTE_1", cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@id_local", SqlDbType.Int).Value = id_local;
                            cmd.Parameters.Add("@id_personal", SqlDbType.Int).Value = id_personal;
                            cmd.Parameters.Add("@id_turno", SqlDbType.Int).Value = id_turno;
                            cmd.Parameters.Add("@fecha_ini", SqlDbType.VarChar).Value = fecha_ini;
                            cmd.Parameters.Add("@fecha_fin", SqlDbType.VarChar).Value = fecha_fin;


                            DataTable dt_detalle = new DataTable();
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dt_detalle);
                                foreach (DataRow row in dt_detalle.Rows)
                                {
                                    Reporte_LectorHuellas_E Entidad = new Reporte_LectorHuellas_E();

                                    Entidad.contrato = row["contrato"].ToString();
                                    Entidad.fecha = row["fecha"].ToString();
                                    Entidad.DNI = row["DNI"].ToString();

                                    Entidad.nombre = row["nombre"].ToString();
                                    Entidad.CECO = row["CECO"].ToString();
                                    Entidad.jefeCuadrilla = row["jefeCuadrilla"].ToString();
                                    Entidad.Hora_Inicio = row["Hora_Inicio"].ToString();
                                    Entidad.Posicion_Inicio = row["Posicion_Inicio"].ToString();

                                    Entidad.Hora_Salida = row["Hora_Salida"].ToString();
                                    Entidad.Posicion_Fin = row["Posicion_Fin"].ToString();
                                    Entidad.Horas = row["Horas"].ToString();
                                    Entidad.ID = row["ID"].ToString();
                                    Entidad.CECO_Tareo = row["CECO_Tareo"].ToString();

                                    obj_list.Add(Entidad);
                                }
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

        public string ExportarExcel_Reporte(int id_local, int id_personal, int id_turno, string fecha_ini, string fecha_fin, int opcion_reporte)
        {
            string Res = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cadenaCnx))
                {
                    cn.Open();
                    if (opcion_reporte == 1 ) // exel inicio fin 
                    {
                        using (SqlCommand cmd = new SqlCommand("SP_S_REP_LECTOR_HUELLAS_REPORTE_1_EXCEL", cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@id_local", SqlDbType.Int).Value = id_local;
                            cmd.Parameters.Add("@id_personal", SqlDbType.Int).Value = id_personal;
                            cmd.Parameters.Add("@id_turno", SqlDbType.Int).Value = id_turno;
                            cmd.Parameters.Add("@fecha_ini", SqlDbType.VarChar).Value = fecha_ini;
                            cmd.Parameters.Add("@fecha_fin", SqlDbType.VarChar).Value = fecha_fin;

                            DataTable dt_detalle = new DataTable();
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dt_detalle);
                                if (dt_detalle.Rows.Count <= 0)
                                {
                                    Res = "0|No hay informacion disponible";
                                }
                                else
                                {
                                    Res = Generar_Excel_Reporte_uno(dt_detalle);
                                }
                            }
                        }
                    }
                    else if (opcion_reporte == 2) // reporte tareo inicio fin
                    {
                        using (SqlCommand cmd = new SqlCommand("SP_S_REP_LECTOR_HUELLAS_REPORTE_2_PERSONAL", cn))
                        {
                            cmd.CommandTimeout = 0;
                            cmd.CommandType = CommandType.StoredProcedure;
                            cmd.Parameters.Add("@id_local", SqlDbType.Int).Value = id_local;
                            cmd.Parameters.Add("@id_personal", SqlDbType.Int).Value = id_personal;
                            cmd.Parameters.Add("@id_turno", SqlDbType.Int).Value = id_turno;
                            cmd.Parameters.Add("@fecha_ini", SqlDbType.VarChar).Value = fecha_ini;
                            cmd.Parameters.Add("@fecha_fin", SqlDbType.VarChar).Value = fecha_fin;

                            DataTable dt_detalle_Personal = new DataTable();
                            using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                            {
                                da.Fill(dt_detalle_Personal);
                                if (dt_detalle_Personal.Rows.Count <= 0)
                                {
                                    Res = "0|No hay informacion disponible";
                                }
                                else
                                {
                                    Res = Generar_Excel_Reporte_dos(dt_detalle_Personal, id_local,  id_personal,  id_turno,  fecha_ini,  fecha_fin);
                                }
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

        public string Generar_Excel_Reporte_uno(DataTable DT_detalles)
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
                FileRuta = System.Web.Hosting.HostingEnvironment.MapPath("~/Temp/Descargas/Huellero_Reporte_inicio_fin_" + _servidor);
                string rutaServer = ConfigurationManager.AppSettings["ArchivoTexto_IngresoTrabajador"];

                FileExcel = rutaServer + "Huellero_Reporte_inicio_fin_" + _servidor;
                FileInfo _file = new FileInfo(FileRuta);
                if (_file.Exists)
                {
                    _file.Delete();
                    _file = new FileInfo(FileRuta);
                }
                using (Excel.ExcelPackage oEx = new Excel.ExcelPackage(_file))
                {
                    Excel.ExcelWorksheet oWs = oEx.Workbook.Worksheets.Add("Reporte_Inicio_Fin");
                    oWs.Cells.Style.Font.SetFromFont(new Font("Tahoma", 9));  
                    
                    for (int i = 1; i <= 13; i++)
                    {
                        oWs.Cells[_filaInicio, i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        oWs.Cells[_filaInicio, i].Style.Font.Size = 9; //letra tamaño   
                        oWs.Cells[_filaInicio, i].Style.Font.Bold = true; //Letra negrita

                        oWs.Cells[_filaInicio, i].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#000000")); // color de la letra
                        oWs.Cells[_filaInicio, i].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;   // fondo de celda
                        oWs.Cells[_filaInicio, i].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightYellow); // fondo de celda


                    }
                    
                    oWs.Cells[_filaInicio, 1].Value = "Contrato";
                    oWs.Cells[_filaInicio, 2].Value = "Fecha";
                    oWs.Cells[_filaInicio, 3].Value = "DNI";
                    oWs.Cells[_filaInicio, 4].Value = "Nombre";
                    oWs.Cells[_filaInicio, 5].Value = "CECO";
                    oWs.Cells[_filaInicio, 6].Value = "Jefe Cuadrilla";

                    oWs.Cells[_filaInicio, 7].Value = "Hora de Inicio";
                    oWs.Cells[_filaInicio, 8].Value = "Posición Inicio";

                    oWs.Cells[_filaInicio, 9].Value = "Hora de Salida";
                    oWs.Cells[_filaInicio, 10].Value = "Posición Fin";
                    oWs.Cells[_filaInicio, 11].Value = "Horas";
                    oWs.Cells[_filaInicio, 12].Value = "ID";
                    oWs.Cells[_filaInicio, 13].Value = "CECO (Tareo)";       

                    foreach (DataRow oBj in DT_detalles.Rows)
                    {
                        for (int i = 1; i <= 13; i++)
                        {
                            oWs.Cells[_fila, i].Style.Font.Size = 8; //letra tamaño 
                            oWs.Cells[_fila, i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }

                        oWs.Cells[_fila, 1].Value = oBj["contrato"].ToString();
                        oWs.Cells[_fila, 2].Value = oBj["fecha"].ToString();
                        oWs.Cells[_fila, 3].Value = oBj["DNI"].ToString();
                        oWs.Cells[_fila, 4].Value = oBj["nombre"].ToString();

                        oWs.Cells[_fila, 5].Value = oBj["CECO"].ToString();
                        oWs.Cells[_fila, 6].Value = oBj["jefeCuadrilla"].ToString();
                        oWs.Cells[_fila, 7].Value = oBj["Hora_Inicio"].ToString();
                        oWs.Cells[_fila, 8].Value = oBj["Posicion_Inicio"].ToString();

                        oWs.Cells[_fila, 9].Value = oBj["Hora_Salida"].ToString();
                        oWs.Cells[_fila, 10].Value = oBj["Posicion_Fin"].ToString();
                        oWs.Cells[_fila, 11].Value = oBj["Horas"].ToString();
                        oWs.Cells[_fila, 12].Value = oBj["ID"].ToString();
                        oWs.Cells[_fila, 13].Value = oBj["CECO_Tareo"].ToString();
 
                        _fila++;
                    }

                    for (int i = 1; i <= 12; i++)
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

        public DataTable Get_Tareo_Detallado(int id_local, int id_personal, int id_turno, string fecha_ini, string fecha_fin)
        {
            DataTable resultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(cadenaCnx))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_S_REP_LECTOR_HUELLAS_REPORTE_2_DETALLADO", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id_local", SqlDbType.Int).Value = id_local;
                        cmd.Parameters.Add("@id_personal", SqlDbType.Int).Value = id_personal;
                        cmd.Parameters.Add("@id_turno", SqlDbType.Int).Value = id_turno;
                        cmd.Parameters.Add("@fecha_ini", SqlDbType.VarChar).Value = fecha_ini;
                        cmd.Parameters.Add("@fecha_fin", SqlDbType.VarChar).Value = fecha_fin;

                        DataTable dt_detalle = new DataTable();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt_detalle);
                            resultado = dt_detalle;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return resultado;
        }

        public DataTable Get_Tareo_FechasAgrupado(int id_local, int id_personal, int id_turno, string fecha_ini, string fecha_fin)
        {
            DataTable resultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(cadenaCnx))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_S_REP_LECTOR_HUELLAS_REPORTE_2_FECHAS", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id_local", SqlDbType.Int).Value = id_local;
                        cmd.Parameters.Add("@id_personal", SqlDbType.Int).Value = id_personal;
                        cmd.Parameters.Add("@id_turno", SqlDbType.Int).Value = id_turno;
                        cmd.Parameters.Add("@fecha_ini", SqlDbType.VarChar).Value = fecha_ini;
                        cmd.Parameters.Add("@fecha_fin", SqlDbType.VarChar).Value = fecha_fin;

                        DataTable dt_detalle = new DataTable();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt_detalle);
                            resultado = dt_detalle;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return resultado;
        }


        public string Get_VerificarTareo(DataTable Lista_Data, int id_personal, string fecha)
        {
            string res = "";
            string valor = "-";

            for (int i = 0; i < Lista_Data.Rows.Count; i++)
            {
                if (id_personal == Convert.ToInt32(Lista_Data.Rows[i]["id_personal"].ToString()) && fecha == Lista_Data.Rows[i]["fecha_Asis"].ToString())
                {
                    valor = Lista_Data.Rows[i]["codigo_OT"].ToString();
                    break;
                }
            }
            res = Convert.ToString(valor);
            return res;
        }
        

        public string Generar_Excel_Reporte_dos(DataTable DT_personal, int id_local, int id_personal, int id_turno, string fecha_ini, string fecha_fin)
        {
            int _filaInicio = 1;
            int ultimaColum = 0;
            string Res = "";
            string _servidor;
            string _ruta;
            string resultado = "";
            int _fila = 2;
            string FileRuta = "";
            string FileExcel = "";

            try
            {
               /// relacion de tareos 
                DataTable dt_detalles_Tareos = new DataTable();
                dt_detalles_Tareos = Get_Tareo_Detallado(id_local, id_personal, id_turno, fecha_ini, fecha_fin);

                if (dt_detalles_Tareos.Rows.Count <= 0)
                {
                    Res = "0|No hay informacion disponible";
                    return Res;
                }

                DataTable ListaFechas_Agrupada = new DataTable();
                ListaFechas_Agrupada = Get_Tareo_FechasAgrupado(id_local,id_personal,id_turno,fecha_ini, fecha_fin);


                _servidor = String.Format("{0:ddMMyyyy_hhmmss}.xlsx", DateTime.Now);
                FileRuta = System.Web.Hosting.HostingEnvironment.MapPath("~/Temp/Descargas/Huellero_Tareo_inicio_fin_" + _servidor);
                string rutaServer = ConfigurationManager.AppSettings["ArchivoTexto_IngresoTrabajador"];
                FileExcel = rutaServer + "Huellero_Tareo_inicio_fin_" + _servidor;


                FileInfo _file = new FileInfo(FileRuta);
                if (_file.Exists)
                {
                    _file.Delete();
                    _file = new FileInfo(FileRuta);
                }


                using (Excel.ExcelPackage oEx = new Excel.ExcelPackage(_file))
                {
                    Excel.ExcelWorksheet oWs = oEx.Workbook.Worksheets.Add("Tareo_inicio_fin");
                    oWs.Cells.Style.Font.SetFromFont(new Font("Tahoma", 9));  

                    /// --- cabecera
                    for (int i = 1; i <= 2; i++)
                    {
                        oWs.Cells[_filaInicio, i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        oWs.Cells[_filaInicio, i].Style.Font.Size = 9; //letra tamaño   
                        oWs.Cells[_filaInicio, i].Style.Font.Bold = true; //Letra negrita
                    }

                    oWs.Cells[_filaInicio, 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;   // fondo de celda
                    oWs.Cells[_filaInicio, 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.AntiqueWhite); // fondo de celda
                    oWs.Cells[_filaInicio, 1].Value = "DNI";

                    oWs.Cells[_filaInicio, 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;   // fondo de celda
                    oWs.Cells[_filaInicio, 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.AntiqueWhite); // fondo de celda
                    oWs.Cells[_filaInicio, 2].Value = "NOMBRE";


                        ultimaColum = 3;
                        ////AGREGANDO DE FECHAS
                        foreach (DataRow item in ListaFechas_Agrupada.Rows)
                        {
                            //oWs.Cells[_filaInicio, ultimaColum].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#000080")); // color de la letra
                            oWs.Cells[_filaInicio, ultimaColum].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;   // fondo de celda
                            oWs.Cells[_filaInicio, ultimaColum].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.AntiqueWhite); // fondo de celda

                            oWs.Cells[_filaInicio, ultimaColum].Style.Font.Size = 10; //letra tamaño  
                            oWs.Cells[_filaInicio, ultimaColum].Style.Font.Bold = true; //Letra negrita
                            oWs.Cells[_filaInicio, ultimaColum].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            oWs.Cells[_filaInicio, ultimaColum].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            oWs.Cells[_filaInicio, ultimaColum].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center; // alinear texto  
                            oWs.Cells[_filaInicio, ultimaColum].Value = item["Fechas"].ToString();
                            ultimaColum = ultimaColum + 1;
                        }
                        //// FIN DE FECHAS

                    // --- fin de cabeceras

                   foreach (DataRow oBj in DT_personal.Rows)
                    {
                        for (int i = 1; i <= 2; i++)
                        {
                            oWs.Cells[_fila, i].Style.Font.Size = 8; //letra tamaño  
                            oWs.Cells[_fila, i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }
                        oWs.Cells[_fila, 1].Value = oBj["dni"].ToString();
                        oWs.Cells[_fila, 2].Value = oBj["personal"].ToString();


                        ultimaColum = 3;
                        ////AGREGANDO DE FECHAS
                        foreach (DataRow item in ListaFechas_Agrupada.Rows)
                        {
                            oWs.Cells[_fila, ultimaColum].Style.Font.Size = 8; //letra tamaño  
                            oWs.Cells[_fila, ultimaColum].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                            oWs.Cells[_fila, ultimaColum].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            oWs.Cells[_fila, ultimaColum].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center; // alinear texto  
                            oWs.Cells[_fila, ultimaColum].Value = Get_VerificarTareo(dt_detalles_Tareos, Convert.ToInt32(oBj["id_personal"].ToString()), item["Fechas"].ToString());
                            ultimaColum = ultimaColum + 1;
                        }
                        //// FIN DE FECHAS

                        _fila++;
                    }

                  int cant_col = 0;
                  cant_col = (2 + (ListaFechas_Agrupada.Rows.Count));

                   for (int i = 1; i <= cant_col; i++)
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
