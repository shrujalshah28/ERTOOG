using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ErtoOfGujarat.Areas.License.Controllers
{
    public class LearningLicenseController : Controller
    {
        // GET: License/LearningLicense
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult New()
        {

            return View();
        }
    }
}