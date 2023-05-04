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

namespace WebApi_Asistencias_Pert.Controllers.Mantenimiento
{
     [EnableCors("*", "*", "*")]
    public class tblDelegacionController : ApiController
    {
        private CAM_ControlAsistenciaEntities db = new CAM_ControlAsistenciaEntities();

        // GET: api/tblDelegacion
        public IQueryable<tbl_Delegacion> Gettbl_Delegacion()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.tbl_Delegacion;
        }

        // GET: api/tblDelegacion/5
        [ResponseType(typeof(tbl_Delegacion))]
        public IHttpActionResult Gettbl_Delegacion(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            tbl_Delegacion tbl_Delegacion = db.tbl_Delegacion.Find(id);
            if (tbl_Delegacion == null)
            {
                return NotFound();
            }

            return Ok(tbl_Delegacion);
        }

        // PUT: api/tblDelegacion/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_Delegacion(int id, tbl_Delegacion tbl_Delegacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_Delegacion.id_delegacion)
            {
                return BadRequest();
            }

            db.Entry(tbl_Delegacion).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_DelegacionExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/tblDelegacion
        [ResponseType(typeof(tbl_Delegacion))]
        public IHttpActionResult Posttbl_Delegacion(tbl_Delegacion tbl_Delegacion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_Delegacion.Add(tbl_Delegacion);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tbl_Delegacion.id_delegacion }, tbl_Delegacion);
        }

        // DELETE: api/tblDelegacion/5
        [ResponseType(typeof(tbl_Delegacion))]
        public IHttpActionResult Deletetbl_Delegacion(int id)
        {
            tbl_Delegacion tbl_Delegacion = db.tbl_Delegacion.Find(id);
            if (tbl_Delegacion == null)
            {
                return NotFound();
            }

            db.tbl_Delegacion.Remove(tbl_Delegacion);
            db.SaveChanges();

            return Ok(tbl_Delegacion);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_DelegacionExists(int id)
        {
            return db.tbl_Delegacion.Count(e => e.id_delegacion == id) > 0;
        }
    }
}