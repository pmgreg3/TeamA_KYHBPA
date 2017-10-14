using KYHBPA_TeamA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Xml.Linq;


namespace KYHBPA_TeamA.Controllers
{
    public class EventController : Controller
    {
        //Refer to this link in order to set up the Calendar.
        //http://scheduler-net.com/docs/simple-.net-mvc-application-with-scheduler.html#step_2_add_the_scheduler_reference

        public readonly ApplicationDbContext _db = new ApplicationDbContext();

        public ActionResult Index()
        {
            //try { 
            //var scheduler = new DHXScheduler(this); //initializes dhtmlxScheduler
            //scheduler.LoadData = true;// allows loading data
            //scheduler.EnableDataprocessor = true;// enables DataProcessor in order to enable implementation CRUD operations

            //    return View(scheduler);
            //}
            //catch (Exception ex)
            //{
            //    if (ex != null)
            //    {
            //        return RedirectToAction("Index", "Home");
            //    }                
            //}

            //return RedirectToAction("Index", "Home");
            return View();
        }

        public JsonResult Data()
        {
            //Using Dxhtml JavaScript Edition (open source)
            var events = _db.Events;

            var formatedEvents = new List<object>();
            foreach (var ev in events)
            {
                var formattingEvent = new
                {
                    id = ev.EventID,
                    start_date = ev.EventDate.ToString("MM/dd/yyyy"),
                    end_date = ev.EventTime.ToString("MM/dd/yyyy"),
                    //start_date = ev.EventDate.Date.ToString("MM/dd/yyyy HH:mm"),
                    //end_date = ev.EventTime.Date.ToString("MM/dd/yyyy HH:mm"),

                    //name = ev.EventName,
                    text = ev.EventDescription
                    //location = ev.EventLocation

                };
                formatedEvents.Add(formattingEvent);
            }



            return Json(formatedEvents, JsonRequestBehavior.AllowGet);

            //Using Dxhtml MVC Scheduler Edition (free trial)
            //events for loading to scheduler
            //return new SchedulerAjaxData(_db.Events);
        }

        public ActionResult Save(string id, string text, string description, string location, 
            string start_date, string end_date)
        {

            var existingEvent = _db.Events.FirstOrDefault(e => e.EventID.ToString() == id);
            var newDate = Convert.ToDateTime(start_date);
            var newTime = Convert.ToDateTime(end_date);


            if (existingEvent != null)
            {
                existingEvent.EventDate = newDate;
                existingEvent.EventTime = newTime;
                existingEvent.EventName = text;
                existingEvent.EventDescription = description;
                existingEvent.EventLocation = location;
            }
            else
            {

                var newEvent = new Event()
                {
                    EventDate = newDate,
                    EventTime = newTime,
                    EventName = text,
                    EventDescription = description,
                    EventLocation = location

                };
                _db.Events.Add(newEvent);
            }

            _db.SaveChanges();



            return View("Index");
        }

        public ActionResult Delete(string id, string name, string description, string location, string date, string time)
        {

            var existingEvent = _db.Events.FirstOrDefault(e => e.EventID.ToString() == id);
            var newDate = Convert.ToDateTime(date);
            var newTime = Convert.ToDateTime(time);

            if (existingEvent != null)
            {
                _db.Events.Remove(existingEvent);
                _db.SaveChanges();
            }

            return View("Index");
        }


        //public ActionResult Save(Event updatedEvent, FormCollection formData)
        //{
        //    var action = new DataAction(formData);

        //    try
        //    {
        //        switch (action.Type)
        //        {
        //            case DataActionTypes.Insert: // your Insert logic
        //                _db.Events.Add(updatedEvent);
        //                break;
        //            case DataActionTypes.Delete: // your Delete logic
        //                updatedEvent = _db.Events.SingleOrDefault(ev => ev.id == updatedEvent.id);
        //                _db.Events.Remove(updatedEvent);
        //                break;
        //            default:// "update" // your Update logic
        //                updatedEvent = _db.Events.SingleOrDefault(
        //                ev => ev.id == updatedEvent.id);
        //                UpdateModel(updatedEvent);
        //                break;
        //        }
        //        _db.SaveChanges();
        //        action.TargetId = updatedEvent.id;
        //    }
        //    catch (Exception e)
        //    {
        //        action.Type = DataActionTypes.Error;
        //    }
        //    return (new AjaxSaveResponse(action));
        //}
        // GET: Event
    }
}