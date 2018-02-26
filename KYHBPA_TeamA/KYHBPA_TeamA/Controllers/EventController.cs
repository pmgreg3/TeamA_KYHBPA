using KYHBPA_TeamA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Xml.Linq;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Newtonsoft.Json;
using System.IO;
using System.Text.RegularExpressions;
using System.Globalization;
using Newtonsoft.Json.Schema;
using Newtonsoft.Json.Serialization;

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
            TimeZoneInfo easternZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            foreach (var ev in events)
            {
                var formattingEvent = new
                {
                    id = ev.EventID,
                    start_date = TimeZoneInfo.ConvertTimeFromUtc(ev.EventDate, easternZone).ToString("MM/dd/%y HH:mm"),
                    end_date = TimeZoneInfo.ConvertTimeFromUtc(ev.EventTime, easternZone).ToString("MM/dd/%y HH:mm"),
                    //start_date = ev.EventDate.Date.ToString("MM/dd/yyyy HH:mm"),
                    //end_date = ev.EventTime.Date.ToString("MM/dd/yyyy HH:mm"),
                    text = ev.EventDescription

                };
                formatedEvents.Add(formattingEvent);
            }



            return Json(formatedEvents, JsonRequestBehavior.AllowGet);

            //Using Dxhtml MVC Scheduler Edition (free trial)
            //events for loading to scheduler
            //return new SchedulerAjaxData(_db.Events);
        }

        [Authorize]
        public ActionResult Save(string id, string text, string description, string location, 
            string start_date, string end_date)
        {

            var existingEvent = _db.Events.FirstOrDefault(e => e.EventID.ToString() == id);
            var newDate = Convert.ToDateTime(start_date).ToUniversalTime();
            var newTime = Convert.ToDateTime(end_date).ToUniversalTime();


            if (existingEvent != null)
            {
                existingEvent.EventDate = newDate;
                existingEvent.EventTime = newTime;
                existingEvent.EventDescription = text;
            }
            else
            {

                var newEvent = new Models.Event()
                {
                    EventDate = newDate,
                    EventTime = newTime,
                    EventDescription = text,
                };
                _db.Events.Add(newEvent);
            }

            _db.SaveChanges();



            return View("Index");
        }

        [Authorize]
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


        public ActionResult _GoogleEvents()
        {
            var googleEventsDisplayModel = new GoogleEventsDisplayModel();
            string[] SCOPES = { CalendarService.Scope.CalendarReadonly };
            const string CALENDAR_ID = "4uesds7cl60f2g9grtq376q67k@group.calendar.google.com";
            var eventsViewModelList = new List<EventDisplayViewModel>();
            var path = Server.MapPath(@"~/Content/GoogleCalendar/Kentucky HBPA Website Calendar-5072c6abaf14.json");
            ServiceAccountCredential credential;


            using (var stream =
                new FileStream(path, FileMode.Open, FileAccess.Read))
            {

                var credPath = stream.Name;
                var json = System.IO.File.ReadAllText(credPath);
                var serviceAccount = JsonConvert.DeserializeObject<GoogleCalendarServiceAccount>(json);

                credential = new ServiceAccountCredential
                    (
                        new ServiceAccountCredential.Initializer(serviceAccount.Client_email)
                        {
                            Scopes = SCOPES
                        }.FromPrivateKey(serviceAccount.Private_key)
                    );
            }

            var calendarService = new CalendarService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential
            });

            EventsResource.ListRequest request = calendarService.Events.List(CALENDAR_ID);
            request.TimeMin = DateTime.Now;
            request.ShowDeleted = false;
            request.SingleEvents = true;
            request.MaxResults = 4;
            request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

            try
            {
                var events = request.Execute();
                var jsonEvents = CreateJsonFromEventViewModels(events);

                var eventId = 0;

                if (events.Items != null && events.Items.Count > 0)
                {
                    foreach (var eventItem in events.Items)
                    {
                        eventId += 1;
                        var eventVM = new EventDisplayViewModel()
                        {
                            Title = eventItem.Summary,
                            Description = eventItem.Description,
                            StartDate = eventItem.Start.Date,
                            StartTime = ExtractDateTimeFromDateTimeRaw(eventItem.Start.DateTimeRaw),
                            Link = eventItem.HtmlLink,
                            Location = ParseLocationFromString(eventItem.Location),
                            IdForElement = eventId,
                            Url = eventItem.HtmlLink
                        };

                        if (eventItem.EndTimeUnspecified != null && eventItem.EndTimeUnspecified != false)
                        {
                            eventVM.EndDate = eventItem.End.Date;
                            eventVM.EndTime = eventItem.End.DateTime.ToString();
                        }

                        eventsViewModelList.Add(eventVM);
                    }
                }

                googleEventsDisplayModel.EventDisplayViewModels = eventsViewModelList;
                googleEventsDisplayModel.SchemaJson = jsonEvents;

                return View(googleEventsDisplayModel);
            }
            catch
            {
                return View(new GoogleEventsDisplayModel() {
                       EventDisplayViewModels = null
                }
                );
                throw new Exception();
            }
        }

        /// <summary>
        /// Retrieves the ACTUAL date time of an event
        /// </summary>
        /// <param name="dateTimeRaw">DateTimeRaw string from the event object</param>
        /// <returns>Returns a string date time</returns>
        public DateTime ExtractDateTimeFromDateTimeRaw(string dateTimeRaw)
        {
            TimeZoneInfo timezone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");

            var dateTime = Convert.ToDateTime(dateTimeRaw);
            var dateTimeUTC = dateTime.ToUniversalTime();

            var EST = TimeZoneInfo.ConvertTimeFromUtc(dateTimeUTC, timezone);
            return EST;
        }



        public string CreateJsonFromEventViewModels(Events eventsList)
        {
            var jsonEventObjectsList = new List<RootObject>(); 
            foreach(var eventItem in eventsList.Items)
            {
                var jsonEventObject = new RootObject()
                {
                    Type = "Event",
                    Name = eventItem.Summary,
                    Description = eventItem.Description,
                    Location = ParseLocationFromString(eventItem.Location),
                    Organizer = new Organizer(),
                    EndDate = eventItem.End.DateTimeRaw,
                    //Image = new string[] { eventItem.Attachments[0].FileUrl },
                };

                //Handling null start times
                if (eventItem.Start.DateTimeRaw != null)
                {
                    jsonEventObject.StartDate = eventItem.Start.DateTimeRaw;
                }
                else
                {
                    jsonEventObject.StartDate = eventItem.Start.Date;
                }

                jsonEventObjectsList.Add(jsonEventObject);
            }
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            return JsonConvert.SerializeObject(jsonEventObjectsList,serializerSettings);
        }

        public Location ParseLocationFromString(string stringAddress)
        {
            var location = new Location();
            location.Address = new Address();

            try
            {
                var parsing = stringAddress.Split(',');

                location.Name = parsing[0];
                location.Address.StreetAddress = parsing[1];
                location.Address.AddressLocality = parsing[2].Trim();

                var stateZipParse = parsing[3].Trim().Split(' ');

                location.Address.AddressRegion = stateZipParse[0];
                location.Address.PostalCode = stateZipParse[1];
                //location.Address.AddressCountry = parsing[4];

                return location;
            }
            catch
            {
                return location;
            }
        }
    }
}