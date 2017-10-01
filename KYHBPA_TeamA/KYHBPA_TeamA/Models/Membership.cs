using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KYHBPA_TeamA.Models
{
    public class Membership
    {

        public string MemberId { get; set; }
        public bool IsApproved { get; set; }
        public DateTime MembershipEnrollmentDate { get; set; }
        public string LicenseNumber { get; set; }
        public bool IsOwner { get; set; }
        public bool IsTrainer { get; set; }
        public bool IsOwnerAndTrainer { get; set; }
        public string StableName { get; set; }
        public bool AgreedToTerms { get; set; }
        public string Signature { get; set; }
        public string Affiliation { get; set; }
        public string ManagingPartner { get; set; }
        

    }
}