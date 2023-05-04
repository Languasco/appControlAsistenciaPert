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
    public class tblGes_LocalController : ApiController
    {
        private CAM_ControlAsistenciaEntities db = new CAM_ControlAsistenciaEntities();

        // GET: api/tblGes_Local
        public IQueryable<tbl_Ges_Local> Gettbl_Ges_Local()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.tbl_Ges_Local;
        }

        // GET: api/tblGes_Local/5
        [ResponseType(typeof(tbl_Ges_Local))]
        public IHttpActionResult Gettbl_Ges_Local(int id)
        {
            tbl_Ges_Local tbl_Ges_Local = db.tbl_Ges_Local.Find(id);
            if (tbl_Ges_Local == null)
            {
                return NotFound();
            }

            return Ok(tbl_Ges_Local);
        }

        // PUT: api/tblGes_Local/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_Ges_Local(int id, tbl_Ges_Local obj_local)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != obj_local.id_local)
            {
                return BadRequest();
            }

            tbl_Ges_Local Ent_local_R;
            // DATA ACTUAL
            Ent_local_R = db.tbl_Ges_Local.Where(g => g.id_local == obj_local.id_local).FirstOrDefault<tbl_Ges_Local>();

            Ent_local_R.nombre_local = obj_local.nombre_local;
            Ent_local_R.estado = obj_local.estado;
            Ent_local_R.fecha_edicion = DateTime.Now;
            Ent_local_R.usuario_edicion = obj_local.usuario_creacion;
            db.Entry(Ent_local_R).State = EntityState.Modified;

            //db.Entry(tbl_Ges_Local).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_Ges_LocalExists(id))
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

        // POST: api/tblGes_Local
        [ResponseType(typeof(tbl_Ges_Local))]
        public IHttpActionResult Posttbl_Ges_Local(tbl_Ges_Local tbl_Ges_Local)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            tbl_Ges_Local.fecha_creacion = DateTime.Now;
            db.tbl_Ges_Local.Add(tbl_Ges_Local);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tbl_Ges_Local.id_local }, tbl_Ges_Local);
        }


        // DELETE: 
        [ResponseType(typeof(tbl_Ges_Local))]
        public async Task<IHttpActionResult> Deletetbl_Asis_Turno(int id)
        {
            tbl_Ges_Local ent_local_A = await db.tbl_Ges_Local.FindAsync(id);
            ent_local_A = db.tbl_Ges_Local.Where(g => g.id_local  == id).FirstOrDefault<tbl_Ges_Local>();
            ent_local_A.estado = 0;
            db.Entry(ent_local_A).State = System.Data.Entity.EntityState.Modified;
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

        private bool tbl_Ges_LocalExists(int id)
        {
            return db.tbl_Ges_Local.Count(e => e.id_local == id) > 0;
        }
    }
}