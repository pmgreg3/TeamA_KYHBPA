using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KYHBPA_TeamA.Models
{
    public class AddPhotoViewModel
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public HttpPostedFileBase Image { get; set; } = null;
    }

    public class DisplayPhotosViewModel
    {
        public byte[] Data { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public DateTime Date { get; set; }
    }
}