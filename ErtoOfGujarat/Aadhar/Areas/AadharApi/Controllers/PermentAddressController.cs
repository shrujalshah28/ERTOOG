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
    public class PermentAddressController : ApiController
    {
        private AadharDBEntities db = new AadharDBEntities();

        // GET: api/PermentAddress
        public IQueryable<PermentAddressMaster> GetPermentAddressMasters()
        {
            return db.PermentAddressMasters;
        }

        // GET: api/PermentAddress/5
        [ResponseType(typeof(PermentAddressMaster))]
        public IHttpActionResult GetPermentAddressMaster(int id)
        {
            PermentAddressMaster permentAddressMaster = db.PermentAddressMasters.First(a=>a.id==id);
            if (permentAddressMaster == null)
            {
                return NotFound();
            }

            return Ok(permentAddressMaster);
        }

        // PUT: api/PermentAddress/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPermentAddressMaster(int id, PermentAddressMaster permentAddressMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != permentAddressMaster.id)
            {
                return BadRequest();
            }

            db.Entry(permentAddressMaster).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PermentAddressMasterExists(id))
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

        // POST: api/PermentAddress
        [ResponseType(typeof(PermentAddressMaster))]
        public IHttpActionResult PostPermentAddressMaster(PermentAddressMaster permentAddressMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PermentAddressMasters.Add(permentAddressMaster);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PermentAddressMasterExists(permentAddressMaster.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = permentAddressMaster.id }, permentAddressMaster);
        }

        // DELETE: api/PermentAddress/5
        [ResponseType(typeof(PermentAddressMaster))]
        public IHttpActionResult DeletePermentAddressMaster(int id)
        {
            PermentAddressMaster permentAddressMaster = db.PermentAddressMasters.Find(id);
            if (permentAddressMaster == null)
            {
                return NotFound();
            }

            db.PermentAddressMasters.Remove(permentAddressMaster);
            db.SaveChanges();

            return Ok(permentAddressMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PermentAddressMasterExists(int id)
        {
            return db.PermentAddressMasters.Count(e => e.id == id) > 0;
        }
    }
}