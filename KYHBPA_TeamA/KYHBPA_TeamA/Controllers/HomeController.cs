using KYHBPA_TeamA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KYHBPA_TeamA.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "About the KY HBPA page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "KY HBPA Contact information page.";

            return View();
        }
        public ActionResult Board()
        {
            ViewBag.Message = "KY HBPA Board of Directors page.";

            return View();
        }
        public ActionResult Benefits()
        {
            ViewBag.Message = "Your Benevolence page.";

            return View();
        }
        public ActionResult Polls()
        {
            ViewBag.Message = "Your Polls page.";

            return View();
        }
        public ActionResult GamblingEducation()
        {
            ViewBag.Message = "Your gambling education page.";

            return View();
        }
    }
}