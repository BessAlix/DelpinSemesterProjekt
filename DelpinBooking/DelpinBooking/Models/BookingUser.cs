using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DelpinBooking.Models
{
    public class BookingUser : IdentityUser
    {
        [PersonalData]
        public string FullName { get; set; }
    }
}
