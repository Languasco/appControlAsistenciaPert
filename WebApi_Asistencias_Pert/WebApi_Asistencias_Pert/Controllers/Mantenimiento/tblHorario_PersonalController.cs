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
using CapaNegocios.Proceso;

namespace WebApi_Asistencias_Pert.Controllers.Mantenimiento
{
     [EnableCors("*", "*", "*")]
    public class tblHorario_PersonalController : ApiController
    {
        private CAM_ControlAsistenciaEntities db = new CAM_ControlAsistenciaEntities();

        // GET: api/tblHorario_Personal
        public IQueryable<tbl_Horario_Personal> Gettbl_Horario_Personal()
        {
            db.Configuration.ProxyCreationEnabled = false;
            return db.tbl_Horario_Personal;
        }


        public object GetHorarioPersonal(int opcion, string filtro)
        {
            //filtro puede tomar cualquier valor
            db.Configuration.ProxyCreationEnabled = false;
            object resul = null;

            try
            {
                if (opcion == 1)
                {
                    resul = (from a in db.tbl_Horario_Personal
                             join b in db.tbl_Personal on a.id_personal equals b.id_personal 
                             select new
                             {
                                 a.id_personal ,
                                 b.apellidos_personal ,
                                 b.nombres_personal ,
                                 a.lunes_I ,
                                 a.lunes_S,
                                 a.Martes_I,
                                 a.Martes_S,
                                 a.Miercoles_I,
                                 a.Miercoles_S,
                                 a.Jueves_I,
                                 a.Jueves_S,
                                 a.Viernes_I,
                                 a.Viernes_S,
                                 a.Sabado_I,
                                 a.Sabado_S,
                                 a.Domingo_I,
                                 a.Domingo_S   
                             }).ToList();
                }
                else if (opcion == 2)
                {
                    string[] parametros = filtro.Split('|');

                    int  id_personal = Convert.ToInt32(parametros[0].ToString());
                    string lunes_I = parametros[1].ToString();
                    string lunes_S = parametros[2].ToString();
                    string Martes_I = parametros[3].ToString();
                    string Martes_S = parametros[4].ToString();
                    string Miercoles_I = parametros[5].ToString();
                    string Miercoles_S = parametros[6].ToString();
                    string Jueves_I = parametros[7].ToString();
                    string Jueves_S = parametros[8].ToString();
                    string Viernes_I = parametros[9].ToString();
                    string Viernes_S = parametros[10].ToString();
                    string Sabado_I = parametros[11].ToString();
                    string Sabado_S = parametros[12].ToString();
                    string Domingo_I = parametros[13].ToString();
                    string Domingo_S = parametros[14].ToString();


                    HorarioPersonal_BL obj_negocio = new HorarioPersonal_BL();
                    resul = obj_negocio.Insert_Horario_Personal(id_personal, lunes_I , lunes_S , Martes_I , Martes_S , Miercoles_I, Miercoles_S , Jueves_I , Jueves_S , Viernes_I, Viernes_S, Sabado_I, Sabado_S , Domingo_I, Domingo_S);

                }
                else if (opcion == 3)
                {
                    string[] parametros = filtro.Split('|');

                    int  id_personal = Convert.ToInt32(parametros[0].ToString());
                    string lunes_I = parametros[1].ToString();
                    string lunes_S = parametros[2].ToString();
                    string Martes_I = parametros[3].ToString();
                    string Martes_S = parametros[4].ToString();
                    string Miercoles_I = parametros[5].ToString();
                    string Miercoles_S = parametros[6].ToString();
                    string Jueves_I = parametros[7].ToString();
                    string Jueves_S = parametros[8].ToString();
                    string Viernes_I = parametros[9].ToString();
                    string Viernes_S = parametros[10].ToString();
                    string Sabado_I = parametros[11].ToString();
                    string Sabado_S = parametros[12].ToString();
                    string Domingo_I = parametros[13].ToString();
                    string Domingo_S = parametros[14].ToString();


                    HorarioPersonal_BL obj_negocio = new HorarioPersonal_BL();
                    resul = obj_negocio.Update_Horario_Personal(id_personal, lunes_I, lunes_S, Martes_I, Martes_S, Miercoles_I, Miercoles_S, Jueves_I, Jueves_S, Viernes_I, Viernes_S, Sabado_I, Sabado_S, Domingo_I, Domingo_S);

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




        // PUT: api/tblHorario_Personal/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_Horario_Personal(int id, tbl_Horario_Personal tbl_Horario_Personal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != tbl_Horario_Personal.id_personal)
            {
                return BadRequest();
            }

            db.Entry(tbl_Horario_Personal).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_Horario_PersonalExists(id))
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

        // POST: api/tblHorario_Personal
        [ResponseType(typeof(tbl_Horario_Personal))]
        public IHttpActionResult Posttbl_Horario_Personal(tbl_Horario_Personal tbl_Horario_Personal)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.tbl_Horario_Personal.Add(tbl_Horario_Personal);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (tbl_Horario_PersonalExists(tbl_Horario_Personal.id_personal))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = tbl_Horario_Personal.id_personal }, tbl_Horario_Personal);
        }

        // DELETE: api/tblHorario_Personal/5
        [ResponseType(typeof(tbl_Horario_Personal))]
        public IHttpActionResult Deletetbl_Horario_Personal(int id)
        {
            tbl_Horario_Personal tbl_Horario_Personal = db.tbl_Horario_Personal.Find(id);
            if (tbl_Horario_Personal == null)
            {
                return NotFound();
            }

            db.tbl_Horario_Personal.Remove(tbl_Horario_Personal);
            db.SaveChanges();

            return Ok(tbl_Horario_Personal);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_Horario_PersonalExists(int id)
        {
            return db.tbl_Horario_Personal.Count(e => e.id_personal == id) > 0;
        }
    }
}