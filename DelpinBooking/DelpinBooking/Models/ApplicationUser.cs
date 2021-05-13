using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DelpinBooking.Migrations
{
    public class ApplicationUser : IdentityUser
    {
        [Display(Name = "Fornavn")]
        public string FirstName { get; set; }
        [Display(Name = "Efternavn")]
        public string LastName { get; set; }
        [Display(Name = "Selskabsform")]
        public string CompanyForm { get; set; }
        [Display(Name = "Firmanavn")]
        public string CompanyName { get; set; }
        [Display(Name = "Ledernavn")]
        public string LeaderName { get; set; }
        public string CPR { get; set; }
        public string CVR { get; set; }
        [Display(Name = "Adresse")]
        public string Address { get; set; }
        [Display(Name = "Postnummer")]
        public int PostCode { get; set; }
        [Display(Name = "By")]
        public string City { get; set; }
        [Display(Name = "Telefonnummer")]
        public override string PhoneNumber { get; set; }
    }
}
