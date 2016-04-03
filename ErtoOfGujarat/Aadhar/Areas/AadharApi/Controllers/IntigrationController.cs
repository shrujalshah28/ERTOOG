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
    public class IntigrationController : ApiController
    {
        private AadharDBEntities db = new AadharDBEntities();

        // GET: api/Intigration
        public IQueryable<IntigrationMaster> GetIntigrationMasters()
        {
            return db.IntigrationMasters;
        }

        // GET: api/Intigration/5
        [ResponseType(typeof(IntigrationMaster))]
        public IHttpActionResult GetIntigrationMaster(int id)
        {
            IntigrationMaster intigrationMaster = db.IntigrationMasters.Find(id);
            if (intigrationMaster == null)
            {
                return NotFound();
            }

            return Ok(intigrationMaster);
        }

        // PUT: api/Intigration/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutIntigrationMaster(int id, IntigrationMaster intigrationMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != intigrationMaster.requestId)
            {
                return BadRequest();
            }

            db.Entry(intigrationMaster).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!IntigrationMasterExists(id))
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

        // POST: api/Intigration
        [ResponseType(typeof(IntigrationMaster))]
        public IHttpActionResult PostIntigrationMaster(IntigrationMaster intigrationMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.IntigrationMasters.Add(intigrationMaster);
            db.SaveChanges();

            return CreatedAtRoute("DefaultApi", new { id = intigrationMaster.requestId }, intigrationMaster);
        }

        // DELETE: api/Intigration/5
        [ResponseType(typeof(IntigrationMaster))]
        public IHttpActionResult DeleteIntigrationMaster(int id)
        {
            IntigrationMaster intigrationMaster = db.IntigrationMasters.Find(id);
            if (intigrationMaster == null)
            {
                return NotFound();
            }

            db.IntigrationMasters.Remove(intigrationMaster);
            db.SaveChanges();

            return Ok(intigrationMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool IntigrationMasterExists(int id)
        {
            return db.IntigrationMasters.Count(e => e.requestId == id) > 0;
        }
    }
}