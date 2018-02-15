using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KYHBPA_TeamA.Models
{
    public class GoogleEventsDisplayModel
    {
        public List<EventDisplayViewModel> EventDisplayViewModels { get; set; }
        public string SchemaJson { get; set; }
    }

    public class EventDisplayViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string StartDate { get; set; }
        public DateTime StartTime { get; set; }
        public string EndDate { get; set; } = null;
        public string EndTime { get; set; } = null;
        public string Link { get; set; }
        public string Location { get; set; }
        public int IdForElement { get; set; }
        public string Url { get; set; }

    }
}