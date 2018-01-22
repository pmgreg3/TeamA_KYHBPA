using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KYHBPA_TeamA.Models
{
    public class CreateCommentViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        [DataType(DataType.MultilineText)]
        public string Comment { get; set; }
        public int PostNumber { get; set; }
        public virtual Post Post { get; set; }

    }
}