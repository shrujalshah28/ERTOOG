using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ErtoOfGujarat.Areas.License.Models;

namespace ErtoOfGujarat.Areas.License.Controllers
{

    public class LearningLicenseController : Controller
    {
        private ERTOOGDBEntities db = new ERTOOGDBEntities();

        // GET: License/LearningLicense
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Terms()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Terms(FormCollection fc)
        {
            string uid = null;
            string date = System.DateTime.Now.Date.ToString();
            string day = System.DateTime.Now.Day.ToString();
            string month = System.DateTime.Now.Month.ToString();
            string year = System.DateTime.Now.Year.ToString();
            string hour = System.DateTime.Now.Hour.ToString();
            string minute = System.DateTime.Now.Minute.ToString();
            string second = System.DateTime.Now.Second.ToString();
            string ms = System.DateTime.Now.Millisecond.ToString();
            uid = day + month + year + hour + minute + second + ms;
            if (uid.Length > 15)
            {
                uid = uid.Substring(0, 15);
            }
            return RedirectToAction("New", "LearningLicense", new { uid = uid });
        }

        [HttpGet]
        public ActionResult New(string uid)
        {
            ViewBag.uid = uid;
            return View();
        }

        [HttpPost]
        public ActionResult New(string uniqueid, string aadharNo)
        {

            string url = "Aadhar/api/AadharApi/AadharIntigration/GetRequest?aadharNo=" + aadharNo + "&eUid=" + uniqueid + "&type=license";

            using (var client = new HttpClient())
            {
                //client.BaseAddress = new Uri("http://localhost:56150");
                client.BaseAddress = new Uri("http://localhost");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    ContactDetails res = JsonConvert.DeserializeObject<ContactDetails>(responseString);
                    int rid = res.requestId;
                    int age = res.Age;
                    string email = res.emailId;
                    decimal mno = res.phoneNo;
                    //rid =Convert.ToInt32(data["requestId"].Values<int>());
                    //age= Convert.ToInt32(data["Age"].Values<int>());
                    //mno= Convert.ToDecimal(data["phoneNo"].Values<int>());
                    ResponceMaster rm = new ResponceMaster() { aadharNo = Convert.ToDecimal(aadharNo), uniqueId = uniqueid, requestId = rid, age = age, phoneNumber = mno, emailId = email, eOTP = 0, mOTP = 0 };
                    db.ResponceMasters.Add(rm);
                    db.SaveChanges();
                }
            }

            return RedirectToAction("ConfirmDetail", "LearningLicense", new { uid = uniqueid });
        }

        [HttpGet]
        public ActionResult ConfirmDetail(string uid)
        {
            var data = from asd in db.ResponceMasters where asd.uniqueId == uid select asd;
            string pno = null;
            string email = null;
            foreach (var item in data)
            {
                pno = Convert.ToDecimal(item.phoneNumber).ToString();
                email = item.emailId;
            }
            string lpno = "******";
            lpno += pno.Substring(6, 4);
            int at = email.IndexOf("@");
            string lemail = email.Substring(0, 4);
            for (int i = 0; i < at - 4; i++)
            {
                lemail += "*";
            }
            lemail += email.Substring(at);
            ViewBag.uid = uid;
            ViewBag.pno = pno;
            ViewBag.email = email;
            ViewBag.lpno = lpno;
            ViewBag.lemail = lemail;
            return View();
        }

        [HttpPost]
        public ActionResult ConfirmDetail(string uniqueid, FormCollection fc)
        {
            string phoneNo = fc["phoneNumber"].ToString();
            string emailId = fc["emailId"].ToString();
            int pkey = 0;
            string rid = null;
            string pno = null;
            string email = null;
            var data = from asd in db.ResponceMasters where asd.uniqueId == uniqueid select new { asd.pKey, asd.requestId, asd.phoneNumber, asd.emailId };
            foreach (var item in data)
            {
                pkey = item.pKey;
                rid = item.requestId.ToString();
                pno = item.phoneNumber.ToString();
                email = item.emailId;
            }

            if (phoneNo == pno && emailId == email)
            {
                string url = "Aadhar/api/AadharApi/AadharIntigration/ConfirmRequest?rId=" + rid + "&conformatiton=true";

                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("http://localhost");
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var responce = client.GetAsync(url).Result;
                    if (responce.IsSuccessStatusCode)
                    {
                        string responceString = responce.Content.ReadAsStringAsync().Result;
                        OtpDetails cd = JsonConvert.DeserializeObject<OtpDetails>(responceString);
                        int rrid = cd.requestId;
                        int motp = cd.mobileOTP;
                        int eotp = cd.emailOTP;
                        // check rid and rrid is same
                        ResponceMaster rm = new ResponceMaster() { pKey = pkey, mOTP = motp, eOTP = eotp };
                        db.Configuration.ValidateOnSaveEnabled = false;
                        try
                        {
                            db.ResponceMasters.Attach(rm);
                            db.Entry(rm).Property(asd => asd.mOTP).IsModified = true;
                            db.Entry(rm).Property(asd => asd.eOTP).IsModified = true;
                            db.SaveChanges();
                        }
                        finally
                        {
                            db.Configuration.ValidateOnSaveEnabled = true;
                        }
                    }
                }
                return RedirectToAction("EnterOTP", "LearningLicense", new { uid = uniqueid });
            }
            else
            {
                return RedirectToAction("ConfirmDetail", "LearningLicense", new { uid = uniqueid });
            }
        }

        [HttpGet]
        public ActionResult EnterOTP(string uid)
        {
            ViewBag.uid = uid;
            return View();
        }

        [HttpPost]
        public ActionResult EnterOTP(string uniqueid, int mOTP, int eOTP)
        {
            int pkey = 0;
            int motp = 0;
            int eotp = 0;
            var data = from asd in db.ResponceMasters where asd.uniqueId == uniqueid select new { asd.pKey, asd.mOTP, asd.eOTP };
            foreach (var item in data)
            {
                pkey = item.pKey;
                motp = (int)(item.mOTP);
                eotp = (int)(item.eOTP);
            }
            if (mOTP == motp && eOTP == eotp)
            {
                // Data can fatch here and send to contoller
                return RedirectToAction("YourDetails", "LearningLicense", new { uid = uniqueid });
            }
            else
            {
                return RedirectToAction("EnterOTP", "LearningLicense", new { uid = uniqueid });
            }
        }

        [HttpGet]
        public ActionResult YourDetails(string uid)
        {
            ViewBag.uid = uid;
            int rid = 0;
            var data1 = from asd in db.ResponceMasters where asd.uniqueId == uid select new { asd.requestId };
            foreach (var item in data1)
            {
                rid = (int)item.requestId;
            }
            string url = "Aadhar/api/AadharApi/AadharIntigration/GetData?rId=" + rid + "&conformation=true";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("http://localhost");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var responce = client.GetAsync(url).Result;
                if (responce.IsSuccessStatusCode)
                {
                    string responceString = responce.Content.ReadAsStringAsync().Result;
                    UserDetails ud = JsonConvert.DeserializeObject<UserDetails>(responceString);
                    return View(ud);
                }
            }

            return View();
        }

        [HttpPost]
        public ActionResult YourDetails(string uid, UserDetails model)
        {
            // uid can be removed from argument.
            string unid = Request["uniqueid"];
            var data = from asd in db.ResponceMasters where asd.uniqueId == unid select new { asd.aadharNo, asd.phoneNumber, asd.emailId };
            string aadharno = null;
            decimal pno = 0;
            string email = null;
            foreach (var item in data)
            {
                aadharno = item.aadharNo.ToString();
                pno = Convert.ToDecimal(item.phoneNumber);
                email = item.emailId;
            }

            int gender = 0;
            switch (model.gender)
            {
                case "Male":
                    gender = 1;
                    break;
                case "Female":
                    gender = 2;
                    break;
                case "Other":
                    gender = 3;
                    break;
                default:
                    break;
            }

            int bloodgroup = 0;
            switch (model.BloodGroup)
            {
                case "A+":
                    bloodgroup = 1;
                    break;
                case "A-":
                    bloodgroup = 2;
                    break;
                case "B+":
                    bloodgroup = 3;
                    break;
                case "B-":
                    bloodgroup = 4;
                    break;
                case "AB+":
                    bloodgroup = 5;
                    break;
                case "AB-":
                    bloodgroup = 6;
                    break;
                case "O+":
                    bloodgroup = 7;
                    break;
                case "O-":
                    bloodgroup = 8;
                    break;
                default:
                    break;
            }

            bool isgarduded = false;
            if (model.Orphan == "Yes")
            {
                isgarduded = true;
            }
            ErtoMaster _ertomaster = new ErtoMaster()
            {
                uniqueId = unid,
                firstName = model.FirstName,
                lastName = model.LastName,
                gender = gender,
                dob = Convert.ToDateTime(model.DOB),
                bloodGroup = bloodgroup,
                isGaurded = isgarduded
            };
            db.ErtoMasters.Add(_ertomaster);
            db.SaveChanges();
            int newId = _ertomaster.id;

            ContectMaster _contectmastermain = new ContectMaster()
            {
                id = newId,
                phoneNumber = pno,
                emailId = email,
                isPrimary = true,
            };
            db.ContectMasters.Add(_contectmastermain);
            db.SaveChanges();

            if (model.SecondaryEmailId != null)
            {
                ContectMaster _contectmastersecondary = new ContectMaster()
                {
                    id = newId,
                    phoneNumber = Convert.ToDecimal(model.SecondaryPhoneNumber),
                    emailId = model.SecondaryEmailId,
                    isPrimary = false,
                };
                db.ContectMasters.Add(_contectmastersecondary);
                db.SaveChanges();
            }

            string[] fathername = model.fatherName.Split(' ');

            GardianMaster _fatherrecord = new GardianMaster()
            {
                id = newId,
                typeGardian = 1,
                gFirstName = fathername[0],
                gMiddleName = fathername[1],
                gLastName = fathername[2],
            };
            db.GardianMasters.Add(_fatherrecord);
            db.SaveChanges();

            string[] mothername = model.MotherName.Split(' ');
            GardianMaster _motherrecord = new GardianMaster()
            {
                id = newId,
                typeGardian = 2,
                gFirstName = mothername[0],
                gMiddleName = mothername[1],
                gLastName = mothername[2],
            };
            db.GardianMasters.Add(_motherrecord);
            db.SaveChanges();

            if (model.Orphan == "Yes")
            {
                string[] gardianname = model.GardianName.Split(' ');
                GardianMaster _gardianrecord = new GardianMaster()
                {
                    id = newId,
                    typeGardian = 3,
                    gFirstName = gardianname[0],
                    gMiddleName = gardianname[1],
                    gLastName = gardianname[2],
                };
                db.GardianMasters.Add(_gardianrecord);
                db.SaveChanges();
            }

            bool islivethere = true;
            if(model.LivingThere=="No")
            {
                islivethere = false;
            }

            string pma = null;
            string pna = null;
            string poa = null;
            string[] address = model.PermentAddress.Split(',');
            switch (address.Length)
            {
                case 3:
                    pma = address[0];
                    pna = address[1];
                    poa = address[2];
                    break;
                case 4:
                    pma = address[0] + "," + address[1];
                    pna = address[2];
                    poa = address[3];
                    break;
                case 5:
                    pma = address[0] + "," + address[1];
                    pna = address[2] + "," + address[3];
                    poa = address[4];
                    break;
                default:
                    pma = address[0] + "," + address[1];
                    pna = address[2] + "," + address[3];
                    poa = address[4] + "," + address[5];
                    break;
            }

            PermentAddressMaster _permentaddress = new PermentAddressMaster()
            {
                id = newId,
                mainAddress = pma,
                nearByAddress = pna,
                optionalAddress = poa,
                city = model.PermentCity,
                pincode = Convert.ToDecimal(model.PermentPincode),
                IsLiveInPermentAddress = islivethere,
            };
            db.PermentAddressMasters.Add(_permentaddress);
            db.SaveChanges();

            if (!islivethere)
            {
                string oma = null;
                string ona = null;
                string ooa = null;
                string[] optionaladdress = model.PresentAddress.Split(',');
                switch (address.Length)
                {
                    case 3:
                        oma = address[0];
                        ona = address[1];
                        ooa = address[2];
                        break;
                    case 4:
                        oma = address[0] + "," + address[1];
                        ona = address[2];
                        ooa = address[3];
                        break;
                    case 5:
                        oma = address[0] + "," + address[1];
                        ona = address[2] + "," + address[3];
                        ooa = address[4];
                        break;
                    default:
                        oma = address[0] + "," + address[1];
                        ona = address[2] + "," + address[3];
                        ooa = address[4] + "," + address[5];
                        break;
                }

                PersentAddressMaster _persentaddress = new PersentAddressMaster()
                {
                    id = newId,
                    mainAddress = oma,
                    nearByAddress = ona,
                    optionalAddress = ooa,
                    city = model.PresentCity,
                    pincode = Convert.ToDecimal(model.PresentPincode),
                    duration = Convert.ToInt32(model.Duration),
                };
                db.PersentAddressMasters.Add(_persentaddress);
                db.SaveChanges();
            }

            ExternalIdentityMaster _externalidentity = new ExternalIdentityMaster()
            {
                id = newId,
                idenityType = 1,
                identityNo = aadharno,
            };
            db.ExternalIdentityMasters.Add(_externalidentity);
            db.SaveChanges();

            return RedirectToAction("SelectVanue", "LearningLicense", new { uid = unid });
        }

        [HttpGet]
        public ActionResult SelectVanue(string uid)
        {
            ViewBag.uid = uid;
            ViewBag.vanue = new SelectList(db.vanueMasters, "pKey", "vanueName");
            return View();
        }

        [HttpPost]
        public ActionResult SelectVanue(string uid,int vanue)
        {
            return View();
        }
    }
}