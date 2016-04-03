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
    public class ContectController : ApiController
    {
        private AadharDBEntities db = new AadharDBEntities();

        // GET: api/Contect
        public IQueryable<ContectMaster> GetContectMasters()
        {
            return db.ContectMasters;
        }

        // GET: api/Contect/5
        [ResponseType(typeof(ContectMaster))]
        public IHttpActionResult GetContectMaster(int id)
        {
            //ContectMaster contectMaster = db.ContectMasters.First(a => a.isPrimary == false);
            ContectMaster contectMaster = db.ContectMasters.First(a => a.id == id);
            //ContectMaster contectMaster = db.ContectMasters.Find(id);
            if (contectMaster == null)
            {
                return NotFound();
            }

            return Ok(contectMaster);
        }

        // PUT: api/Contect/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutContectMaster(int id, ContectMaster contectMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contectMaster.id)
            {
                return BadRequest();
            }

            db.Entry(contectMaster).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContectMasterExists(id))
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

        // POST: api/Contect
        [ResponseType(typeof(ContectMaster))]
        public IHttpActionResult PostContectMaster(ContectMaster contectMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ContectMasters.Add(contectMaster);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ContectMasterExists(contectMaster.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = contectMaster.id }, contectMaster);
        }

        // DELETE: api/Contect/5
        [ResponseType(typeof(ContectMaster))]
        public IHttpActionResult DeleteContectMaster(int id)
        {
            ContectMaster contectMaster = db.ContectMasters.Find(id);
            if (contectMaster == null)
            {
                return NotFound();
            }

            db.ContectMasters.Remove(contectMaster);
            db.SaveChanges();

            return Ok(contectMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ContectMasterExists(int id)
        {
            return db.ContectMasters.Count(e => e.id == id) > 0;
        }
    }
}