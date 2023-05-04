using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using CapaDatos;
using System.Web.Http.Cors;
using System.Threading.Tasks;
using CapaNegocios.Proceso;

namespace WebApi_Asistencias_Pert.Controllers.Mantenimiento
{
    [EnableCors("*", "*", "*")]
    public class tblIngresoPersonalController : ApiController
    {
        private CAM_ControlAsistenciaEntities db = new CAM_ControlAsistenciaEntities();

        // GET: api/tblIngresoPersonal
        public IQueryable<tbl_IngresoPersonal> Gettbl_IngresoPersonal()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.tbl_IngresoPersonal;
        }
        
        public object GetRegistro_Produccion(int opcion, string filtro)
        {
            //filtro puede tomar cualquier valor
            db.Configuration.ProxyCreationEnabled = false;
            object resul = null;

            try
            {
                if (opcion == 1)
                {
                    resul = (from a in db.tbl_Ges_Local
                             select new
                             {
                                 a.id_local,
                                 a.nombre_local,
                                 a.estado
                             }).ToList();
                }
                else if (opcion == 2)
                {
                    resul = (from a in db.tbl_Ges_OT_Contable
                             select new
                             {
                                 a.id_OtContable,
                                 a.codigo_OTContable,
                                 a.descripcion_OTContable
                             }).ToList();
                }
                else if (opcion == 3)
                {

                    string[] parametros = filtro.Split('|');

                    int local = Convert.ToInt32(parametros[0].ToString());
                    int OT_contable = Convert.ToInt32(parametros[1].ToString());
                    string fechaAsistencia = parametros[2].ToString();

                    Registro_Produccion_BL obj_negocio = new Registro_Produccion_BL();
                    resul = obj_negocio.Listar_RegistroProduccion(local, OT_contable, fechaAsistencia);

                }

                else if (opcion == 4)
                {

                    resul = (from a in db.tbl_Personal
                             where a.id_cargo_personal == 10 || a.id_cargo_personal == 8
                             select new
                             {
                               a.id_personal,
                               personal= a.apellidos_personal + " " + a.nombres_personal
                      
                             }).ToList();
                }
                else if (opcion == 5)
                {                    
                    string[] parametros = filtro.Split('|');
                    string dni = parametros[0].ToString();

                    resul = (from a in db.tbl_Personal 
                             join b in db.Tbl_Cargo_Personal  on a.id_cargo_personal equals b.id_Cargo
                             where a.nroDoc_personal == dni
                             select new
                             {
                                 a.id_personal,
                                 personal = a.apellidos_personal + " " + a.nombres_personal,
                                 cargo =b.nombre_cargo 

                             }).ToList();
                }
                else if (opcion == 6)
                {
                    string[] parametros = filtro.Split('|');

                    int id_asistencia = Convert.ToInt32(parametros[0].ToString());
                    int id_Local = Convert.ToInt32(parametros[1].ToString());
                    int id_OTContable = Convert.ToInt32(parametros[2].ToString());
                    string Fecha = parametros[3].ToString();
                    int id_personal = Convert.ToInt32(parametros[4].ToString());
                    string id_tasi_codigo = parametros[5].ToString();
                    string horaIngreso = parametros[6].ToString();
                    string horaSalida = parametros[7].ToString();
                    int id_turno = Convert.ToInt32(parametros[8].ToString());
                    string observacion = parametros[9].ToString();
                    int usuario = Convert.ToInt32(parametros[10].ToString());

                    //RegistroAsistencia_BL obj_negocio = new RegistroAsistencia_BL();
                    //resul = obj_negocio.Insert_Asistencia(id_asistencia, id_Local, id_OTContable, Fecha, id_personal, id_tasi_codigo, horaIngreso, horaSalida, id_turno, observacion, usuario);

                }

                else
                {
                    //string[] parametros = filtro.Split('|');
                    //int id_ordenTrabajo = Convert.ToInt32(parametros[0]);

                    //resul = (from a in db.tbl_OrdenesTrabajo_Baremo
                    //         join b in db.tbl_Baremos on a.id_baremo equals b.id_baremo
                    //         join c in db.tbl_Baremos_UnidadMedida on b.id_unidadMedida equals c.id_unidadMedida
                    //         where a.id_ordenTrabajo == id_ordenTrabajo
                    //         select new
                    //         {
                    //             a.id_OrdenTrabajo_Bar,
                    //             a.id_ordenTrabajo,
                    //             a.id_baremo,
                    //             unidad_medida = c.abreviatura_unidadMedida,
                    //             a.cantidad_OrdenTrabajo_Bar,
                    //             a.cantidad_MovilOrdenTrabajo_Bar,
                    //             a.precio_OrdenTrabajo_Bar,
                    //             a.estado,
                    //             b.codigo_baremo,
                    //             b.descripcion_baremo

                    //         }).ToList();
                    resul = "Opcion selecciona invalida";

                }

            }
            catch (Exception ex)
            {
                resul = ex.Message;
            }
            return resul;
        }
        
        // PUT: api/tblIngresoPersonal/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_IngresoPersonal(int id, tbl_IngresoPersonal obj_data)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != obj_data.id_Ingreso)
            {
                return BadRequest();
            }

            tbl_IngresoPersonal Ent_R;
            // DATA ACTUAL
            Ent_R = db.tbl_IngresoPersonal.Where(g => g.id_Ingreso == obj_data.id_Ingreso).FirstOrDefault<tbl_IngresoPersonal>();

            Ent_R.id_Local = obj_data.id_Local;
            Ent_R.id_OTContable = obj_data.id_OTContable;
            Ent_R.id_Personal_Responsable = obj_data.id_Personal_Responsable;
            Ent_R.id_personal = obj_data.id_personal;
            Ent_R.ImporteProduccion_Ingreso = obj_data.ImporteProduccion_Ingreso;
            Ent_R.obsProduccion_Ingreso = obj_data.obsProduccion_Ingreso;
            Ent_R.ImporteMovilidad_Ingreso = obj_data.ImporteMovilidad_Ingreso;
            Ent_R.obsMovilidad_Ingreso = obj_data.obsMovilidad_Ingreso;
            Ent_R.fecha_ingreso = obj_data.fecha_ingreso;
            Ent_R.estado = obj_data.estado;
 
            Ent_R.fecha_edicion = DateTime.Now;
            Ent_R.usuario_edicion = obj_data.usuario_creacion;
            db.Entry(Ent_R).State = EntityState.Modified;

             try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_IngresoPersonalExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

             return Ok("OK");
        }

        // POST: api/tblIngresoPersonal
        [ResponseType(typeof(tbl_IngresoPersonal))]
        public IHttpActionResult Posttbl_IngresoPersonal(tbl_IngresoPersonal tbl_IngresoPersonal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            tbl_IngresoPersonal.fecha_creacion = DateTime.Now;
            db.tbl_IngresoPersonal.Add(tbl_IngresoPersonal);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbl_IngresoPersonalExists(tbl_IngresoPersonal.id_Ingreso))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_IngresoPersonal.id_Ingreso }, tbl_IngresoPersonal);
        }



        // DELETE: 
        [ResponseType(typeof(tbl_IngresoPersonal))]
        public async Task<IHttpActionResult> Deletetbl_IngresoPersonal(int id)
        {
            tbl_IngresoPersonal tblingrseso_A = await db.tbl_IngresoPersonal.FindAsync(id);
            tblingrseso_A = db.tbl_IngresoPersonal.Where(g => g.id_Ingreso == id).FirstOrDefault<tbl_IngresoPersonal>();
            tblingrseso_A.estado = 0;
            db.Entry(tblingrseso_A).State = System.Data.Entity.EntityState.Modified;
            await db.SaveChangesAsync();
            return Ok("OK");
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_IngresoPersonalExists(int id)
        {
            return db.tbl_IngresoPersonal.Count(e => e.id_Ingreso == id) > 0;
        }
    }
}