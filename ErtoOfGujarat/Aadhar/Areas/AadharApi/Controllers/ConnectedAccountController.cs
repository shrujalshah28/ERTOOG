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
    public class ConnectedAccountController : ApiController
    {
        private AadharDBEntities db = new AadharDBEntities();

        // GET: api/ConnectedAccount
        public IQueryable<ConnectedAccountMaster> GetConnectedAccountMasters()
        {
            return db.ConnectedAccountMasters;
        }

        // GET: api/ConnectedAccount/5
        [ResponseType(typeof(ConnectedAccountMaster))]
        public IHttpActionResult GetConnectedAccountMaster(int id)
        {
            ConnectedAccountMaster connectedAccountMaster = db.ConnectedAccountMasters.Find(id);
            if (connectedAccountMaster == null)
            {
                return NotFound();
            }

            return Ok(connectedAccountMaster);
        }

        // PUT: api/ConnectedAccount/5
        [ResponseType(typeof(void))]
        public IHttpActionResult PutConnectedAccountMaster(int id, ConnectedAccountMaster connectedAccountMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != connectedAccountMaster.id)
            {
                return BadRequest();
            }

            db.Entry(connectedAccountMaster).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConnectedAccountMasterExists(id))
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

        // POST: api/ConnectedAccount
        [ResponseType(typeof(ConnectedAccountMaster))]
        public IHttpActionResult PostConnectedAccountMaster(ConnectedAccountMaster connectedAccountMaster)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.ConnectedAccountMasters.Add(connectedAccountMaster);

            try
            {
                db.SaveChanges();
            }
            catch (DbUpdateException)
            {
                if (ConnectedAccountMasterExists(connectedAccountMaster.id))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtRoute("DefaultApi", new { id = connectedAccountMaster.id }, connectedAccountMaster);
        }

        // DELETE: api/ConnectedAccount/5
        [ResponseType(typeof(ConnectedAccountMaster))]
        public IHttpActionResult DeleteConnectedAccountMaster(int id)
        {
            ConnectedAccountMaster connectedAccountMaster = db.ConnectedAccountMasters.Find(id);
            if (connectedAccountMaster == null)
            {
                return NotFound();
            }

            db.ConnectedAccountMasters.Remove(connectedAccountMaster);
            db.SaveChanges();

            return Ok(connectedAccountMaster);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool ConnectedAccountMasterExists(int id)
        {
            return db.ConnectedAccountMasters.Count(e => e.id == id) > 0;
        }
    }
}