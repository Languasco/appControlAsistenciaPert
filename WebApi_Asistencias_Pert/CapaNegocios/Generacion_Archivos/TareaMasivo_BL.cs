using CapaDatos.Proceso;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Excel = OfficeOpenXml;
using Style = OfficeOpenXml.Style;

namespace CapaNegocios.Proceso
{
    public class TareaMasivo_BL
    {
        string cadenaCnx = System.Configuration.ConfigurationManager.ConnectionStrings["conexionDsige"].ConnectionString;


        public DataTable Get_RelacionTareos(int local, int OT_contable, int anio, int mes)
        {
            DataTable resultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(cadenaCnx))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_S_TAREO_MASIVO", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id_local", SqlDbType.Int).Value = local;
                        cmd.Parameters.Add("@id_ot", SqlDbType.Int).Value = OT_contable;
                        cmd.Parameters.Add("@anio", SqlDbType.Int).Value = anio;
                        cmd.Parameters.Add("@mes", SqlDbType.Int).Value = mes;

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

        public DataTable Get_PersonalAgrupado(int local, int OT_contable, int anio, int mes)
        {
            DataTable resultado = new DataTable();
            try
            {
                using (SqlConnection cn = new SqlConnection(cadenaCnx))
                {
                    cn.Open();
                    using (SqlCommand cmd = new SqlCommand("SP_S_TAREO_MASIVO_AGRUPADO", cn))
                    {
                        cmd.CommandTimeout = 0;
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@id_local", SqlDbType.Int).Value = local;
                        cmd.Parameters.Add("@id_ot", SqlDbType.Int).Value = OT_contable;
                        cmd.Parameters.Add("@anio", SqlDbType.Int).Value = anio;
                        cmd.Parameters.Add("@mes", SqlDbType.Int).Value = mes;

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


        public string Get_VerificarAsistencia(DataTable Lista_Data, int id_personal, int dia)
        {
            string res = "";
            string cant = "0.0000";

            for (int i = 0; i < Lista_Data.Rows.Count; i++)
            {
               if (id_personal == Convert.ToInt32(Lista_Data.Rows[i]["id_personal"].ToString()) && dia == Convert.ToInt32(Lista_Data.Rows[i]["dia"].ToString()) && Lista_Data.Rows[i]["id_tasi_codigo"].ToString() == "A")
                {
                    cant = "8.0000";
                    break;
                }
            }
            res = Convert.ToString(cant);
            return res;
        }

        public string Get_TotalDiaAsistencia(DataTable Lista_Data, int id_personal)
        {
            string res = "";
            int cant = 0;

            for (int i = 0; i < Lista_Data.Rows.Count; i++)
            {
                if (id_personal == Convert.ToInt32(Lista_Data.Rows[i]["id_personal"].ToString()) && Lista_Data.Rows[i]["id_tasi_codigo"].ToString() == "A")
                {
                    cant = (cant + 8);
                }
            }
            res = string.Format("{0:0.00}", cant);
            return res;
        }

        public string Listar_TareoMasivo(int local, int OT_contable, int anio, int mes)
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
                FileRuta = System.Web.Hosting.HostingEnvironment.MapPath("~/Temp/Descargas/TareoMasivo_" + _servidor);
                string rutaServer = ConfigurationManager.AppSettings["ArchivoTexto_IngresoTrabajador"];

                FileExcel = rutaServer + "TareoMasivo_" + _servidor;
                FileInfo _file = new FileInfo(FileRuta);
                if (_file.Exists)
                {
                    _file.Delete();
                    _file = new FileInfo(FileRuta);
                }

                //relacion de tareos 
                DataTable dt_detalles_Tareos = new DataTable();
                dt_detalles_Tareos = Get_RelacionTareos(local, OT_contable, anio, mes);

                if (dt_detalles_Tareos.Rows.Count <= 0)
                {
                    Res = "0|No hay informacion disponible";
                    return Res;
                }

                DataTable ListaAgrupada_Personal = new DataTable();
                ListaAgrupada_Personal = Get_PersonalAgrupado(local, OT_contable, anio, mes);

                List<TareoMasivo_E> ListaMeses = new List<TareoMasivo_E>();

                //Cabeceras de Meses
                for (int i = 1; i <=30 ;  i++)
                {
                    TareoMasivo_E obj_ent = new TareoMasivo_E(); ;
                    obj_ent.numero_dia= i;
                    ListaMeses.Add(obj_ent);
                }          
                                    //id_personal, codigo_personal, personal, codigo_OTContable, dia
                 using (Excel.ExcelPackage oEx = new Excel.ExcelPackage(_file))
                {
                    Excel.ExcelWorksheet oWs = oEx.Workbook.Worksheets.Add("Tareo_Masivo");
                      

                    oWs.Cells[1, 1].Value = "EMPRESA";
                    oWs.Cells[1, 1].Style.Font.Size = 8; //letra tamaño   
                    oWs.Cells[1, 1].Style.Font.Bold = true; //Letra negrita

                    oWs.Cells[2, 1].Value = "PLANILLA";
                    oWs.Cells[2, 1].Style.Font.Size = 8; //letra tamaño   
                    oWs.Cells[2, 1].Style.Font.Bold = true; //Letra negrita

                    oWs.Cells[3, 1].Value = "AÑO";
                    oWs.Cells[3, 1].Style.Font.Size = 8; //letra tamaño   
                    oWs.Cells[3, 1].Style.Font.Bold = true; //Letra negrita

                    oWs.Cells[4, 1].Value = "MES";
                    oWs.Cells[4, 1].Style.Font.Size = 8; //letra tamaño   
                    oWs.Cells[4, 1].Style.Font.Bold = true; //Letra negrita

                    oWs.Cells[1, 2].Value = "01";
                    oWs.Cells[1, 2].Style.Font.Size = 8; //letra tamaño   
                    oWs.Cells[1, 2].Style.Font.Bold = true; //Letra negrita

                    oWs.Cells[2, 2].Value = "EMP/OBC";
                    oWs.Cells[2, 2].Style.Font.Size = 8; //letra tamaño   
                    oWs.Cells[2, 2].Style.Font.Bold = true; //Letra negrita

                    oWs.Cells[3, 2].Value = anio;
                    oWs.Cells[3, 2].Style.Font.Size = 8; //letra tamaño   
                    oWs.Cells[3, 2].Style.Font.Bold = true; //Letra negrita
                    oWs.Cells[3, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    oWs.Cells[3, 2].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left; // alinear texto

                    oWs.Cells[4, 2].Value = mes;
                    oWs.Cells[4, 2].Style.Font.Size = 8; //letra tamaño   
                    oWs.Cells[4, 2].Style.Font.Bold = true; //Letra negrita
                    oWs.Cells[4, 2].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    oWs.Cells[4, 2].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Left; // alinear texto

                    for (int i = 1; i <= 3; i++)
                    {                         
                        oWs.Cells[_filaInicio, i].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        oWs.Cells[_filaInicio, i].Style.Font.Size = 9; //letra tamaño   
                        oWs.Cells[_filaInicio, i].Style.Font.Bold = true; //Letra negrita
                    } 

                    oWs.Cells[_filaInicio, 1].Value = "Código Trabajador";
                    oWs.Cells[_filaInicio, 2].Value = "Nombre Trabajador";
                    oWs.Cells[_filaInicio, 3].Value = "OT/Centro de Costo";   
                                        
                    ultimaColum=4;
                    //AGREGANDO CABECERA DIAS
                    foreach (TareoMasivo_E item in ListaMeses)
                    {
                        oWs.Cells[_filaInicio, ultimaColum].Style.Font.Size =14; //letra tamaño  
                        oWs.Cells[_filaInicio, ultimaColum].Style.Font.Bold = true; //Letra negrita
                        oWs.Cells[_filaInicio, ultimaColum].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                        oWs.Cells[_filaInicio, ultimaColum].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                        oWs.Cells[_filaInicio, ultimaColum].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center; // alinear texto
                        oWs.Cells[_filaInicio, ultimaColum].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF")); // color de la letra
                        oWs.Cells[_filaInicio, ultimaColum].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;   // fondo de celda
                        oWs.Cells[_filaInicio, ultimaColum].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Black); // fondo de celda
                        oWs.Cells[_filaInicio, ultimaColum].Value = item.numero_dia; 
                        ultimaColum = ultimaColum + 1;
                    }
                    //FIM DE CABECERA  DIAS
                    ultimaColum = ultimaColum + 1;

                    oWs.Cells[6, ultimaColum].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
                    oWs.Cells[6, ultimaColum].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                    oWs.Cells[6, ultimaColum].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Center; // alinear texto
                    oWs.Cells[6, ultimaColum].Style.Font.Color.SetColor(System.Drawing.ColorTranslator.FromHtml("#FFFFFF")); // color de la letra
                    oWs.Cells[6, ultimaColum].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;   // fondo de celda
                    oWs.Cells[6, ultimaColum].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.Black); // fondo de celda
                    oWs.Cells[6, ultimaColum].Style.Font.Size = 14; //letra tamaño  
                    oWs.Cells[6, ultimaColum].Style.Font.Bold = true; //Letra negrita
                    oWs.Cells[6, ultimaColum].Value = "TOTAL";
     
                    foreach (DataRow oBj in ListaAgrupada_Personal.Rows)
                    {
                        for (int i = 1; i <= 3; i++)
                        {
                            oWs.Cells[_fila, i].Style.Font.Size = 8; //letra tamaño   
                        } 

                        oWs.Cells[_fila, 1].Value = oBj["codigo_personal"].ToString();
                        oWs.Cells[_fila, 2].Value = oBj["personal"].ToString();
                        oWs.Cells[_fila, 3].Value = oBj["codigo_OTContable"].ToString();

                   
                        ultimaColum = 4;
                        //AGREGANDO DETALLE DIAS
                        foreach (TareoMasivo_E item in ListaMeses)
                        {
                            oWs.Column(ultimaColum).AutoFit();
                            oWs.Cells[_fila, ultimaColum].Style.WrapText = true;
                            oWs.Cells[_fila, ultimaColum].Style.Font.Size = 8; //letra tamaño  
                            oWs.Cells[_fila, ultimaColum].Value = Get_VerificarAsistencia(dt_detalles_Tareos, Convert.ToInt32(oBj["id_personal"].ToString()), item.numero_dia);
                            oWs.Cells[_fila, ultimaColum].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            oWs.Cells[_fila, ultimaColum].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Right; // alinear texto
                            ultimaColum = ultimaColum + 1;
                        }
                        //FIN DE  DETALLE DIAS 

                        //AGREGANDO TOTAL DIA
 
                            oWs.Column(35).AutoFit();
                            oWs.Cells[_fila, 35].Style.WrapText = true;
                            oWs.Cells[_fila, 35].Style.Font.Size = 9; //letra tamaño  
                            oWs.Cells[_fila, 35].Style.Font.Bold = true; //Letra negrita
                            oWs.Cells[_fila, 35].Value = Get_TotalDiaAsistencia(dt_detalles_Tareos, Convert.ToInt32(oBj["id_personal"].ToString()));
                            oWs.Cells[_fila, 35].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
                            oWs.Cells[_fila, 35].Style.HorizontalAlignment = Style.ExcelHorizontalAlignment.Right; // alinear texto
 
                        ////FIN DE TOTAL DIA
                        
                        _fila++;
                    }
 
                    for (int i =1 ; i <= 33; i++)
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
