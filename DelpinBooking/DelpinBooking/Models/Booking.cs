using System;
using System.Collections.Generic;
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
        public int PickUpDate { get; set; }
        public int ReturnDate { get; set; }
        public string RentType { get; set; }
        public string DepartmentStore { get; set; }
        public decimal PricePrDay { get; set; }
        public int CustomerID { get; set; }
        public int PhoneNumber { get; set; }
        public string CustomerName { get; set; }
        public string CompanyName { get; set; }
        public string Address { get; set; }
        public string City { get; set; }




    }
}
