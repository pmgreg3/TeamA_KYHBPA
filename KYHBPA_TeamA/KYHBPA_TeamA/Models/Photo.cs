using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KYHBPA_TeamA.Models
{
    public class Photo
    {
        public int PhotoID { get; set; }
        public string PhotoTitle { get; set; }
        public string PhotoDesc { get; set; }
        public DateTime TimeStamp { get; set; }
        public byte[] PhotoData { get; set; }

    }
}