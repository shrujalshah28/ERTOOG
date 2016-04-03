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
    public class OTPController : ApiController
    {
        private AadharDBEntities db = new AadharDBEntities();

        // GET: api/OTP
        public IQueryable<OTPMaster> GetOTPMasters()
        {
            return db.OTPMasters;
        }

        // GET: api/OTP/5
        [ResponseType(typeof(OTPMaster))]
        public IHttpActionResult GetOTPMaster(int id)
        {
            OTPMaster oTPMaster = db.OTPMasters.Find(id);
            if (oTPMaster == null)
            {
                return NotFound();
            }

            return Ok(oTPMaster);
        }

        // PUT: api/OTP/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutOTPMaster(int id, OTPMaster oTPMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != oTPMaster.requestId)
            {
                return BadRequest();
            }

            db.Entry(oTPMaster).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OTPMasterExists(id))
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

        // POST: api/OTP
        [ResponseType(typeof(OTPMaster))]
        public IHttpActionResult PostOTPMaster(OTPMaster oTPMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.OTPMasters.Add(oTPMaster);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (OTPMasterExists(oTPMaster.requestId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = oTPMaster.requestId }, oTPMaster);
        }

        // DELETE: api/OTP/5
        [ResponseType(typeof(OTPMaster))]
        public IHttpActionResult DeleteOTPMaster(int id)
        {
            OTPMaster oTPMaster = db.OTPMasters.Find(id);
            if (oTPMaster == null)
            {
                return NotFound();
            }

            db.OTPMasters.Remove(oTPMaster);
            db.SaveChanges();

            return Ok(oTPMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OTPMasterExists(int id)
        {
            return db.OTPMasters.Count(e => e.requestId == id) > 0;
        }
    }
}