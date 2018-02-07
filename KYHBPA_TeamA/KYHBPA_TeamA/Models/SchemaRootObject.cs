using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KYHBPA_TeamA.Models
{
    public class RootObject
    {
        [JsonProperty("@context")]
        public string Context { get { return "http://schema.org"; } }

        [JsonProperty("@type")]
        public string Type { get; set; }
        public string Name { get; set; }
        public string StartDate { get; set; }
        public Location Location { get; set; }
        public string[] Image { get; set; }
        public string Description { get; set; }
        public string EndDate { get; set; }
        public Organizer Organizer { get; set; }
    }
    
    public class Location
    {
        [JsonProperty("@type")]
        public string Type { get { return "Place"; } }
        public string Name { get; set; }
        public Address Address { get; set; }
    }
    
    public class Address
    {
        [JsonProperty("@type")]
        public string Type { get { return "PostalAddress"; } }
        public string StreetAddress { get; set; }
        public string AddressLocality { get; set; }
        public string PostalCode { get; set; }
        public string AddressRegion { get; set; }
        public string AddressCountry { get; set; }
    }

    public class Organizer
    {
        [JsonProperty("@type")]
        public string Type { get { return "Organization"; } }
        public string LegalName { get; set; } = "Kentucky Horsemen's Benevolent and Protective Association, INC";
    }
}
