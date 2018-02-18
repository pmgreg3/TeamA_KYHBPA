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
            var vm = new LandingPageViewModel();
            vm.FeaturedArticles = db.Posts.Where(x => x.FrontPageFeature == true).ToList();

            return View(vm);
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
        public ActionResult RacingEducation()
        {
            ViewBag.Message = "Your racing education page.";

            return View();
        }
        public ActionResult Membership()
        {
            ViewBag.Message = "Your membership page.";

            return View();
        }
    }
}