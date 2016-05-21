using ErtoOfGujarat.Areas.License.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;

namespace ErtoOfGujarat.Areas.License.Controllers
{

    public class LearningLicenseController : Controller
    {
        private ERTOOGDBEntities _db = new ERTOOGDBEntities();

        // GET: License/LearningLicense
        public ActionResult Index()
        {
            return View();
        }

        // GET: License/LearningLicense/Terms
        [HttpGet]
        public ActionResult Terms()
        {
            return View();
        }

        // POST: License/LearningLicense/Terms
        [HttpPost]
        public ActionResult Terms(FormCollection fc)
        {
            string uid = null;
            string date = DateTime.Now.Date.ToString();
            string day = DateTime.Now.Day.ToString();
            string month = DateTime.Now.Month.ToString();
            string year = DateTime.Now.Year.ToString();
            string hour = DateTime.Now.Hour.ToString();
            string minute = DateTime.Now.Minute.ToString();
            string second = DateTime.Now.Second.ToString();
            string ms = DateTime.Now.Millisecond.ToString();

            uid = day + month + year + hour + minute + second + ms;

            if (uid.Length > 15)
            {
                uid = uid.Substring(0, 15);
            }

            return RedirectToAction("New", "LearningLicense", new { uid = uid });
        }

        // GET: License/LearningLicense/New
        [HttpGet]
        public ActionResult New(string uid)
        {
            ViewBag.uid = uid;

            return View();
        }

        // POST: License/LearningLicense/New
        [HttpPost]
        public ActionResult New(string uniqueId, string aadharNo)
        {

            string url = "Aadhar/api/AadharApi/AadharIntigration/GetRequest?aadharNo=" + aadharNo + "&eUid=" + uniqueId + "&type=license";

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
                    ContactDetails _contectdetails = JsonConvert.DeserializeObject<ContactDetails>(responseString);
                    int rid = _contectdetails.requestId;
                    int age = _contectdetails.Age;
                    string email = _contectdetails.emailId;
                    decimal mno = _contectdetails.phoneNo;
                    //rid =Convert.ToInt32(data["requestId"].Values<int>());
                    //age= Convert.ToInt32(data["Age"].Values<int>());
                    //mno= Convert.ToDecimal(data["phoneNo"].Values<int>());
                    ResponceMaster _responcemaster = new ResponceMaster()
                    {
                        aadharNo = Convert.ToDecimal(aadharNo),
                        uniqueId = uniqueId,
                        requestId = rid,
                        age = age,
                        phoneNumber = mno,
                        emailId = email,
                        eOTP = 0,
                        mOTP = 0
                    };
                    _db.ResponceMasters.Add(_responcemaster);
                    _db.SaveChanges();
                }
            }

            return RedirectToAction("ConfirmDetail", "LearningLicense", new { uid = uniqueId });
        }

        // GET: License/LearningLicense/ConfirmDetail
        [HttpGet]
        public ActionResult ConfirmDetail(string uid)
        {
            var data = from asd in _db.ResponceMasters where asd.uniqueId == uid select asd;

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

        // POST: License/LearningLicense/ConfirmDetail
        [HttpPost]
        public ActionResult ConfirmDetail(string uniqueId, FormCollection fc)
        {
            string phoneNo = fc["phoneNumber"].ToString();
            string emailId = fc["emailId"].ToString();

            int pkey = 0;
            string rid = null;
            string pno = null;
            string email = null;
            var data = from asd in _db.ResponceMasters where asd.uniqueId == uniqueId select new { asd.pKey, asd.requestId, asd.phoneNumber, asd.emailId };
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
                        OtpDetails _otpdetails = JsonConvert.DeserializeObject<OtpDetails>(responceString);
                        int rrid = _otpdetails.requestId;
                        int motp = _otpdetails.mobileOTP;
                        int eotp = _otpdetails.emailOTP;
                        // check rid and rrid is same
                        ResponceMaster _responcemaster = new ResponceMaster()
                        {
                            pKey = pkey,
                            mOTP = motp,
                            eOTP = eotp
                        };
                        _db.Configuration.ValidateOnSaveEnabled = false;
                        try
                        {
                            _db.ResponceMasters.Attach(_responcemaster);
                            _db.Entry(_responcemaster).Property(asd => asd.mOTP).IsModified = true;
                            _db.Entry(_responcemaster).Property(asd => asd.eOTP).IsModified = true;
                            _db.SaveChanges();
                        }
                        finally
                        {
                            _db.Configuration.ValidateOnSaveEnabled = true;
                        }
                    }
                }

                return RedirectToAction("EnterOTP", "LearningLicense", new { uid = uniqueId });
            }
            else
            {
                return RedirectToAction("ConfirmDetail", "LearningLicense", new { uid = uniqueId });
            }
        }

        // GET: License/LearningLicense/EnterOTP
        [HttpGet]
        public ActionResult EnterOTP(string uid)
        {
            ViewBag.uid = uid;

            return View();
        }

        // POST: License/LearningLicense/EnterOTP
        [HttpPost]
        public ActionResult EnterOTP(string uniqueId, int mOTP, int eOTP)
        {
            int pkey = 0;
            int motp = 0;
            int eotp = 0;
            var data = from asd in _db.ResponceMasters where asd.uniqueId == uniqueId select new { asd.pKey, asd.mOTP, asd.eOTP };
            foreach (var item in data)
            {
                pkey = item.pKey;
                motp = (int)(item.mOTP);
                eotp = (int)(item.eOTP);
            }

            if (mOTP == motp && eOTP == eotp)
            {
                // Data can fatch here and send to contoller
                return RedirectToAction("YourDetails", "LearningLicense", new { uid = uniqueId });
            }
            else
            {
                return RedirectToAction("EnterOTP", "LearningLicense", new { uid = uniqueId });
            }
        }

        // GET: License/LearningLicense/YourDetails
        [HttpGet]
        public ActionResult YourDetails(string uid)
        {
            ViewBag.uid = uid;

            int rid = 0;
            var data1 = from asd in _db.ResponceMasters where asd.uniqueId == uid select new { asd.requestId };
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
                    UserDetails _userdetails = JsonConvert.DeserializeObject<UserDetails>(responceString);
                    return View(_userdetails);
                }
            }

            return View();
        }

        // POST: License/LearningLicense/YourDetails
        [HttpPost]
        public ActionResult YourDetails(string uid, UserDetails model)
        {
            // uid can be removed from argument.
            string unid = Request["uniqueId"];

            var data = from asd in _db.ResponceMasters where asd.uniqueId == unid select new { asd.aadharNo, asd.phoneNumber, asd.emailId };
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
            _db.ErtoMasters.Add(_ertomaster);
            _db.SaveChanges();
            int newId = _ertomaster.id;

            ContectMaster _contectmastermain = new ContectMaster()
            {
                id = newId,
                phoneNumber = pno,
                emailId = email,
                isPrimary = true,
            };
            _db.ContectMasters.Add(_contectmastermain);
            _db.SaveChanges();

            if (model.SecondaryEmailId != null)
            {
                ContectMaster _contectmastersecondary = new ContectMaster()
                {
                    id = newId,
                    phoneNumber = Convert.ToDecimal(model.SecondaryPhoneNumber),
                    emailId = model.SecondaryEmailId,
                    isPrimary = false,
                };
                _db.ContectMasters.Add(_contectmastersecondary);
                _db.SaveChanges();
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
            _db.GardianMasters.Add(_fatherrecord);
            _db.SaveChanges();

            string[] mothername = model.MotherName.Split(' ');
            GardianMaster _motherrecord = new GardianMaster()
            {
                id = newId,
                typeGardian = 2,
                gFirstName = mothername[0],
                gMiddleName = mothername[1],
                gLastName = mothername[2],
            };
            _db.GardianMasters.Add(_motherrecord);
            _db.SaveChanges();

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
                _db.GardianMasters.Add(_gardianrecord);
                _db.SaveChanges();
            }

            bool islivethere = true;
            if (model.LivingThere == "No")
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
            _db.PermentAddressMasters.Add(_permentaddress);
            _db.SaveChanges();

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
                _db.PersentAddressMasters.Add(_persentaddress);
                _db.SaveChanges();
            }

            ExternalIdentityMaster _externalidentity = new ExternalIdentityMaster()
            {
                id = newId,
                idenityType = 1,
                identityNo = aadharno,
            };
            _db.ExternalIdentityMasters.Add(_externalidentity);
            _db.SaveChanges();

            return RedirectToAction("SelectVanue", "LearningLicense", new { uid = unid });
        }

        // GET: License/LearningLicense/SelectVanue
        [HttpGet]
        public ActionResult SelectVanue(string uid)
        {
            ViewBag.uid = uid;
            ViewBag.vanue = new SelectList(_db.vanueMasters, "pKey", "vanueName");

            return View();
        }

        // POST: License/LearningLicense/SelectVanue
        [HttpPost]
        public ActionResult SelectVanue(string uniqueId, int vanue)
        {
            var data = from asd in _db.vanueMasters where asd.pKey == vanue select new { asd.vanueName };
            string place = null;
            foreach (var item in data)
            {
                place = item.vanueName;
            }

            return RedirectToAction("PickADate", "LearningLicense", new { uid = uniqueId, vanue = place });
        }

        // GET: License/LearningLicense/PickADate
        [HttpGet]
        public ActionResult PickADate(string uid, string vanue)
        {
            ViewBag.uid = uid;
            ViewBag.vanue = vanue;
            DateTime today = DateTime.Today;
            int count = 5;
            List<DateTime> datelist = new List<DateTime>();
            Hashtable datehashlist = new Hashtable();
            while (count > 0)
            {
                today = today.AddDays(1);
                DateTime date = DateTime.Today;
                int totalcount = 0;
                int pkey = 0;
                //_db.AppointmentDateMasters.ToList().Where(a => a.date == today && a.vanue == 1 && a.totalCount < 450); //It actualy worked!!!
                var data = from asd in _db.AppointmentDateMasters join qwe in _db.vanueMasters on asd.vanue equals qwe.pKey where (qwe.vanueName == vanue && asd.date == today) select asd;
                foreach (var item in data)
                {
                    pkey = item.pKey;
                    date = item.date;
                    totalcount = (int)item.totalCount;
                }
                if (totalcount < 450)
                {
                    datehashlist.Add(pkey, date);
                    datelist.Add(date);
                    count--;
                }
                else
                {
                    continue;
                }

            }

            ViewBag.datehash = new SelectList(datehashlist, "key", "value");

            return View();
        }

        // POST: License/LearningLicense/PickADate
        [HttpPost]
        public ActionResult PickADate(string uniqueId, string vanue, int datehash)
        {
            return RedirectToAction("ChooseTime", "LearningLicense", new { uid = uniqueId, vanue = vanue, dateid = datehash });
        }

        // GET: License/LearningLicense/ChooseTime
        [HttpGet]
        public ActionResult ChooseTime(string uid, string vanue, int dateid)
        {
            ViewBag.uid = uid;
            ViewBag.vanue = vanue;

            var data1 = from zxc in _db.AppointmentDateMasters where zxc.pKey == dateid select new { zxc.date };
            string selecteddate = null;
            foreach (var item in data1)
            {
                selecteddate = item.date.ToShortDateString();
            }
            ViewBag.selecteddate = selecteddate;
            DateTime time = new DateTime(2016, 05, 12, 10, 0, 0);
            int starthour = time.Hour;
            int startminute = time.Minute;
            Hashtable timehash = new Hashtable(15);

            var count = (from asd in _db.AppointmentTimeMasters where asd.date == dateid select asd).Count();
            if (count <= 0)
            {
                for (int i = 0; i < 14; i++)
                {
                    DateTime newtime = time.AddMinutes(30);
                    if (time.Hour == 12 && time.Minute == 0)
                    {
                        newtime = newtime.AddMinutes(30);
                        time = time.AddMinutes(30);
                    }
                    string temp = time.Hour.ToString() + ":" + time.Minute.ToString() + " - " + newtime.Hour.ToString() + ":" + newtime.Minute.ToString();
                    timehash.Add(i + 1, temp);
                    time = newtime;
                }
            }
            else
            {
                for (int i = 0; i < 14; i++)
                {
                    var data = from qwe in _db.AppointmentTimeMasters where qwe.date == dateid && qwe.timeSlot == (i + 1) select qwe;
                    var counttime = data.Count();
                    if (counttime <= 0)
                    {
                        DateTime newtime = time.AddMinutes(30 * i);
                        string temp = starthour.ToString() + ":" + startminute.ToString() + " - " + newtime.Hour.ToString() + ":" + newtime.Minute.ToString();
                        timehash.Add(i + 1, temp);
                    }
                    else if (counttime > 0)
                    {
                        int totalcount = 0;
                        foreach (var item in data)
                        {
                            totalcount = (int)item.totalCount;
                        }
                        if (totalcount < 30)
                        {
                            DateTime newtime = time.AddMinutes(30 * i);
                            string temp = starthour.ToString() + ":" + startminute.ToString() + " - " + newtime.Hour.ToString() + ":" + newtime.Minute.ToString();
                            timehash.Add(i + 1, temp);
                        }
                        else
                        {
                            continue;
                        }
                    }
                }
            }

            ViewBag.timehash = new SelectList(timehash, "key", "value");

            return View();
        }

        // POST: License/LearningLicense/ChooseTime
        [HttpPost]
        public ActionResult ChooseTime(string uniqueId, string vanue, int datehash, int timehash)
        {
            return RedirectToAction("ConfirmAppointment", "LearningLicense", new { uid = uniqueId, vanue = vanue, dateid = datehash, timeid = timehash });
        }

        // GET: License/LearningLicense/ConfirmAppointment
        [HttpGet]
        public ActionResult ConfirmAppointment(string uid, string vanue, int dateid, int timeid)
        {
            ViewBag.uid = uid;
            ViewBag.vanue = vanue;

            var data = from zxc in _db.AppointmentDateMasters where zxc.pKey == dateid select new { zxc.date };
            string selecteddate = null;
            foreach (var item in data)
            {
                selecteddate = item.date.ToShortDateString();
            }
            ViewBag.selecteddate = selecteddate;
            string selectedtime = null;
            DateTime temp = new DateTime(13, 05, 2016, 09, 30, 00);
            if (timeid < 5)
            {
                selectedtime = (temp.AddMinutes(timeid * 30)).Hour.ToString() + ":" + (temp.AddMinutes(timeid * 30)).Minute.ToString() + "-" + (temp.AddMinutes((timeid + 1) * 30)).Hour.ToString() + ":" + (temp.AddMinutes((timeid + 1) * 30)).Minute.ToString();
            }
            else
            {
                selectedtime = (temp.AddMinutes((timeid + 1) * 30)).Hour.ToString() + ":" + (temp.AddMinutes((timeid + 1) * 30)).Minute.ToString() + "-" + (temp.AddMinutes((timeid + 2) * 30)).Hour.ToString() + ":" + (temp.AddMinutes((timeid + 2) * 30)).Minute.ToString();
            }

            ViewBag.selectedtime = selectedtime;

            return View();
        }

        // POST: License/LearningLicense/ConfirmAppointment
        [HttpPost]
        public ActionResult ConfirmAppointment(FormCollection fc)
        {
            string uniqueid = fc["uniqueId"];
            string vanue = fc["vanue"];
            int datehash = Convert.ToInt32(fc["datehash"]);
            int timehash = Convert.ToInt32(fc["timehash"]);

            int vanueid = 0;
            var data = from asd in _db.vanueMasters where asd.vanueName == vanue select new { asd.pKey };
            foreach (var item in data)
            {
                vanueid = item.pKey;
            }

            var datedata = from qwe in _db.AppointmentDateMasters where qwe.pKey == datehash && qwe.vanue == vanueid select qwe;
            int totalcount = 0;
            DateTime date = DateTime.Now;
            foreach (var item in datedata)
            {
                date = item.date;
                totalcount = (int)item.totalCount;
            }

            AppointmentDateMaster _appointmentdatedaster = new AppointmentDateMaster()
            {
                pKey = datehash,
                totalCount = (totalcount + 1)
            };
            _db.Configuration.ValidateOnSaveEnabled = false;
            try
            {
                _db.AppointmentDateMasters.Attach(_appointmentdatedaster);
                _db.Entry(_appointmentdatedaster).Property(asd => asd.totalCount).IsModified = true;
                _db.SaveChanges();
            }
            finally
            {
                _db.Configuration.ValidateOnSaveEnabled = true;
            }

            string time = null;
            DateTime temp = new DateTime(13, 05, 2016, 09, 30, 00);
            if (timehash < 5)
            {
                time = (temp.AddMinutes(timehash * 30)).Hour.ToString() + ":" + (temp.AddMinutes(timehash * 30)).Minute.ToString() + "-" + (temp.AddMinutes((timehash + 1) * 30)).Hour.ToString() + ":" + (temp.AddMinutes((timehash + 1) * 30)).Minute.ToString();
            }
            else
            {
                time = (temp.AddMinutes((timehash + 1) * 30)).Hour.ToString() + ":" + (temp.AddMinutes((timehash + 1) * 30)).Minute.ToString() + "-" + (temp.AddMinutes((timehash + 2) * 30)).Hour.ToString() + ":" + (temp.AddMinutes((timehash + 2) * 30)).Minute.ToString();
            }

            var timedata = from zxc in _db.AppointmentTimeMasters where zxc.date == datehash && zxc.timeSlot == timehash select zxc;
            int count = timedata.Count();
            int timepkey = 0;
            if (count <= 0)
            {
                AppointmentTimeMaster _appointmenttimemaster = new AppointmentTimeMaster()
                {
                    date = datehash,
                    timeSlot = timehash,
                    totalCount = 1
                };
                _db.AppointmentTimeMasters.Add(_appointmenttimemaster);
                _db.SaveChanges();
                timepkey = _appointmenttimemaster.pkey;
            }
            else
            {
                int totaltimecount = 0;
                foreach (var item in timedata)
                {
                    timepkey = item.pkey;
                    totaltimecount = (int)item.totalCount;
                }

                AppointmentTimeMaster _appointmenttimemaster = new AppointmentTimeMaster()
                {
                    pkey = timepkey,
                    totalCount = (totaltimecount + 1)
                };
                _db.Configuration.ValidateOnSaveEnabled = false;
                try
                {
                    _db.AppointmentTimeMasters.Attach(_appointmenttimemaster);
                    _db.Entry(_appointmenttimemaster).Property(zxc => zxc.totalCount).IsModified = true;
                    _db.SaveChanges();
                }
                finally
                {
                    _db.Configuration.ValidateOnSaveEnabled = true;
                }
            }

            var userdata = from qwe in _db.ErtoMasters where qwe.uniqueId == uniqueid select new { qwe.id };
            int id = 0;
            foreach (var item in userdata)
            {
                id = item.id;
            }

            AppointmentMaster _appointmentmaster = new AppointmentMaster()
            {
                id = id,
                timeOfAppointment = timepkey
            };
            _db.AppointmentMasters.Add(_appointmentmaster);
            _db.SaveChanges();

            string email = null;
            var contectdata = from lkj in _db.ContectMasters where lkj.id == id && lkj.isPrimary == true select lkj;
            foreach (var item in contectdata)
            {
                email = item.emailId;
            }

            MailMessage _mailmessage = new MailMessage("shrujalshah28@gmail.com", email);
            _mailmessage.Subject = "Appointment";
            _mailmessage.Body = "<p>Appointment Letter</p></br><table style=\"width:20%\"><tr><td> Vanue </td><td> " + vanue + "</td></tr><tr><td> Date </td><td>" + date + "</td></tr><tr><td> Time </td><td>" + time + "</td></tr></table></br><p> Call Shrujal and confirm your OTP.</p>";
            _mailmessage.IsBodyHtml = true;

            SmtpClient sc = new SmtpClient();
            sc.Send(_mailmessage);

            return RedirectToAction("ThankYou", "LearningLicense");
        }

        // GET: License/LearningLicense/ThankYou
        [HttpGet]
        public ActionResult ThankYou()
        {
            return View();
        }
    }
}