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

namespace CapaNegocios.Proceso
{
    public class Reporte_TareoPersonal_BL
    {
        string cadenaCnx = System.Configuration.ConfigurationManager.ConnectionStrings["conexionDsige"].ConnectionString;

        public List<Reporte_TareoPersonal_E> Listar_ReporteTareoPersonal(int local, string fecha_ini, string fecha_fin, int  id_personal)
        {
            try
            {
                List<Reporte_TareoPersonal_E> obj_list = new List<Reporte_TareoPersonal_E>();
                using (SqlConnection cn = new SqlConnection(cadenaCnx))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_S_REGISTRO_TAREO_PERSONAL", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id_local", SqlDbType.Int).Value = local;
                        cmd.Parameters.Add("@fecha_ini", SqlDbType.VarChar).Value = fecha_ini;
                        cmd.Parameters.Add("@fecha_fin", SqlDbType.VarChar).Value = fecha_fin;
                        cmd.Parameters.Add("@id_personal", SqlDbType.Int).Value = id_personal; 


                        DataTable dt_detalle = new DataTable();
                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(dt_detalle);
                            foreach (DataRow row in dt_detalle.Rows)
                            {
                                Reporte_TareoPersonal_E Entidad = new Reporte_TareoPersonal_E();

                                Entidad.Fecha = row["Fecha"].ToString();
                                Entidad.Dia = row["Dia"].ToString();
                                Entidad.Turno = row["Turno"].ToString();
                                Entidad.HR_ingreso = row["HR_ingreso"].ToString();
                                Entidad.HR_salida = row["HR_salida"].ToString();
                                Entidad.Entrada_real = row["Entrada_real"].ToString();
                                Entidad.Salida_real = row["Salida_real"].ToString();
                                Entidad.HR_Trabajadas = row["HR_Trabajadas"].ToString();
                                Entidad.Horas_fijas = row["Horas_fijas"].ToString();
                                Entidad.Horas_extras = row["Horas_extras"].ToString();
                                Entidad.Ot_contable = row["Ot_contable"].ToString();
                                
                                                              
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
        
        public string ExportarExcel_ReporteTareoPersonal(int local, int  id_ot, string fecha_ini, string fecha_fin)
        {
            string Res = "";
            try
            {
                using (SqlConnection cn = new SqlConnection(cadenaCnx))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_S_TAREO_PERSONAL_PERSONAL", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id_local", SqlDbType.Int).Value = local;
                        cmd.Parameters.Add("@id_OT", SqlDbType.Int).Value = id_ot;
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
                            else {
                                Res = Generar_Excel_TareoPersonal(dt_detalle, local, id_ot, fecha_ini, fecha_fin);     
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
                
        public DataTable Get_RelacionTareos(int local, int id_ot, string fecha_ini, string fecha_fin)
        {
            DataTable resultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(cadenaCnx))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_S_TAREO_PERSONAL_FECHAS_DETALLE", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id_local", SqlDbType.Int).Value = local;
                        cmd.Parameters.Add("@id_OT", SqlDbType.Int).Value = id_ot;
                        cmd.Parameters.Add("@fecha_ini", SqlDbType.VarChar ).Value = fecha_ini;
                        cmd.Parameters.Add("@fecha_fin", SqlDbType.VarChar ).Value = fecha_fin;

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
        
        public DataTable Get_Tareo_FechasAgrupado(int local, int id_ot, string fecha_ini, string fecha_fin)
        {
            DataTable resultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(cadenaCnx))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_S_TAREO_PERSONAL_FECHAS_AGRUPADAS", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id_local", SqlDbType.Int).Value = local;
                        cmd.Parameters.Add("@id_OT", SqlDbType.Int).Value = id_ot;
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
        
        public int  Get_TotalesTipoAsistencia(DataTable Lista_Data, int id_personal, string tipoAsistencia)
        {
            int res = 0;
            int valor =0 ;

            for (int i = 0; i < Lista_Data.Rows.Count; i++)
            {
                if (id_personal == Convert.ToInt32(Lista_Data.Rows[i]["id_personal"].ToString()) && Lista_Data.Rows[i]["id_tipoAsistencia"].ToString() == tipoAsistencia)
                {
                    valor = valor + 1; 
                }
            }
            res = Convert.ToInt32(valor);
            return res;
        }
                        
        public string Get_VerificarAsistencia(DataTable Lista_Data, int id_personal, string fecha)
        {
            string res = "";
            string valor = "-";

            for (int i = 0; i < Lista_Data.Rows.Count; i++)
            {
                if (id_personal == Convert.ToInt32(Lista_Data.Rows[i]["id_personal"].ToString()) && fecha == Lista_Data.Rows[i]["fecha_asistencia"].ToString() && Lista_Data.Rows[i]["id_tipoAsistencia"].ToString() == "A")
                {
                    valor = Lista_Data.Rows[i]["codigo_OT"].ToString();
                    break;
                }
                else if (id_personal == Convert.ToInt32(Lista_Data.Rows[i]["id_personal"].ToString()) && fecha == Lista_Data.Rows[i]["fecha_asistencia"].ToString() && Lista_Data.Rows[i]["id_tipoAsistencia"].ToString() == "F")
                {
                    valor = "F";
                    break;
                }
                else if (id_personal == Convert.ToInt32(Lista_Data.Rows[i]["id_personal"].ToString()) && fecha == Lista_Data.Rows[i]["fecha_asistencia"].ToString() && Lista_Data.Rows[i]["id_tipoAsistencia"].ToString() == "V")
                {
                    valor = "V";
                    break;
                }
                else if (id_personal == Convert.ToInt32(Lista_Data.Rows[i]["id_personal"].ToString()) && fecha == Lista_Data.Rows[i]["fecha_asistencia"].ToString() && Lista_Data.Rows[i]["id_tipoAsistencia"].ToString() == "S")
                {
                    valor = "S";
                    break;
                }
                else if (id_personal == Convert.ToInt32(Lista_Data.Rows[i]["id_personal"].ToString()) && fecha == Lista_Data.Rows[i]["fecha_asistencia"].ToString() && Lista_Data.Rows[i]["id_tipoAsistencia"].ToString() == "B")
                {
                    valor = "R";
                    break;
                }
                else if (id_personal == Convert.ToInt32(Lista_Data.Rows[i]["id_personal"].ToString()) && fecha == Lista_Data.Rows[i]["fecha_asistencia"].ToString() && Lista_Data.Rows[i]["id_tipoAsistencia"].ToString() == "DM")
                {
                    valor = "D";
                    break;
                }
            }
            res = Convert.ToString(valor);
            return res;
        }

        public string Get_VerificarHorasExtras(DataTable Lista_Data, int id_personal, string fecha)
        {
            string res = "";
            string valor = "";

            for (int i = 0; i < Lista_Data.Rows.Count; i++)
            {
                if (id_personal == Convert.ToInt32(Lista_Data.Rows[i]["id_personal"].ToString()) && fecha == Lista_Data.Rows[i]["fecha_asistencia"].ToString())
                {
                    if (Lista_Data.Rows[i]["horaExtra"].ToString() == "00:00")
                    {
                        valor = "";
                    }
                    else {
                        valor = Lista_Data.Rows[i]["horaExtra"].ToString();
                    }
                    break; 
                }
            }
            res = Convert.ToString(valor);
            return res;
        }
                        
        public string Generar_Excel_TareoPersonal(DataTable ListaAgrupada_Personal, int local, int id_ot, string fecha_ini, string fecha_fin)
        {
            int _filaInicio = 6;

            string Res = "";
            string _servidor;
            string _ruta;
            string resultado = "";
            int _fila = 7;
            string FileRuta = "";
            string FileExcel = "";

            int ultimaColum = 0;
            try
            {
                _servidor = String.Format("{0:ddMMyyyy_hhmmss}.xlsx", DateTime.Now);
                FileRuta = System.Web.Hosting.HostingEnvironment.MapPath("~/Temp/Descargas/TareoPersonal_" + _servidor);
                string rutaServer = ConfigurationManager.AppSettings["ArchivoTexto_IngresoTrabajador"];

                FileExcel = rutaServer + "TareoPersonal_" + _servidor;
                FileInfo _file = new FileInfo(FileRuta);
                if (_file.Exists)
                {
                    _file.Delete();
                    _file = new FileInfo(FileRuta);
                }

                //relacion de tareos 
                DataTable dt_detalles_Tareos = new DataTable();
                dt_detalles_Tareos = Get_RelacionTareos(local,id_ot,fecha_ini,fecha_fin);

                if (dt_detalles_Tareos.Rows.Count <= 0)
                {
                    Res = "0|No hay informacion disponible";
                    return Res;
                }

                DataTable ListaFechas_Agrupada = new DataTable();
                ListaFechas_Agrupada = Get_Tareo_FechasAgrupado(local, id_ot, fecha_ini, fecha_fin);
                
                List<TareoMasivo_E> listaNombreMeses = new List<TareoMasivo_E>();
                string nombreMes = "";
                int indice = 1;

                //  --  nombre de los meses
                for (int i = 0; i < ListaFechas_Agrupada.Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        nombreMes = "";
                        nombreMes = ListaFechas_Agrupada.Rows[i]["nombreMes"].ToString();
                        TareoMasivo_E obj_ent = new TareoMasivo_E(); ;
                        obj_ent.nombreMes = nombreMes;
                        obj_ent.numero_dia = indice;
                        listaNombreMeses.Add(obj_ent);
                    }
                    else {
                        if (nombreMes != ListaFechas_Agrupada.Rows[i]["nombreMes"].ToString())
                        {
                            indice = indice + 1;
                            nombreMes = "";
                            nombreMes = ListaFechas_Agrupada.Rows[i]["nombreMes"].ToString();
                            TareoMasivo_E obj_ent = new TareoMasivo_E(); ;
                            obj_ent.nombreMes = nombreMes;
                            obj_ent.numero_dia = indice;
                            listaNombreMeses.Add(obj_ent);
                        }               
                    }            
                }

                var cant = 0;
                foreach (TareoMasivo_E item in listaNombreMeses)
                {
                    cant = 0;
                    for (int i = 0; i < ListaFechas_Agrupada.Rows.Count; i++)
                    {
                        if ( item.nombreMes  == ListaFechas_Agrupada.Rows[i]["nombreMes"].ToString())
                        {
                            cant = cant + 1;
                        }
                    }
                    item.cantidad = cant;
                }
                //  --  nombre de los meses
                
                using (Excel.ExcelPackage oEx = new Excel.ExcelPackage(_file))
                {
                    Excel.ExcelWorksheet oWs = oEx.Workbook.Worksheets.Add("Tareo_Personal");
                    oWs.Cells.Style.Font.SetFromFont(new Font("Tahoma", 9));  

                    ultimaColum = 13;
                    ////AGREGANDO CABECERA DIAS-MES
                    foreach (DataRow item in ListaFechas_Agrupada.Rows)
                    {
                        oWs.Cells[1, ultimaColum].Style.Font.Size = 11; //letra tamaño  
                        oWs.Cells[1, ultimaColum].Style.Font.Bold = true; //Letra negrita
                        oWs.Cells[1, ultimaColum].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        oWs.Cells[1, ultimaColum].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        oWs.Cells[1, ultimaColum].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center; // alinear texto  
                        oWs.Cells[1, ultimaColum].Value = item["diaMes"].ToString();
                        ultimaColum = ultimaColum + 1;
                        oWs.Cells[1, ultimaColum].Style.Font.Size = 11; //letra tamaño  
                        oWs.Cells[1, ultimaColum].Style.Font.Bold = true; //Letra negrita
                        oWs.Cells[1, ultimaColum].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        oWs.Cells[1, ultimaColum].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        oWs.Cells[1, ultimaColum].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center; // alinear texto  
                        oWs.Cells[1, ultimaColum].Value = item["diaMes"].ToString();
                        
                        oWs.Cells[1, ultimaColum - 1, 1, ultimaColum].Merge = true;  // combinar celdaS 
                        ultimaColum = ultimaColum + 1;
                    }
                    //FIM DE CABECERA  DIAS DIAS-MES

                    ultimaColum = 13;
                    var nroColumna = 0;
                    foreach (TareoMasivo_E item in listaNombreMeses)
                    {
                        oWs.Cells[3, ultimaColum].Style.Font.Size = 16; //letra tamaño  
                        oWs.Cells[3, ultimaColum].Style.Font.Bold = true; //Letra negrita
                        oWs.Cells[3, ultimaColum].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        oWs.Cells[3, ultimaColum].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center; // alinear texto  
                        oWs.Cells[3, ultimaColum].Value = item.nombreMes;

                        oWs.Cells[3, ultimaColum].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#000080")); // color de la letra
                        nroColumna = (ultimaColum + (item.cantidad * 2));

                        oWs.Cells[3, ultimaColum, 3, (nroColumna - 1)].Merge = true;  // combinar celdaS    
                        oWs.Cells[3, ultimaColum, 3, (nroColumna - 1)].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        ultimaColum = nroColumna;
                    }

                    
                    ultimaColum = 13;
                    List<int> ListaFinSemana = new List<int>();
                    ////AGREGANDO CABECERA NOMBRE DE DIA
                    foreach (DataRow item in ListaFechas_Agrupada.Rows)
                    {
                        ///color de Fondo
                        if (item["nombreDia"].ToString() == "SÁBADO" || item["nombreDia"].ToString() == "Sabado" || item["nombreDia"].ToString() == "DOMINGO" || item["nombreDia"].ToString() == "Domingo")
                        {
                            oWs.Cells[4, ultimaColum].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#FF0000")); // color de la letra
                            oWs.Cells[4, ultimaColum].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;   // fondo de celda
                            oWs.Cells[4, ultimaColum].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.PowderBlue); // fondo de celda
                            //Agregando la columna Fin de Semana
                            ListaFinSemana.Add(ultimaColum);
                            ListaFinSemana.Add((ultimaColum + 1 ));
                        }
                        else
                        {
                            oWs.Cells[4, ultimaColum].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#000080")); // color de la letra
                            oWs.Cells[4, ultimaColum].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;   // fondo de celda
                            oWs.Cells[4, ultimaColum].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.PowderBlue); // fondo de celda
                        }
                        oWs.Cells[4, ultimaColum].Style.Font.Size = 11; //letra tamaño  
                        oWs.Cells[4, ultimaColum].Style.Font.Bold = true; //Letra negrita
                        oWs.Cells[4, ultimaColum].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        oWs.Cells[4, ultimaColum].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        oWs.Cells[4, ultimaColum].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center; // alinear texto  
                        oWs.Cells[4, ultimaColum].Value = item["nombreDia"].ToString();
                        ultimaColum = ultimaColum + 1;
                        oWs.Cells[4, ultimaColum].Style.Font.Size = 11; //letra tamaño  
                        oWs.Cells[4, ultimaColum].Style.Font.Bold = true; //Letra negrita
                        oWs.Cells[4, ultimaColum].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        oWs.Cells[4, ultimaColum].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        oWs.Cells[4, ultimaColum].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center; // alinear texto  
                        oWs.Cells[4, ultimaColum].Value = item["nombreDia"].ToString();

                        oWs.Cells[4, ultimaColum - 1, 4, ultimaColum].Merge = true;  // combinar celdaS 
                        ultimaColum = ultimaColum + 1;
                    }
                    //FIM DE  CABECERA NOMBRE DE DIA

                    ultimaColum = 13;
                    ////AGREGANDO CABECERA ASIT HHEE
                    foreach (DataRow item in ListaFechas_Agrupada.Rows)
                    {
                        oWs.Cells[5, ultimaColum].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#000080")); // color de la letra
                        oWs.Cells[5, ultimaColum].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;   // fondo de celda
                        oWs.Cells[5, ultimaColum].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.PowderBlue); // fondo de celda
                        oWs.Cells[5, ultimaColum].Style.Font.Size = 11; //letra tamaño  
                        oWs.Cells[5, ultimaColum].Style.Font.Bold = true; //Letra negrita
                        oWs.Cells[5, ultimaColum].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        oWs.Cells[5, ultimaColum].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        oWs.Cells[5, ultimaColum].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center; // alinear texto  
                        oWs.Cells[5, ultimaColum].Value = "ASIT";   
                        ultimaColum = ultimaColum + 1;
                        oWs.Cells[5, ultimaColum].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#000080")); // color de la letra
                        oWs.Cells[5, ultimaColum].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;   // fondo de celda
                        oWs.Cells[5, ultimaColum].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.PowderBlue); // fondo de celda
                        oWs.Cells[5, ultimaColum].Style.Font.Size = 11; //letra tamaño  
                        oWs.Cells[5, ultimaColum].Style.Font.Bold = true; //Letra negrita
                        oWs.Cells[5, ultimaColum].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        oWs.Cells[5, ultimaColum].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        oWs.Cells[5, ultimaColum].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center; // alinear texto  
                        oWs.Cells[5, ultimaColum].Value = "HHEE";
                        oWs.Cells[5, ultimaColum - 1, 5, ultimaColum].Style.TextRotation = 90;  // rotar el Texto
                        ultimaColum = ultimaColum + 1;
                    }
                    //FIM DE  CCABECERA ASIT HHEE
                    
                    ultimaColum = 13;
                    ////AGREGANDO CABECERA NUMERO DIA
                    foreach (DataRow item in ListaFechas_Agrupada.Rows)
                    {
                        oWs.Cells[6, ultimaColum].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#000080")); // color de la letra
                        oWs.Cells[6, ultimaColum].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;   // fondo de celda
                        oWs.Cells[6, ultimaColum].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.PowderBlue); // fondo de celda
                        oWs.Cells[6, ultimaColum].Style.Font.Size = 11; //letra tamaño  
                        oWs.Cells[6, ultimaColum].Style.Font.Bold = true; //Letra negrita
                        oWs.Cells[6, ultimaColum].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        oWs.Cells[6, ultimaColum].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        oWs.Cells[6, ultimaColum].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center; // alinear texto  
                        oWs.Cells[6, ultimaColum].Value = Convert.ToInt32(item["dia"].ToString());
                        ultimaColum = ultimaColum + 1;
                        oWs.Cells[6, ultimaColum].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#000080")); // color de la letra
                        oWs.Cells[6, ultimaColum].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;   // fondo de celda
                        oWs.Cells[6, ultimaColum].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.PowderBlue); // fondo de celda
                        oWs.Cells[6, ultimaColum].Style.Font.Size = 11; //letra tamaño  
                        oWs.Cells[6, ultimaColum].Style.Font.Bold = true; //Letra negrita
                        oWs.Cells[6, ultimaColum].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        oWs.Cells[6, ultimaColum].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        oWs.Cells[6, ultimaColum].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center; // alinear texto  
                        oWs.Cells[6, ultimaColum].Value = Convert.ToInt32(item["dia"].ToString());

                        oWs.Cells[6, ultimaColum - 1, 6, ultimaColum].Merge = true;  // combinar celdaS 
                        ultimaColum = ultimaColum + 1;
                    }

                        oWs.Cells[6, ultimaColum].Style.WrapText = true;
                        oWs.Cells[6, ultimaColum].Style.Font.Size = 11; //letra tamaño  
                        oWs.Cells[6, ultimaColum].Style.Font.Bold = true; //Letra negrita
                        oWs.Cells[6, ultimaColum].Value = "F";
                        oWs.Cells[6, ultimaColum].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        oWs.Cells[6, ultimaColum].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        oWs.Cells[6, ultimaColum].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center; // alinear texto
                        oWs.Cells[6, ultimaColum].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF")); // color de la letra
                        oWs.Cells[6, ultimaColum].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;   // fondo de celda
                        oWs.Cells[6, ultimaColum].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red); // fondo de celda

                        oWs.Cells[6, ultimaColum + 1].Style.WrapText = true;
                        oWs.Cells[6, ultimaColum + 1].Style.Font.Size = 11; //letra tamaño  
                        oWs.Cells[6, ultimaColum + 1].Style.Font.Bold = true; //Letra negrita
                        oWs.Cells[6, ultimaColum + 1].Value = "V";
                        oWs.Cells[6, ultimaColum + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        oWs.Cells[6, ultimaColum + 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        oWs.Cells[6, ultimaColum + 1].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center; // alinear texto
                        oWs.Cells[6, ultimaColum + 1].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF")); // color de la letra
                        oWs.Cells[6, ultimaColum + 1].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;   // fondo de celda
                        oWs.Cells[6, ultimaColum + 1].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue); // fondo de celda

                        oWs.Cells[6, ultimaColum + 2].Style.WrapText = true;
                        oWs.Cells[6, ultimaColum + 2].Style.Font.Size = 11; //letra tamaño  
                        oWs.Cells[6, ultimaColum + 2].Style.Font.Bold = true; //Letra negrita
                        oWs.Cells[6, ultimaColum + 2].Value = "S";
                        oWs.Cells[6, ultimaColum + 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        oWs.Cells[6, ultimaColum + 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        oWs.Cells[6, ultimaColum + 2].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center; // alinear texto
                        oWs.Cells[6, ultimaColum + 2].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF")); // color de la letra
                        oWs.Cells[6, ultimaColum + 2].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;   // fondo de celda
                        oWs.Cells[6, ultimaColum + 2].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue); // fondo de celda

                        oWs.Cells[6, ultimaColum + 3].Style.WrapText = true;
                        oWs.Cells[6, ultimaColum + 3].Style.Font.Size = 11; //letra tamaño  
                        oWs.Cells[6, ultimaColum + 3].Style.Font.Bold = true; //Letra negrita
                        oWs.Cells[6, ultimaColum + 3].Value = "D";
                        oWs.Cells[6, ultimaColum + 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        oWs.Cells[6, ultimaColum + 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        oWs.Cells[6, ultimaColum + 3].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center; // alinear texto
                        oWs.Cells[6, ultimaColum + 3].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF")); // color de la letra
                        oWs.Cells[6, ultimaColum + 3].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;   // fondo de celda
                        oWs.Cells[6, ultimaColum + 3].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Green); // fondo de celda

                    //FIM DE  CCABECERA  NUMERO DIA

                    //Aplicando color de Fondo Rojo  para Fila 5 y 6
                    for (int i = 0; i < ListaFinSemana.Count; i++)
                    {
                        for (int j = 5; j <= 6; j++)
                        {
                            oWs.Cells[j, ListaFinSemana[i]].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#FF0000")); // color de la letra
                            oWs.Cells[j, ListaFinSemana[i]].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;   // fondo de celda
                            oWs.Cells[j, ListaFinSemana[i]].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.PowderBlue); // fondo de celda
                        }
                    }
                    //Aplicando color de Fondo Rojo
                    
                    //var headerCells = oWs.Cells[6, 1, 6, 12];
                    //var headerFont = headerCells.Style.Font;
                    //headerFont.SetFromFont(new Font("Tahoma", 9));

                    // marco Cabecera Personal
                    for (int i = 1; i <= 12; i++)
                    {
                        oWs.Cells[_filaInicio, i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        oWs.Cells[_filaInicio, i].Style.Font.Size = 9; //letra tamaño   
                        oWs.Cells[_filaInicio, i].Style.Font.Bold = true; //Letra negrita
                        oWs.Cells[_filaInicio, i].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#000080")); // color de la letra
                        oWs.Cells[_filaInicio, i].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;   // fondo de celda
                        oWs.Cells[_filaInicio, i].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.PowderBlue); // fondo de celda
                    }

                    oWs.Cells[_filaInicio, 1].Value = "Item";
                    oWs.Cells[_filaInicio, 2].Value = "EMPRESA";
                    oWs.Cells[_filaInicio, 3].Value = "DNI";

                    oWs.Cells[_filaInicio, 4].Value = "APELLIDOS Y NOMBRES";
                    oWs.Cells[_filaInicio, 5].Value = "FECHA INGRESO";
                    oWs.Cells[_filaInicio, 6].Value = "TEC A CARGO";

                    oWs.Cells[_filaInicio, 7].Value = "CARGO";
                    oWs.Cells[_filaInicio, 8].Value = "ESTADO";
                    oWs.Cells[_filaInicio, 9].Value = "CECO";

                    oWs.Cells[_filaInicio, 10].Value = "OP";
                    oWs.Cells[_filaInicio, 11].Value = "RESPONSABLE";
                    oWs.Cells[_filaInicio, 12].Value = "COMENTARIOS";

                    int inc=1;
                    int cant_col = 0;
                    foreach (DataRow oBj in ListaAgrupada_Personal.Rows)
                    {
                        cant_col = 0;
                        cant_col = (16 + (ListaFechas_Agrupada.Rows.Count * 2));
                        //headerCells = oWs.Cells[_fila, 1, _fila, cant_col];
                        //headerFont = headerCells.Style.Font;
                        //headerFont.SetFromFont(new Font("Tahoma", 8));                                   

                        for (int i = 1; i <= 12; i++)
                        {
                            oWs.Cells[_fila, i].Style.Font.Size = 8; //letra tamaño   
                            oWs.Cells[_fila, i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        }

                        oWs.Cells[_fila, 1].Value = inc;
                        oWs.Cells[_fila, 2].Value = oBj["empresa"].ToString();
                        oWs.Cells[_fila, 3].Value = oBj["dni"].ToString();
                        oWs.Cells[_fila, 4].Value = oBj["apellidos_nombres"].ToString();
                        oWs.Cells[_fila, 5].Value = oBj["fecha_ingreso"].ToString();

                        oWs.Cells[_fila, 6].Value = oBj["tec_cargo"].ToString();
                        oWs.Cells[_fila, 7].Value = oBj["cargo"].ToString();
                        oWs.Cells[_fila, 8].Value = oBj["estado"].ToString();
                        oWs.Cells[_fila, 9].Value = oBj["ceco"].ToString();

                        oWs.Cells[_fila, 10].Value = oBj["op"].ToString();
                        oWs.Cells[_fila, 11].Value = oBj["responsable"].ToString();
                        oWs.Cells[_fila, 12].Value = oBj["comentarios"].ToString();

                        ultimaColum = 13;
                        //AGREGANDO DETALLE DIAS

                        var color = "";
                        foreach (DataRow item in ListaFechas_Agrupada.Rows)
                        {
                            oWs.Cells[_fila, ultimaColum].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);          
                            oWs.Cells[_fila, ultimaColum].Style.Font.Size = 8; //letra tamaño  

                            color = Get_VerificarAsistencia(dt_detalles_Tareos, Convert.ToInt32(oBj["id_personal"].ToString()), item["fecha"].ToString());
                            oWs.Cells[_fila, ultimaColum].Value = color;

                            if (color == "F")
                            {
                                oWs.Cells[_fila, ultimaColum].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF")); // color de la letra
                                oWs.Cells[_fila, ultimaColum].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;   // fondo de celda
                                oWs.Cells[_fila, ultimaColum].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Red); // fondo de celda
                            }
                            else if (color == "V")
                            {
                                oWs.Cells[_fila, ultimaColum].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF")); // color de la letra
                                oWs.Cells[_fila, ultimaColum].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;   // fondo de celda
                                oWs.Cells[_fila, ultimaColum].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.SkyBlue); // fondo de celda
                            }
                            else if (color == "S")
                            {
                                oWs.Cells[_fila, ultimaColum].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF")); // color de la letra
                                oWs.Cells[_fila, ultimaColum].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;   // fondo de celda
                                oWs.Cells[_fila, ultimaColum].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Blue ); // fondo de celda
                            }
                            else if (color == "R")
                            {
                                oWs.Cells[_fila, ultimaColum].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF")); // color de la letra
                                oWs.Cells[_fila, ultimaColum].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;   // fondo de celda
                                oWs.Cells[_fila, ultimaColum].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Orange); // fondo de celda
                            }
                            else if (color == "D")
                            {
                                oWs.Cells[_fila, ultimaColum].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF")); // color de la letra
                                oWs.Cells[_fila, ultimaColum].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;   // fondo de celda
                                oWs.Cells[_fila, ultimaColum].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Green); // fondo de celda
                            }

                            oWs.Cells[_fila, ultimaColum].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            oWs.Cells[_fila, ultimaColum].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center ; // alinear texto
                            ultimaColum = ultimaColum + 1;
                            oWs.Cells[_fila, ultimaColum].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);       
                            oWs.Cells[_fila, ultimaColum].Style.Font.Size = 8; //letra tamaño  
                            oWs.Cells[_fila, ultimaColum].Value = Get_VerificarHorasExtras(dt_detalles_Tareos, Convert.ToInt32(oBj["id_personal"].ToString()), item["fecha"].ToString());
                            oWs.Cells[_fila, ultimaColum].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            oWs.Cells[_fila, ultimaColum].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center ; // alinear texto
                            ultimaColum = ultimaColum + 1;
                        }
                        //FIN DE  DETALLE DIAS 

                        //AGREGANDO TOTAL DIA
                        oWs.Cells[_fila, ultimaColum].Style.Font.Size = 8; //letra tamaño  
                        oWs.Cells[_fila, ultimaColum].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        oWs.Cells[_fila, ultimaColum].Value = Get_TotalesTipoAsistencia(dt_detalles_Tareos, Convert.ToInt32(oBj["id_personal"].ToString()), "F");
                        oWs.Cells[_fila, ultimaColum].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        oWs.Cells[_fila, ultimaColum].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center; // alinear texto

                        oWs.Cells[_fila, ultimaColum + 1].Style.Font.Size = 8; //letra tamaño  
                        oWs.Cells[_fila, ultimaColum + 1].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        oWs.Cells[_fila, ultimaColum + 1].Value = Get_TotalesTipoAsistencia(dt_detalles_Tareos, Convert.ToInt32(oBj["id_personal"].ToString()), "V");
                        oWs.Cells[_fila, ultimaColum + 1].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        oWs.Cells[_fila, ultimaColum + 1].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center; // alinear texto

                        oWs.Cells[_fila, ultimaColum + 2].Style.Font.Size = 8; //letra tamaño  
                        oWs.Cells[_fila, ultimaColum + 2].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        oWs.Cells[_fila, ultimaColum + 2].Value = Get_TotalesTipoAsistencia(dt_detalles_Tareos, Convert.ToInt32(oBj["id_personal"].ToString()), "S");
                        oWs.Cells[_fila, ultimaColum + 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        oWs.Cells[_fila, ultimaColum + 2].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center; // alinear texto

                        oWs.Cells[_fila, ultimaColum + 3].Style.Font.Size = 8; //letra tamaño
                        oWs.Cells[_fila, ultimaColum + 3].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        oWs.Cells[_fila, ultimaColum + 3].Value = Get_TotalesTipoAsistencia(dt_detalles_Tareos, Convert.ToInt32(oBj["id_personal"].ToString()), "DM");
                        oWs.Cells[_fila, ultimaColum + 3].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        oWs.Cells[_fila, ultimaColum + 3].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center; // alinear texto
                        ////FIN DE TOTAL DIA
                        _fila++;
                        inc = inc + 1;
                    }

                    cant_col = 0;
                    cant_col = (12 + (ListaFechas_Agrupada.Rows.Count * 2));
                    for (int i = 1; i <=cant_col ; i++)
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
