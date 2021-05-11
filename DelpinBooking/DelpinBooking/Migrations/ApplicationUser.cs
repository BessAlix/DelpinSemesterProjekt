using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DelpinBooking.Migrations
{
    public class ApplicationUser : IdentityUser
    { 
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CompanyName { get; set; }
        public string CPR { get; set; }
        public string CVR { get; set; }
        public string LeaderName { get; set; }
        public string Address { get; set; }
        public int PostCode { get; set; }
        public string City { get; set; }
    }
}
