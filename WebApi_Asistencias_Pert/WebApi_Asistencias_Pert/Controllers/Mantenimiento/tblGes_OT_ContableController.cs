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
    public class tblGes_OT_ContableController : ApiController
    {
        private CAM_ControlAsistenciaEntities db = new CAM_ControlAsistenciaEntities();

        // GET: api/tblGes_OT_Contable
        public IQueryable<tbl_Ges_OT_Contable> Gettbl_Ges_OT_Contable()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.tbl_Ges_OT_Contable;
        }

        public object GetOT_Contables(int opcion, string filtro)
        {
            //filtro puede tomar cualquier valor
            db.Configuration.ProxyCreationEnabled = false;
            object resul = null;

            try
            {

                if (opcion == 1)
                {
                    resul = (from a in db.tbl_Ges_OT_Contable
                             join b in db.tbl_Empresa on a.id_Empresa equals b.id_empresa
                             join c in db.tbl_Delegacion on a.id_Delegacion equals c.id_delegacion
                             select new
                             {
                                 a.id_OtContable,
                                 a.id_Empresa,
                                 b.nombre_empresa,
                                 a.id_Delegacion,
                                 c.nombre_delegacion,
                                 a.codigo_OTContable,
                                 a.descripcion_OTContable,
                                 a.tipo_OTContable,
                                 a.fechaApertura_OTContable,
                                 a.fechaBaja_OTContable,
                                 a.estado,
                                 a.usuario_creacion,
                                 a.fecha_creacion,
                                 a.usuario_edicion,
                                 a.fecha_edicion
 
                             }).ToList();

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

 

        // PUT: api/tblGes_OT_Contable/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_Ges_OT_Contable(int id, tbl_Ges_OT_Contable tbl_Ges_OT_Contable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_Ges_OT_Contable.id_OtContable)
            {
                return BadRequest();
            }

            tbl_Ges_OT_Contable.fecha_creacion = DateTime.Now;
            db.tbl_Ges_OT_Contable.Add(tbl_Ges_OT_Contable);
            db.Entry(tbl_Ges_OT_Contable).State = EntityState.Modified;
            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_Ges_OT_ContableExists(id))
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

        // POST: api/tblGes_OT_Contable
        [ResponseType(typeof(tbl_Ges_OT_Contable))]
        public IHttpActionResult Posttbl_Ges_OT_Contable(tbl_Ges_OT_Contable tbl_Ges_OT_Contable)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            tbl_Ges_OT_Contable.fecha_creacion = DateTime.Now;
            db.tbl_Ges_OT_Contable.Add(tbl_Ges_OT_Contable);
            db.SaveChanges();


            return CreatedAtRoute("DefaultApi", new { id = tbl_Ges_OT_Contable.id_OtContable }, tbl_Ges_OT_Contable);
        }

        // DELETE: api/tblGes_OT_Contable/5
        [ResponseType(typeof(tbl_Ges_OT_Contable))]
        public async Task<IHttpActionResult> Deletetbl_Ges_OT_Contable(int id)
        {
          

            tbl_Ges_OT_Contable tbl_ocn = await db.tbl_Ges_OT_Contable.FindAsync(id);
            tbl_ocn = db.tbl_Ges_OT_Contable.Where(g => g.id_OtContable == id).FirstOrDefault<tbl_Ges_OT_Contable>();
            tbl_ocn.estado = 0;
            db.Entry(tbl_ocn).State = System.Data.Entity.EntityState.Modified;
            await db.SaveChangesAsync();
            return Ok("ok");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_Ges_OT_ContableExists(int id)
        {
            return db.tbl_Ges_OT_Contable.Count(e => e.id_OtContable == id) > 0;
        }
    }
}