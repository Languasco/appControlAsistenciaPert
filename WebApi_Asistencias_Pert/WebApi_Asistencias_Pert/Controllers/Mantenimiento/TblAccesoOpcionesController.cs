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
using System.Web.Http.Cors;
using CapaDatos;
 

namespace webApiFacturacion.Controllers.Mantenimientos
{
    [EnableCors("*", "*", "*")]
    public class TblAccesoOpcionesController : ApiController
    {
        private CAM_ControlAsistenciaEntities db = new CAM_ControlAsistenciaEntities();
 
        // GET: api/TblAccesoOpciones
        public object Gettbl_AccesoOpciones()
        {
          
            db.Configuration.ProxyCreationEnabled = false;
    
            var list = (from a in db.tbl_Personal
                        join b in db.Tbl_Cargo_Personal on a.id_cargo_personal equals b.id_Cargo
                        select new
                        {
                            Codigo = a.id_personal,
                            Nombre = a.nombres_personal,
                            Usuario = a.nombreUsario_personal,
                            cargo = b.nombre_cargo,
                            estado = a.estado
                        }).ToList();

            // list;

            return list;
         
        }

        // GET: api/TblAccesoOpciones/5
        [ResponseType(typeof(tbl_AccesoOpciones))]
        public IHttpActionResult Gettbl_AccesoOpciones(int id, int opcion, int condicion)
        {
            db.Configuration.ProxyCreationEnabled = false;
            var list = new object();
            if (condicion == 1)
            {
                list = (from a in db.tbl_AccesoOpciones
                        where a.id_personal == id && a.id_opcion == opcion
                        select new
                        {
                            id_RegistroAcceso = a.id_RegistroAcceso,
                            id_personal = a.id_personal,
                            id_opcion = a.id_opcion,
                            permisos_opciones = a.permisos_opciones,
                            estado = a.estado

                        }).ToList();
            }
            else if (condicion == 2)
            {
                list = (from a in db.tbl_AccesoOpciones
                        join b in db.tbl_Definicion_Opciones on a.id_opcion equals b.id_opcion
                        join c in db.tbl_Personal on a.id_personal equals c.id_personal
                        join d in db.Tbl_Cargo_Personal on c.id_cargo_personal equals d.id_Cargo
                        where a.id_opcion == opcion
                        select new
                        {
                            nombre_opcion = b.nombre_opcion,
                            id_RegistroAcceso = a.id_RegistroAcceso,
                            id_personal = a.id_personal,
                            id_opcion = a.id_opcion,
                            permisos_opciones = a.permisos_opciones,
                            estado = a.estado

                        }).ToList();
            }
            else
            {
                list = (from acc in db.tbl_AccesoOpciones
                        join def in db.tbl_Definicion_Opciones on acc.id_opcion equals def.id_opcion
                        where acc.id_personal == id && def.estado==1
                        orderby def.orden_Opcion ascending 
                        select new
                        {
                            acc.id_personal,
                            acc.id_opcion,
                            def.nombre_opcion,
                            def.nombreParentID,
                            def.parentID,
                            def.url_opcion,
                            def.orden_Opcion

                        }).ToList();
            }



            return Ok(list);
        }



        // PUT: api/TblAccesoOpciones/5
        [ResponseType(typeof(void))]
        public IHttpActionResult Puttbl_AccesoOpciones(int id, tbl_AccesoOpciones obj_AccesoOpciones)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != obj_AccesoOpciones.id_RegistroAcceso)
            {
                return BadRequest();
            }

            tbl_AccesoOpciones Ent_AccesoOpcionesR;
            // DATA ACTUAL
            Ent_AccesoOpcionesR = db.tbl_AccesoOpciones.Where(g => g.id_RegistroAcceso == obj_AccesoOpciones.id_RegistroAcceso).FirstOrDefault<tbl_AccesoOpciones>();

            Ent_AccesoOpcionesR.id_personal = obj_AccesoOpciones.id_personal;
            Ent_AccesoOpcionesR.id_opcion = obj_AccesoOpciones.id_opcion;
            Ent_AccesoOpcionesR.permisos_opciones = obj_AccesoOpciones.permisos_opciones;
            Ent_AccesoOpcionesR.estado = obj_AccesoOpciones.estado;
            Ent_AccesoOpcionesR.usuario_creacion = obj_AccesoOpciones.usuario_creacion;
            Ent_AccesoOpcionesR.fecha_creacion = obj_AccesoOpciones.fecha_creacion;
            Ent_AccesoOpcionesR.fecha_edicion = DateTime.Now;
            db.Entry(Ent_AccesoOpcionesR).State = EntityState.Modified;


            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tbl_AccesoOpcionesExists(id))
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

        // POST: api/TblAccesoOpciones
        [ResponseType(typeof(tbl_AccesoOpciones))]
        public IHttpActionResult Posttbl_AccesoOpciones(tbl_AccesoOpciones obj_AccesoOpciones)
        {

            tbl_AccesoOpciones Ent_AccesoOpcionesR;
            // DATA ACTUAL
            Ent_AccesoOpcionesR = db.tbl_AccesoOpciones.Where(g => g.id_personal == obj_AccesoOpciones.id_personal && g.id_opcion == obj_AccesoOpciones.id_opcion).FirstOrDefault<tbl_AccesoOpciones>();

            if (Ent_AccesoOpcionesR == null)
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                obj_AccesoOpciones.fecha_creacion = DateTime.Now;
                db.tbl_AccesoOpciones.Add(obj_AccesoOpciones);
                db.SaveChanges();

                return CreatedAtRoute("DefaultApi", new { id = obj_AccesoOpciones.id_RegistroAcceso }, obj_AccesoOpciones);

            }
            else
            {

                // DATA ACTUAL

                Ent_AccesoOpcionesR.id_personal = Ent_AccesoOpcionesR.id_personal;
                Ent_AccesoOpcionesR.id_opcion = Ent_AccesoOpcionesR.id_opcion;
                Ent_AccesoOpcionesR.permisos_opciones = obj_AccesoOpciones.permisos_opciones;
                Ent_AccesoOpcionesR.estado = Ent_AccesoOpcionesR.estado;
                Ent_AccesoOpcionesR.usuario_creacion = Ent_AccesoOpcionesR.usuario_creacion;
                Ent_AccesoOpcionesR.fecha_creacion = Ent_AccesoOpcionesR.fecha_creacion;
                Ent_AccesoOpcionesR.fecha_edicion = DateTime.Now;
                db.Entry(Ent_AccesoOpcionesR).State = EntityState.Modified;




                db.SaveChanges();

                return StatusCode(HttpStatusCode.NoContent);

            }
        }



        // DELETE: api/TblAccesoOpciones/5
        [ResponseType(typeof(tbl_AccesoOpciones))]
        public IHttpActionResult Deletetbl_AccesoOpciones(int id)
        {
            tbl_AccesoOpciones tbl_AccesoOpciones = db.tbl_AccesoOpciones.Find(id);
            if (tbl_AccesoOpciones == null)
            {
                return NotFound();
            }

            //db.tbl_AccesoOpciones.Remove(tbl_AccesoOpciones);
            //db.SaveChanges();


            tbl_AccesoOpciones ent_accesoopciones;
            ent_accesoopciones = db.tbl_AccesoOpciones.Where(g => g.id_RegistroAcceso == id).FirstOrDefault<tbl_AccesoOpciones>();
            ent_accesoopciones.estado = 0;
            db.Entry(ent_accesoopciones).State = System.Data.Entity.EntityState.Modified;

            db.Configuration.ProxyCreationEnabled = false;

            return Ok(tbl_AccesoOpciones);

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool tbl_AccesoOpcionesExists(int id)
        {
            return db.tbl_AccesoOpciones.Count(e => e.id_RegistroAcceso == id) > 0;
        }
    }
}