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
    public class PersentAddressController : ApiController
    {
        private AadharDBEntities db = new AadharDBEntities();

        // GET: api/PersentAddress
        public IQueryable<PersentAddressMaster> GetPersentAddressMasters()
        {
            return db.PersentAddressMasters;
        }

        // GET: api/PersentAddress/5
        [ResponseType(typeof(PersentAddressMaster))]
        public IHttpActionResult GetPersentAddressMaster(int id)
        {
            PersentAddressMaster persentAddressMaster = db.PersentAddressMasters.Find(id);
            if (persentAddressMaster == null)
            {
                return NotFound();
            }

            return Ok(persentAddressMaster);
        }

        // PUT: api/PersentAddress/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutPersentAddressMaster(int id, PersentAddressMaster persentAddressMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != persentAddressMaster.id)
            {
                return BadRequest();
            }

            db.Entry(persentAddressMaster).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersentAddressMasterExists(id))
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

        // POST: api/PersentAddress
        [ResponseType(typeof(PersentAddressMaster))]
        public IHttpActionResult PostPersentAddressMaster(PersentAddressMaster persentAddressMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.PersentAddressMasters.Add(persentAddressMaster);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (PersentAddressMasterExists(persentAddressMaster.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = persentAddressMaster.id }, persentAddressMaster);
        }

        // DELETE: api/PersentAddress/5
        [ResponseType(typeof(PersentAddressMaster))]
        public IHttpActionResult DeletePersentAddressMaster(int id)
        {
            PersentAddressMaster persentAddressMaster = db.PersentAddressMasters.Find(id);
            if (persentAddressMaster == null)
            {
                return NotFound();
            }

            db.PersentAddressMasters.Remove(persentAddressMaster);
            db.SaveChanges();

            return Ok(persentAddressMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool PersentAddressMasterExists(int id)
        {
            return db.PersentAddressMasters.Count(e => e.id == id) > 0;
        }
    }
}