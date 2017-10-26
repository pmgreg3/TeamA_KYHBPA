using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KYHBPA_TeamA.Models
{
    public class BlogListViewModel
    {
        public IList<Post> Posts { get; set; }

        public int TotalPosts { get; set; }

    }
}