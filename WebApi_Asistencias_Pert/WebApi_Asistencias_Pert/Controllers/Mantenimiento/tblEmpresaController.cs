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
    public class tblEmpresaController : ApiController
    {
        private CAM_ControlAsistenciaEntities db = new CAM_ControlAsistenciaEntities();

        // GET: api/tblEmpresa
        public IQueryable<tbl_Empresa> Gettbl_Empresa()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.tbl_Empresa;
        }

        // GET: api/tblEmpresa/5
        [ResponseType(typeof(tbl_Empresa))]
        public IHttpActionResult Gettbl_Empresa(int id)
        {
            db.Configuration.ProxyCreationEnabled = false;
            tbl_Empresa tbl_Empresa = db.tbl_Empresa.Find(id);
            if (tbl_Empresa == null)
            {
                return NotFound();
            }

            return Ok(tbl_Empresa);
        }

        // PUT: api/tblEmpresa/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_Empresa(int id, tbl_Empresa tbl_Empresa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_Empresa.id_empresa)
            {
                return BadRequest();
            }

            db.Entry(tbl_Empresa).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_EmpresaExists(id))
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

        // POST: api/tblEmpresa
        [ResponseType(typeof(tbl_Empresa))]
        public IHttpActionResult Posttbl_Empresa(tbl_Empresa tbl_Empresa)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_Empresa.Add(tbl_Empresa);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = tbl_Empresa.id_empresa }, tbl_Empresa);
        }

        // DELETE: api/tblEmpresa/5
        [ResponseType(typeof(tbl_Empresa))]
        public IHttpActionResult Deletetbl_Empresa(int id)
        {
            tbl_Empresa tbl_Empresa = db.tbl_Empresa.Find(id);
            if (tbl_Empresa == null)
            {
                return NotFound();
            }

            db.tbl_Empresa.Remove(tbl_Empresa);
            db.SaveChanges();

            return Ok(tbl_Empresa);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_EmpresaExists(int id)
        {
            return db.tbl_Empresa.Count(e => e.id_empresa == id) > 0;
        }
    }
}