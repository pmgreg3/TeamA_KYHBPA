using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KYHBPA_TeamA.Models
{
    public class AddDocumentViewModel
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public HttpPostedFileBase File { get; set; } = null;
    }

    public class DisplayDocumentsViewModel
    {
        public int Id { get; set; }
        public byte[] Content { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }

    }
}