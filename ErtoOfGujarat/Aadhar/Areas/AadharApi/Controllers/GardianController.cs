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
using Aadhar;

namespace Aadhar.Areas.AadharApi.Controllers
{
    public class GardianController : ApiController
    {
        private AadharDBEntities db = new AadharDBEntities();

        // GET: api/Gardian
        public IQueryable<GardianMaster> GetGardianMasters()
        {
            return db.GardianMasters;
        }

        // GET: api/Gardian/5
        [ResponseType(typeof(GardianMaster))]
        public IHttpActionResult GetGardianMaster(int id)
        {
            //GardianMaster gardianMaster = db.GardianMasters.Find(id);
            GardianMaster gardianMaster = db.GardianMasters.First(a => a.id == id);
            if (gardianMaster == null)
            {
                return NotFound();
            }

            return Ok(gardianMaster);
        }

        // GET: /api/AadharApi/Gardian/1?gtype=2
        [ResponseType(typeof(GardianMaster))]
        public IHttpActionResult GetGardianMaster(int id,int gtype)
        {
            //GardianMaster gardianMaster = db.GardianMasters.Find(id);
            GardianMaster gardianMaster = db.GardianMasters.First(a => a.id == id && a.typeGardian == gtype);
            if (gardianMaster == null)
            {
                return NotFound();
            }

            return Ok(gardianMaster);
        }

        // PUT: api/Gardian/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutGardianMaster(int id, GardianMaster gardianMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != gardianMaster.id)
            {
                return BadRequest();
            }

            db.Entry(gardianMaster).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GardianMasterExists(id))
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

        // POST: api/Gardian
        [ResponseType(typeof(GardianMaster))]
        public IHttpActionResult PostGardianMaster(GardianMaster gardianMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.GardianMasters.Add(gardianMaster);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (GardianMasterExists(gardianMaster.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = gardianMaster.id }, gardianMaster);
        }

        // DELETE: api/Gardian/5
        [ResponseType(typeof(GardianMaster))]
        public IHttpActionResult DeleteGardianMaster(int id)
        {
            GardianMaster gardianMaster = db.GardianMasters.Find(id);
            if (gardianMaster == null)
            {
                return NotFound();
            }

            db.GardianMasters.Remove(gardianMaster);
            db.SaveChanges();

            return Ok(gardianMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool GardianMasterExists(int id)
        {
            return db.GardianMasters.Count(e => e.id == id) > 0;
        }
    }
}