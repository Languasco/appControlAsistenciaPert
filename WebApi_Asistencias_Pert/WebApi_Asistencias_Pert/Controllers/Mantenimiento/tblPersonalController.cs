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
    public class tblPersonalController : ApiController
    {
        private CAM_ControlAsistenciaEntities db = new CAM_ControlAsistenciaEntities();

        // GET: api/tblPersonal
        public object Gettbl_Personal()
        {

            object listPersonal = (from per in db.tbl_Personal
                                   select new
                                   {
                                       per.id_personal,
                                       per.codigo_personal,
                                       per.id_OtContable,
                                       per.nroDoc_personal,
                                       per.tipoDoc_personal,
                                       per.apellidos_personal,
                                       per.nombres_personal,
                                       per.tip_personal,
                                       per.id_cargo_personal,
                                       per.fotoUrl_personal,
                                       per.nroCelular_personal,
                                       per.email_personal,
                                       per.nombreUsario_personal,
                                       per.contrasenia_personal,
                                       per.envio_enlinea_personal,
                                       per.id_perfil,
                                       per.fecha_cese,
                                       per.estado,
                                       per.usuario_creacion,
                                       per.fecha_creacion,
                                       per.usuario_edicion,
                                       per.fecha_edicion
                                   }).ToList();

            return listPersonal;
            //return db.tbl_Personal.OrderBy(c => c.apellidos_personal);
        }

        // GET: api/tblPersonal/5
        [ResponseType(typeof(tbl_Personal))]
        public IHttpActionResult Gettbl_Personal(int id)
        {
            tbl_Personal tbl_Personal = db.tbl_Personal.Find(id);
            if (tbl_Personal == null)
            {
                return NotFound();
            }

            return Ok(tbl_Personal);
        }

        // PUT: api/tblPersonal/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_Personal(int id, tbl_Personal tbl_Personal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_Personal.id_personal)
            {
                return BadRequest();
            }


            tbl_Personal.fecha_edicion = DateTime.Now;
            tbl_Personal.usuario_edicion = tbl_Personal.usuario_creacion;
            db.Entry(tbl_Personal).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_PersonalExists(id))
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

        // POST: api/tblPersonal
        [ResponseType(typeof(tbl_Personal))]
        public IHttpActionResult Posttbl_Personal(tbl_Personal tbl_Personal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            tbl_Personal.fecha_creacion = DateTime.Now;
            db.tbl_Personal.Add(tbl_Personal);
            db.SaveChanges();

            //Grabando datos en la Base de Datos de Asistencias
            int  id_personal_res = tbl_Personal.id_personal;
            Personal_BL obj_negocio= new Personal_BL();
            obj_negocio.Insert_BD_Asistencia(id_personal_res);
            
            return CreatedAtRoute("DefaultApi", new { id = tbl_Personal.id_personal }, tbl_Personal);
        }

 
        // DELETE: api/TblPersonal/5
        [ResponseType(typeof(tbl_Personal))]
        public async Task<IHttpActionResult> Deletetbl_Personal(int id)
        {

            tbl_Personal tblPersonal = await db.tbl_Personal.FindAsync(id);
            tblPersonal = db.tbl_Personal.Where(g => g.id_personal == id).FirstOrDefault<tbl_Personal>();
            tblPersonal.estado = 0;
            db.Entry(tblPersonal).State = System.Data.Entity.EntityState.Modified;
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

        private bool tbl_PersonalExists(int id)
        {
            return db.tbl_Personal.Count(e => e.id_personal == id) > 0;
        }
    }
}