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

namespace WebApi_Asistencias_Pert.Controllers.Mantenimiento
{
    [EnableCors("*", "*", "*")]
    public class tblAsis_TipoAsistenciaController : ApiController
    {
        private CAM_ControlAsistenciaEntities db = new CAM_ControlAsistenciaEntities();

        // GET: api/tblAsis_TipoAsistencia
        public IQueryable<tbl_Asis_TipoAsistencia> Gettbl_Asis_TipoAsistencia()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.tbl_Asis_TipoAsistencia;
        }


        public object GetAsis_TipoAsistencia(int opcion, string filtro)
        {
            //filtro puede tomar cualquier valor
            db.Configuration.ProxyCreationEnabled = false;
            object resul = null;

            try
            {

                if (opcion == 1)
                {
                    string[] parametros = filtro.Split('|');
                    string codigoTipo = parametros[0].ToString();

                    resul = db.tbl_Asis_TipoAsistencia.Count(e => e.id_tasi_codigo == codigoTipo);

                }
                else
                {
                    resul = "Opcion selecciona invalida";

                }

            }
            catch (Exception ex)
            {
                resul = ex.Message;
            }
            return resul;
        }


        // PUT: api/tblAsis_TipoAsistencia/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_Asis_TipoAsistencia(string id, tbl_Asis_TipoAsistencia obj)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != obj.id_tasi_codigo)
            {
                return BadRequest();
            }

            tbl_Asis_TipoAsistencia Ent_R;
            // DATA ACTUAL
            Ent_R = db.tbl_Asis_TipoAsistencia.Where(g => g.id_tasi_codigo  == obj.id_tasi_codigo).FirstOrDefault<tbl_Asis_TipoAsistencia>();

            //Ent_R.id_tasi_codigo = obj.id_tasi_codigo;
            Ent_R.descripcion_tasi = obj.descripcion_tasi;
            Ent_R.abreviatura_tasi = obj.abreviatura_tasi;
            Ent_R.valor_tasi = obj.valor_tasi;
            Ent_R.estado = obj.estado;
            Ent_R.fecha_edicion = DateTime.Now;
            Ent_R.usuario_edicion = obj.usuario_creacion;

            db.Entry(Ent_R).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_Asis_TipoAsistenciaExists(id))
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

        // POST: api/tblAsis_TipoAsistencia
        [ResponseType(typeof(tbl_Asis_TipoAsistencia))]
        public IHttpActionResult Posttbl_Asis_TipoAsistencia(tbl_Asis_TipoAsistencia tbl_Asis_TipoAsistencia)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            tbl_Asis_TipoAsistencia.fecha_creacion = DateTime.Now;
            db.tbl_Asis_TipoAsistencia.Add(tbl_Asis_TipoAsistencia);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbl_Asis_TipoAsistenciaExists(tbl_Asis_TipoAsistencia.id_tasi_codigo))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_Asis_TipoAsistencia.id_tasi_codigo }, tbl_Asis_TipoAsistencia);
        }



        [ResponseType(typeof(tbl_Asis_Turno))]
        public async Task<IHttpActionResult> Deletetbl_Asis_TipoAsistencia(string id)
        {
            tbl_Asis_TipoAsistencia Entidad_A = await db.tbl_Asis_TipoAsistencia.FindAsync(id);
            Entidad_A = db.tbl_Asis_TipoAsistencia.Where(g => g.id_tasi_codigo == id).FirstOrDefault<tbl_Asis_TipoAsistencia>();
            Entidad_A.estado = 0;
            db.Entry(Entidad_A).State = System.Data.Entity.EntityState.Modified;
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

        private bool tbl_Asis_TipoAsistenciaExists(string id)
        {
            return db.tbl_Asis_TipoAsistencia.Count(e => e.id_tasi_codigo == id) > 0;
        }
    }
}