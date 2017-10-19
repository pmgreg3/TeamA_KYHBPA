using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KYHBPA_TeamA.Controllers
{
    public class PollsController : Controller
    {
        // GET: Polls
        public ActionResult Index()
        {
            return View();
        }
    }
}