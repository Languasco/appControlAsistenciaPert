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

namespace WebApi_Asistencias_Pert.Controllers
{
    public class tblUsuariosController : ApiController
    {
        private CAM_ControlAsistenciaEntities db = new CAM_ControlAsistenciaEntities();

        // GET: api/tblUsuarios
        public IQueryable<tbl_Usuarios> Gettbl_Usuarios()
        {
            return db.tbl_Usuarios;
        }

        // GET: api/tblUsuarios/5
        [ResponseType(typeof(tbl_Usuarios))]
        public IHttpActionResult Gettbl_Usuarios(int id)
        {
            tbl_Usuarios tbl_Usuarios = db.tbl_Usuarios.Find(id);
            if (tbl_Usuarios == null)
            {
                return NotFound();
            }

            return Ok(tbl_Usuarios);
        }

        // PUT: api/tblUsuarios/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_Usuarios(int id, tbl_Usuarios tbl_Usuarios)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_Usuarios.id_Usuario)
            {
                return BadRequest();
            }

            db.Entry(tbl_Usuarios).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_UsuariosExists(id))
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

        // POST: api/tblUsuarios
        [ResponseType(typeof(tbl_Usuarios))]
        public IHttpActionResult Posttbl_Usuarios(tbl_Usuarios tbl_Usuarios)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_Usuarios.Add(tbl_Usuarios);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbl_UsuariosExists(tbl_Usuarios.id_Usuario))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_Usuarios.id_Usuario }, tbl_Usuarios);
        }

        // DELETE: api/tblUsuarios/5
        [ResponseType(typeof(tbl_Usuarios))]
        public IHttpActionResult Deletetbl_Usuarios(int id)
        {
            tbl_Usuarios tbl_Usuarios = db.tbl_Usuarios.Find(id);
            if (tbl_Usuarios == null)
            {
                return NotFound();
            }

            db.tbl_Usuarios.Remove(tbl_Usuarios);
            db.SaveChanges();

            return Ok(tbl_Usuarios);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_UsuariosExists(int id)
        {
            return db.tbl_Usuarios.Count(e => e.id_Usuario == id) > 0;
        }
    }
}