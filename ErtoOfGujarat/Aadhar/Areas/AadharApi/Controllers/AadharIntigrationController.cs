using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using Aadhar;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace Aadhar.Areas.AadharApi.Controllers
{


    //[RoutePrefix("~/api/{area}/AadharIntigration")]
    public class AadharIntigrationController : ApiController
    {
        private AadharDBEntities db = new AadharDBEntities();

        [System.Web.Http.HttpGet]
        //[Route("Request/{id}")]
        //[System.Web.Http.ActionName("request")]
        public IHttpActionResult GetRequest(decimal aadharNo,string eUid,string type)
        {
            //int id = Convert.ToInt32(db.AadharMasters.Where(a => a.aadharNo == aadharNo).Select(a => a.id).ToString());
            //int id = Convert.ToInt32(id1);
            //Getting id from aadhar number
            var data = from asd in db.AadharMasters where asd.aadharNo == aadharNo select new { asd.id };
            int id = 0;
            foreach (var item in data)
            {
               id = item.id;
            }
            IntigrationMaster im = new IntigrationMaster() { id = id, aadharNo = aadharNo, externalUniqueId = eUid, clientType = type, requestDateTime = DateTime.Now };
            db.IntigrationMasters.Add(im);
            db.SaveChanges();
            int newRequestID = im.requestId;

            var addedRid = from asd in db.IntigrationMasters where asd.id == id orderby asd.id ascending group asd by asd.id ;
            int rid = 0;
            foreach (var item in addedRid)
            {
                foreach (var li in item)
                {
                    rid = li.requestId;
                }
            }

            OTPMaster om = new OTPMaster() { requestId = rid, mOTP = 1234, eOTP = 123456 };
            db.OTPMasters.Add(om);
            db.SaveChanges();

            var cinfo = from asd in db.ContectMasters where asd.id == id && asd.isPrimary == true select asd;
            decimal pno = 0;
            string email = null;
            foreach (var item in cinfo)
            {
                pno = Convert.ToDecimal(item.phoneNumber);
                email = item.emailId;
            }
            var result = new { requestId = rid, phoneNo = pno, emailId = email };

            return Ok(result);
        }
    }
}
