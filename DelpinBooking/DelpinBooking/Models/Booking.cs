using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DelpinBooking.Models
{
    public class Booking
    {

        public int Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Pick-up date")]
        public DateTime PickUpDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Return date")]
        public DateTime ReturnDate { get; set; }
        [Display(Name = "Machine type")]
        public string RentType { get; set; }
        [Display(Name = "Warehouse")]
        public string DepartmentStore { get; set; }
        [Display(Name = "Customer ID")]
        public int CustomerID { get; set; }
        [Display(Name = "Phone number")]
        public int PhoneNumber { get; set; }
        [Display(Name = "Customer name")]
        public string CustomerName { get; set; }
        [Display(Name = "Company name")]
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }




    }
}
