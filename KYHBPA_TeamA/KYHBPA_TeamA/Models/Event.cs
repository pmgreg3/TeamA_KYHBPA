using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KYHBPA_TeamA.Models
{
    public class Event // event table
    {
        public int eventID { get; set; }
        public string EventName { get; set; }
        public DateTime EventTime { get; set; }
        public DateTime EventDate { get; set; } 
        public string EventDescription { get; set; }
        public string EventLocation { get; set; }

    }
}