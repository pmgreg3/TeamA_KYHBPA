using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace KYHBPA_TeamA.Helpers
{
    public class Constants
    {
        public static readonly SmtpClient ReleaseSmtpClient = new SmtpClient()
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            UseDefaultCredentials = false,

        };

        public static readonly SmtpClient DebugSmtpClient = new SmtpClient()
        {
            EnableSsl = false,
            Host = "relay-hosting.secureserver.net",
            Port = 25
        };
    }
}