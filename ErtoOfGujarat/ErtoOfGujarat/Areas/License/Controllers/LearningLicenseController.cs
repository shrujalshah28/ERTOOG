using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ErtoOfGujarat.Areas.License.Controllers
{

    class myData
    {
        public int requestId;
        public int Age;
        public decimal phoneNo;
        public string emailId;
    }

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
                //client.BaseAddress = new Uri("http://localhost/Aadhar");
                client.BaseAddress = new Uri("http://localhost");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                var response = client.GetAsync(url).Result;
                if (response.IsSuccessStatusCode)
                {
                    string responseString = response.Content.ReadAsStringAsync().Result;
                    //var result = (JObject)response.Content.ReadAsStringAsync().Result;
                    //string ei = result["emailId"].Value<string>();
                    //string rs = responseString.Replace("{", "");
                    //rs = rs.Replace("}", "");
                    myData res = JsonConvert.DeserializeObject<myData>(responseString);
                    //var data = res.Children();
                    //var jsonobject = JObject.Parse(responseString);
                    //var issues = jsonobject[""].Select(x => new
                    //{
                    //    PriorityLevel = (string)x.SelectToken("PriorityLevel"),
                    //    State = (string)x.SelectToken("State")
                    //});
                    //JToken jt= (JToken)data["emailId"].Values();
                    //email = jt.ToString();
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
                    //foreach (var item in data)
                    //{

                    //}
                }
            }


            //using (var client = new HttpClient())
            //{
            //    //var request = Url.RouteUrl(url);
            //    client.BaseAddress = new Uri("http://localhost:55761/");
            //    client.DefaultRequestHeaders.Accept.Add(
            //    new MediaTypeWithQualityHeaderValue("application/json"));

            //    HttpResponseMessage response = client.GetAsync(url).Result;

            //    if (response.IsSuccessStatusCode)
            //    {
            //        //var users = response.Content.ReadAsAsync &
            //        //lt; IEnumerable & lt; Users & gt; &gt; ().Result;
            //        //usergrid.ItemsSource = users;

            //    }
            //    //var productDetailUrl = Url.RouteUrl(
            //    //    "DefaultApi",
            //    //    new { httproute = "", controller = "GetRequest", id = id },
            //    //    Request.Url.Scheme
            //    //);
            //    //var model = client
            //    //            .GetAsync(productDetailUrl)
            //    //            .Result
            //    //            .Content.ReadAsAsync<ProductItems>().Result;

            //    //return View(model);
            //}

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
            for (int i = 0; i < at-4; i++)
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
        public ActionResult ConfirmDetail(string uid,string phoneNo, string emailId)
        {
            string pno = null;
            string email = null;
            var data = from asd in db.ResponceMasters where asd.uniqueId == uid select new { asd.phoneNumber, asd.emailId };
            foreach (var item in data)
            {

            }
            return View();
        }
    }
}