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
    public class AadharController : ApiController
    {
        private AadharDBEntities db = new AadharDBEntities();

        // GET: api/Aadhar
        public IQueryable<AadharMaster> GetAadharMasters()
        {
            return db.AadharMasters;
        }

        // GET: api/Aadhar/5
        [ResponseType(typeof(AadharMaster))]
        public IHttpActionResult GetAadharMaster(int id)
        {
            AadharMaster aadharMaster = db.AadharMasters.Find(id);
            if (aadharMaster == null)
            {
                return NotFound();
            }

            return Ok(aadharMaster);
        }

        // PUT: api/Aadhar/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutAadharMaster(int id, AadharMaster aadharMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != aadharMaster.id)
            {
                return BadRequest();
            }

            db.Entry(aadharMaster).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AadharMasterExists(id))
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

        // POST: api/Aadhar
        [ResponseType(typeof(AadharMaster))]
        public IHttpActionResult PostAadharMaster(AadharMaster aadharMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.AadharMasters.Add(aadharMaster);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = aadharMaster.id }, aadharMaster);
        }

        // DELETE: api/Aadhar/5
        [ResponseType(typeof(AadharMaster))]
        public IHttpActionResult DeleteAadharMaster(int id)
        {
            AadharMaster aadharMaster = db.AadharMasters.Find(id);
            if (aadharMaster == null)
            {
                return NotFound();
            }

            db.AadharMasters.Remove(aadharMaster);
            db.SaveChanges();

            return Ok(aadharMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool AadharMasterExists(int id)
        {
            return db.AadharMasters.Count(e => e.id == id) > 0;
        }
    }
}