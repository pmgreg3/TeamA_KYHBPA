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
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            var first = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            var events = db.Events.OrderBy(e => e.EventDate).ToList().Where(e => e.EventDate >= first && e.EventTime <= first.AddDays(30)).ToList()
                .Select(e => new Event()
                {
                    EventDate = TimeZoneInfo.ConvertTimeFromUtc(e.EventDate, easternZone),
                    EventDescription = e.EventDescription,
                    EventLocation = e.EventLocation,
                    EventTime = TimeZoneInfo.ConvertTimeFromUtc(e.EventTime, easternZone),
                    EventID = e.EventID,
                    EventName = e.EventName
                }).ToList();

            var viewModel = new PhotoGalleryViewModel

            {
                Events = events
            };
            return View(viewModel);
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
        public ActionResult Membership()
        {
            ViewBag.Message = "Your membership page.";

            return View();
        }
    }
}