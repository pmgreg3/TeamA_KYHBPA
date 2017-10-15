using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KYHBPA_TeamA.Models;
using System.Net;

namespace KYHBPA_TeamA.Controllers
{
    public class CalendarController : Controller
    {
        private readonly ApplicationDbContext _db = new ApplicationDbContext();

        // GET: Calendar
        public ActionResult Index()
        {
            return View();
        }

        //public ActionResult Backend()
        //{
        //    return new Dpm().CallBack(this);
        //}

        public ActionResult Edit(string id, string name,string text, string location)
        {

            var eventToModify = _db.Events.FirstOrDefault(ev => ev.EventID.ToString() == id);

            eventToModify.EventName = name;
            eventToModify.EventDescription = text;
            eventToModify.EventLocation = location;
            _db.SaveChanges();

            return new HttpStatusCodeResult(HttpStatusCode.OK);
            //return new Dpm().CallBack(this);
        }
    }


    //public class Dpm : DayPilotMonth
    //{
    //    private readonly ApplicationDbContext _db = new ApplicationDbContext();
    //    protected override void OnInit(InitArgs e)
    //    {
    //        Events = from ev in _db.Events where !((ev.end_date <= VisibleStart) || (ev.start_date >= VisibleEnd)) select ev;

    //        DataIdField = "id";
    //        DataTextField = "text";
    //        DataStartField = "start_date";
    //        DataEndField = "end_date";

    //        Update();
    //    }

    //    protected override void OnFinish()
    //    {
    //        if (UpdateType == CallBackUpdateType.None)
    //        {
    //            return;
    //        }

    //        DataIdField = "id";
    //        DataStartField = "start_date";
    //        DataEndField = "end_date";
    //        DataTextField = "text";


    //        Events = from e in _db.Events where !((e.end_date <= VisibleStart) || (e.start_date >= VisibleEnd)) select e;
    //    }


    //    protected override void OnTimeRangeSelected(TimeRangeSelectedArgs e)
    //    {
    //        if (string.IsNullOrEmpty((string)e.Data["name"]))
    //        {
    //            return;
    //        }

    //        var createdEvent = new Event()
    //        {
    //            text = (string)e.Data["name"],
    //            start_date = e.Start,
    //            end_date = e.End
    //        };

    //        _db.Events.Add(createdEvent);

    //        _db.SaveChanges();

    //        Update();
    //    }

    //    protected override void OnEventMove(EventMoveArgs e)
    //    {
    //        var dbEvent = _db.Events.FirstOrDefault(ev => ev.id.ToString() == e.Id);

    //        if (dbEvent != null)
    //        {

    //            dbEvent.start_date = e.NewStart;
    //            dbEvent.end_date = e.NewEnd;

    //            _db.SaveChanges();
    //        }

    //        Update();            
    //    }

    //    protected override void OnEventClick(EventClickArgs e)
    //    {
    //        if (string.IsNullOrEmpty(e.Text))
    //        {
    //            return;
    //        }

    //        var dbEvent = _db.Events.FirstOrDefault(ev => ev.id.ToString() == e.Id);

    //        if (dbEvent != null)
    //        {
    //            dbEvent.text = e.Text;
    //            dbEvent.start_date = e.Start;
    //            dbEvent.end_date = e.End;

    //            _db.SaveChanges();
    //        }

    //        Update();
}