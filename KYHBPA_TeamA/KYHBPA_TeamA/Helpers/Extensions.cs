using KYHBPA_TeamA.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KYHBPA_TeamA.Helpers
{
    public static class Extensions
    {

            public static string ToConfigLocalTime(this DateTime utcDT)
            {
                var istTZ = TimeZoneInfo.FindSystemTimeZoneById(ConfigurationManager.AppSettings["Timezone"]);
                return String.Format("{0} ({1})", TimeZoneInfo.ConvertTimeFromUtc
                    (utcDT, istTZ).ToShortDateString(), ConfigurationManager.AppSettings["TimezoneAbbr"]);
            }

            public static string Href(this Post post, UrlHelper helper)
            {
                return helper.RouteUrl(new
                {
                    controller = "Blog",
                    action = "Post",
                    year = post.PostedOn.Year,
                    month = post.PostedOn.Month,
                    title = post.UrlSlug
                });

            }
    }
}
