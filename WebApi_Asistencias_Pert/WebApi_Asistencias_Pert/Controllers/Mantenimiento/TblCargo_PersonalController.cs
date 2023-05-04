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
    public class TblCargo_PersonalController : ApiController
    {
        private CAM_ControlAsistenciaEntities db = new CAM_ControlAsistenciaEntities();

        // GET: api/TblCargo_Personal
        public IQueryable<Tbl_Cargo_Personal> GetTbl_Cargo_Personal()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.Tbl_Cargo_Personal;
        }

        // GET: api/TblCargo_Personal/5
        [ResponseType(typeof(Tbl_Cargo_Personal))]
        public IHttpActionResult GetTbl_Cargo_Personal(int id)
        {
            Tbl_Cargo_Personal tbl_Cargo_Personal = db.Tbl_Cargo_Personal.Find(id);
            if (tbl_Cargo_Personal == null)
            {
                db.Configuration.ProxyCreationEnabled = false;
                return NotFound();
            }

            return Ok(tbl_Cargo_Personal);
        }

        // PUT: api/TblCargo_Personal/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutTbl_Cargo_Personal(int id, Tbl_Cargo_Personal tbl_Cargo_Personal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_Cargo_Personal.id_Cargo)
            {
                return BadRequest();
            }

            tbl_Cargo_Personal.fecha_edicion = DateTime.Now;
            tbl_Cargo_Personal.usuario_edicion = tbl_Cargo_Personal.usuario_creacion;
            db.Entry(tbl_Cargo_Personal).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!Tbl_Cargo_PersonalExists(id))
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

        // POST: api/TblCargo_Personal
        [ResponseType(typeof(Tbl_Cargo_Personal))]
        public IHttpActionResult PostTbl_Cargo_Personal(Tbl_Cargo_Personal tbl_Cargo_Personal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            tbl_Cargo_Personal.fecha_creacion = DateTime.Now;
            db.Tbl_Cargo_Personal.Add(tbl_Cargo_Personal);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tbl_Cargo_Personal.id_Cargo }, tbl_Cargo_Personal);
        }

        //// DELETE: api/TblCargo_Personal/5
        //[ResponseType(typeof(Tbl_Cargo_Personal))]
        //public IHttpActionResult DeleteTbl_Cargo_Personal(int id)
        //{
        //    Tbl_Cargo_Personal tbl_Cargo_Personal = db.Tbl_Cargo_Personal.Find(id);
        //    if (tbl_Cargo_Personal == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Tbl_Cargo_Personal.Remove(tbl_Cargo_Personal);
        //    db.SaveChanges();

        //    return Ok(tbl_Cargo_Personal);
        //}

        [ResponseType(typeof(Tbl_Cargo_Personal ))]
        public async Task<IHttpActionResult> DeleteTbl_Cargo_Personal(int id)
        {
            Tbl_Cargo_Personal Entidad_A = await db.Tbl_Cargo_Personal.FindAsync(id);
            Entidad_A = db.Tbl_Cargo_Personal.Where(g => g.id_Cargo == id).FirstOrDefault<Tbl_Cargo_Personal>();
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

        private bool Tbl_Cargo_PersonalExists(int id)
        {
            return db.Tbl_Cargo_Personal.Count(e => e.id_Cargo == id) > 0;
        }
    }
}