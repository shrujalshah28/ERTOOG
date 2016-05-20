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
using System.Net.Mail;

namespace Aadhar.Areas.AadharApi.Controllers
{
    public class AadharIntigrationController : ApiController
    {
        private AadharDBEntities db = new AadharDBEntities();

        [System.Web.Http.HttpGet]
        public IHttpActionResult GetRequest(decimal aadharNo, string eUid, string type)
        {
            //Getting id and birth day from aadhar number
            var data = from asd in db.AadharMasters where asd.aadharNo == aadharNo select new { asd.id, asd.dob };
            int id = 0;
            System.DateTime dob = System.DateTime.Now;
            foreach (var item in data)
            {
                id = item.id;
                dob = Convert.ToDateTime(item.dob);
            }

            //It can be improved.
            DateTime today = DateTime.Today;
            int age = today.Year - dob.Year;

            if (dob > today.AddYears(-age))
                age--;

            IntigrationMaster im = new IntigrationMaster() { id = id, aadharNo = aadharNo, externalUniqueId = eUid, clientType = type, requestDateTime = DateTime.Now };
            db.IntigrationMasters.Add(im);
            db.SaveChanges();
            int newRequestID = im.requestId;

            Random rnd = new Random();
            int motp = rnd.Next(1000, 9999);
            int eotp = rnd.Next(100000, 999999);

            OTPMaster om = new OTPMaster() { requestId = newRequestID, mOTP = motp, eOTP = eotp };
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
            var result = new { requestId = newRequestID, Age = age, phoneNo = pno, emailId = email };

            return Ok(result);
        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult ConfirmRequest(int rId, bool conformatiton)
        {
            var data = from asd in db.OTPMasters where asd.requestId == rId select asd;
            //var contectdata = from zxc in db.ContectMasters where zxc.id == Convert.ToInt32(from qwe in db.IntigrationMasters where qwe.requestId == rId select new { qwe.id }) && zxc.isPrimary == true select zxc;
            var contectdata = from zxc in db.ContectMasters join qwe in db.IntigrationMasters on zxc.id equals qwe.id where qwe.requestId == rId where zxc.isPrimary == true select zxc;
            decimal pno = 0;
            string email = null;
            foreach (var item in contectdata)
            {
                pno = Convert.ToDecimal(item.phoneNumber);
                email = item.emailId;
            }
            int motp = 0, eotp = 0;
            foreach (var item in data)
            {
                motp = Convert.ToInt32(item.mOTP);
                eotp = Convert.ToInt32(item.eOTP);
            }

            MailMessage _mailmessage = new MailMessage("shrujalshah28@gmail.com", email);
            _mailmessage.Subject = "Your OTP";
            _mailmessage.Body = @"<p> This is system generated Email.</p></br><table style=""width:20%""><tr><td> Mobile OTP </td><td> " + motp + "</td></tr><tr><td> Email OTP </td><td>" + eotp + "</td></tr></table></br><p> Call Shrujal and confirm your OTP.</p>";
            _mailmessage.IsBodyHtml = true;

            SmtpClient sc = new SmtpClient();
            sc.Send(_mailmessage);

            // Logic for send OTP to User.
            var result = new { requestId = rId, mobileOTP = motp, emailOTP = eotp };
            return Ok(result);
        }

        [System.Web.Http.HttpGet]
        public IHttpActionResult GetData(int rId, bool conformation)
        {
            int id = 0;
            string fname = null;
            string lname = null;
            int dbgender = 0;
            string gender = null;
            string dob = null;
            int dbbloodGroup = 0;
            string bloodGroup = null;
            bool dbisGardad = false;
            string orphan = null;

            var Aadhardata = from asd in db.AadharMasters join qwe in db.IntigrationMasters on asd.id equals qwe.id where qwe.requestId == rId select asd;

            foreach (var item in Aadhardata)
            {
                id = item.id;
                fname = item.firstName;
                lname = item.lastName;
                dbgender = Convert.ToInt32(item.gender);
                dob = Convert.ToDateTime(item.dob).ToString();
                dbbloodGroup = Convert.ToInt32(item.bloodGroup);
                dbisGardad = item.isGaurded;
            }
            switch (dbgender)
            {
                case 1:
                    gender = "Male";
                    break;
                case 2:
                    gender = "Female";
                    break;
                case 3:
                    gender = "Other";
                    break;
                default:
                    break;
            }

            switch (dbbloodGroup)
            {
                case 1:
                    bloodGroup = "A+";
                    break;
                case 2:
                    bloodGroup = "A-";
                    break;
                case 3:
                    bloodGroup = "B+";
                    break;
                case 4:
                    bloodGroup = "B-";
                    break;
                case 5:
                    bloodGroup = "AB+";
                    break;
                case 6:
                    bloodGroup = "AB-";
                    break;
                case 7:
                    bloodGroup = "O+";
                    break;
                case 8:
                    bloodGroup = "O-";
                    break;
                default:
                    break;
            }

            orphan = (dbisGardad) ? "Yes" : "No";

            // What if user have more than three number? //
            // Don't let user enter more than two numbers. //
            string email = null;
            string pn = null;

            var Contectdata = from qaz in db.ContectMasters where (qaz.id == id && qaz.isPrimary == false) select qaz;

            if (Contectdata == null)
            {

            }
            else
            {
                foreach (var item in Contectdata)
                {
                    pn = Convert.ToDecimal(item.phoneNumber).ToString();
                    email = item.emailId;
                }
            }

            //var Digialinfodata = from zxc in db.DigitalInfoMasters where zxc.id == id select zxc;

            int typegardian = 0;
            string fathername = null;
            string mothername = null;
            string gardianname = null;

            var Gardiandata = from lkj in db.GardianMasters.AsNoTracking() where lkj.id == id select lkj;

            foreach (var item in Gardiandata)
            {
                typegardian = Convert.ToInt32(item.typeGardian);
                switch (typegardian)
                {
                    case 1:
                        fathername = item.gFirstName + " " + item.gMiddleName + " " + item.gLastName;
                        break;

                    case 2:
                        mothername = item.gFirstName + " " + item.gMiddleName + " " + item.gLastName;
                        break;

                    case 3:
                        gardianname = item.gFirstName + " " + item.gMiddleName + " " + item.gLastName;
                        break;

                    default:
                        break;
                }
            }

            string pma = null;
            string pna = null;
            string poa = null;
            string pcity = null;
            decimal ppincode = 0;
            bool dbIsLiveInPermentAddress = false;
            string paddress = null;
            string liveThere = null;

            var Permentaddressdata = from poi in db.PermentAddressMasters where poi.id == id select poi;

            foreach (var item in Permentaddressdata)
            {
                pma = item.mainAddress;
                pna = item.nearByAddress;
                poa = item.optionalAddress;
                pcity = item.city;
                ppincode = Convert.ToDecimal(item.pincode);
                dbIsLiveInPermentAddress = item.IsLiveInPermentAddress;
            }
            paddress = pma + ", " + pna + ", " + poa + ".";
            liveThere = (dbIsLiveInPermentAddress) ? "Yes" : "No";

            string oma = null;
            string ona = null;
            string ooa = null;
            string ocity = null;
            decimal opincode = 0;
            int duration = 0;
            string oaddress = null;

            if (!dbIsLiveInPermentAddress)
            {
                var presentaddressdata = from mnb in db.PersentAddressMasters where mnb.id == id select mnb;
                foreach (var item in presentaddressdata)
                {
                    oma = item.mainAddress;
                    ona = item.mainAddress;
                    ooa = item.optionalAddress;
                    ocity = item.city;
                    opincode = Convert.ToDecimal(item.pincode);
                    duration = Convert.ToInt32(item.duration);
                }
            }
            oaddress = oma + ", " + ona + ", " + ooa + ".";

            var result = new { RequestId = rId, FirstName = fname, LastName = lname, Gender = gender, DOB = dob, BloodGroup = bloodGroup, Orphan = orphan, SecondaryPhoneNumber = pn, SecondaryEmailId = email, FatherName = fathername, MotherName = mothername, GardianName = gardianname, PermentAddress = paddress, PermentCity = pcity, PermentPincode = ppincode, LivingThere = liveThere, PresentAddress = oaddress, PresentCity = ocity, PresentPincode = opincode, Duration = duration };

            //var result1 = new { RequestId = rId, FirstName = fname, LastName = lname, Gender = gender, DOB = dob, BloodGroup = bloodGroup, Orphan = orphan, SecondaryPhoneNumber = pn, SecondaryEmailId = email, FatherName = fathername, MotherName = mothername, PermentAddress = paddress, PermentCity = pcity, PermentPincode = ppincode, LivingThere = liveThere, PresentAddress = oaddress, PresentCity = ocity, PresentPincode = opincode, Duration = duration };
            //var result2 = new { RequestId = rId, FirstName = fname, LastName = lname, Gender = gender, DOB = dob, BloodGroup = bloodGroup, Orphan = orphan, SecondaryPhoneNumber = pn, SecondaryEmailId = email, FatherName = fathername, MotherName = mothername, GardianName = gardianname, PermentAddress = paddress, PermentCity = pcity, PermentPincode = ppincode, LivingThere = liveThere };
            //var result3 = new { RequestId = rId, FirstName = fname, LastName = lname, Gender = gender, DOB = dob, BloodGroup = bloodGroup, Orphan = orphan, SecondaryPhoneNumber = pn, SecondaryEmailId = email, FatherName = fathername, MotherName = mothername, PermentAddress = paddress, PermentCity = pcity, PermentPincode = ppincode, LivingThere = liveThere };

            //if (dbisGardad)
            //{
            //    if (dbIsLiveInPermentAddress)
            //    {
            //        return Ok(result2);
            //    }
            //    else
            //    {
            //        return Ok(result);
            //    }
            //}
            //else
            //{
            //    if (dbIsLiveInPermentAddress)
            //    {
            //        return Ok(result3);
            //    }
            //    else
            //    {
            //        return Ok(result1);
            //    }
            //}

            return Ok(result);
        }
    }
}
