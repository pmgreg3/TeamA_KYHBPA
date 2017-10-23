using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace KYHBPA_TeamA.Models
{
    public class MembershipsViewModels
    {
        public class CreateMembershipViewModel
        {
            public bool isTrue
            { get { return true; } }


            public bool iAgree { get; set; }

            [Required]
            [Display(Name = "Date of birth")]
            public DateTime DateofBirth { get; set; }
            [Required]
            [Display(Name = "Phone Number")]
            public string PhoneNumber { get; set; }
            [Required]
            public string Address { get; set; }
            [Required]
            public string City { get; set; }
            [Required]
            public string State { get; set; }
            [Required]
            public string ZipCode { get; set; }
            [Required]
            public string LicenseNumber { get; set; }

            public bool IsOwner { get; set; }
            public bool IsTrainer { get; set; }
            public string Affiliation { get; set; }
            [Display(Name = "Managing Partner")]
            public string ManagingPartner { get; set; }

            [Required]
            [Display(Name = "I agree to the terms and conditions")]
            [Compare("isTrue", ErrorMessage = "Please agree to Terms and Conditions")]
            public bool AgreedToTerms { get; set; }
            [Required]
            public string Signature { get; set; }
        }
    }
}