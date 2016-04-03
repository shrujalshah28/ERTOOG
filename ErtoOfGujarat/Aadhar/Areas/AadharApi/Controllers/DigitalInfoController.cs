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
    public class DigitalInfoController : ApiController
    {
        private AadharDBEntities db = new AadharDBEntities();

        // GET: api/DigitalInfo
        public IQueryable<DigitalInfoMaster> GetDigitalInfoMasters()
        {
            return db.DigitalInfoMasters;
        }

        // GET: api/DigitalInfo/5
        [ResponseType(typeof(DigitalInfoMaster))]
        public IHttpActionResult GetDigitalInfoMaster(int id)
        {
            DigitalInfoMaster digitalInfoMaster = db.DigitalInfoMasters.Find(id);
            if (digitalInfoMaster == null)
            {
                return NotFound();
            }

            return Ok(digitalInfoMaster);
        }

        // PUT: api/DigitalInfo/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutDigitalInfoMaster(int id, DigitalInfoMaster digitalInfoMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != digitalInfoMaster.id)
            {
                return BadRequest();
            }

            db.Entry(digitalInfoMaster).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DigitalInfoMasterExists(id))
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

        // POST: api/DigitalInfo
        [ResponseType(typeof(DigitalInfoMaster))]
        public IHttpActionResult PostDigitalInfoMaster(DigitalInfoMaster digitalInfoMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.DigitalInfoMasters.Add(digitalInfoMaster);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (DigitalInfoMasterExists(digitalInfoMaster.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = digitalInfoMaster.id }, digitalInfoMaster);
        }

        // DELETE: api/DigitalInfo/5
        [ResponseType(typeof(DigitalInfoMaster))]
        public IHttpActionResult DeleteDigitalInfoMaster(int id)
        {
            DigitalInfoMaster digitalInfoMaster = db.DigitalInfoMasters.Find(id);
            if (digitalInfoMaster == null)
            {
                return NotFound();
            }

            db.DigitalInfoMasters.Remove(digitalInfoMaster);
            db.SaveChanges();

            return Ok(digitalInfoMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool DigitalInfoMasterExists(int id)
        {
            return db.DigitalInfoMasters.Count(e => e.id == id) > 0;
        }
    }
}