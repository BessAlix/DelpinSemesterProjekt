using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DelpinBooking.Models
{
    public class Warehouse
    {
        public int Id { get; set; }
        [Display(Name = "By")]
        public string City { get; set; }
        [Display(Name = "Postnummer")]
        public int PostCode { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

    }
}
