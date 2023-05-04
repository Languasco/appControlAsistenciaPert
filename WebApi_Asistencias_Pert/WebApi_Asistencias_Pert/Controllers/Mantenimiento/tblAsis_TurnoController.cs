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
    public class tblAsis_TurnoController : ApiController
    {
        private CAM_ControlAsistenciaEntities db = new CAM_ControlAsistenciaEntities();

        // GET: api/tblAsis_Turno
        public IQueryable<tbl_Asis_Turno> Gettbl_Asis_Turno()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.tbl_Asis_Turno;
        }

        // GET: api/tblAsis_Turno/5
        [ResponseType(typeof(tbl_Asis_Turno))]
        public IHttpActionResult Gettbl_Asis_Turno(int id)
        {
            tbl_Asis_Turno tbl_Asis_Turno = db.tbl_Asis_Turno.Find(id);
            if (tbl_Asis_Turno == null)
            {
                return NotFound();
            }

            return Ok(tbl_Asis_Turno);
        }

        // PUT: api/tblAsis_Turno/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_Asis_Turno(int id, tbl_Asis_Turno obj_turno)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != obj_turno.id_turno)
            {
                return BadRequest();
            }

            tbl_Asis_Turno Ent_turno_R;
            // DATA ACTUAL
            Ent_turno_R = db.tbl_Asis_Turno.Where(g => g.id_turno == obj_turno.id_turno).FirstOrDefault<tbl_Asis_Turno>();

            Ent_turno_R.nombre_turno = obj_turno.nombre_turno;
            Ent_turno_R.simbolo_turno = obj_turno.simbolo_turno;
            Ent_turno_R.horaInicio_turno = obj_turno.horaInicio_turno;
            Ent_turno_R.horaTermino_turno = obj_turno.horaTermino_turno;
            Ent_turno_R.tiempoMaxInicio_turno = obj_turno.tiempoMaxInicio_turno;
            Ent_turno_R.estado = obj_turno.estado;

            Ent_turno_R.fecha_edicion = DateTime.Now;
            Ent_turno_R.usuario_edicion = obj_turno.usuario_creacion;
            db.Entry(Ent_turno_R).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_Asis_TurnoExists(id))
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

        // POST: api/tblAsis_Turno
        [ResponseType(typeof(tbl_Asis_Turno))]
        public IHttpActionResult Posttbl_Asis_Turno(tbl_Asis_Turno tbl_Asis_Turno)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            tbl_Asis_Turno.fecha_creacion = DateTime.Now;
            db.tbl_Asis_Turno.Add(tbl_Asis_Turno);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tbl_Asis_Turno.id_turno }, tbl_Asis_Turno);
        }

 
        // DELETE: 
        [ResponseType(typeof(tbl_Asis_Turno))]
        public async Task<IHttpActionResult> Deletetbl_Asis_Turno(int id)
        {
            tbl_Asis_Turno tblturno_A = await db.tbl_Asis_Turno.FindAsync(id);
            tblturno_A = db.tbl_Asis_Turno.Where(g => g.id_turno == id).FirstOrDefault<tbl_Asis_Turno>();
            tblturno_A.estado = 0;
            db.Entry(tblturno_A).State = System.Data.Entity.EntityState.Modified;
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

        private bool tbl_Asis_TurnoExists(int id)
        {
            return db.tbl_Asis_Turno.Count(e => e.id_turno == id) > 0;
        }
    }
}