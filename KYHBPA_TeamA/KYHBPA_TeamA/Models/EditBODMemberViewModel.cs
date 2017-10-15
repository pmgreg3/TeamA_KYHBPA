using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KYHBPA_TeamA.Models
{
    public class EditBODMemberViewModel
    {
        public int ID { get; set; }
        public string Title { get; set; }
        [Display(Name = "First Name")]
        public string FirstName { get; set; }
        [Display(Name = "Last Name")]
        public string LastName { get; set; }
        public string Email { get; set; }
        public HttpPostedFileBase File { get; set; } = null;
        public byte[] PhotoContent { get; set; }
    }
}