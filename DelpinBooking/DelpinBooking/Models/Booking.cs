using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using DelpinBooking.Migrations;

namespace DelpinBooking.Models
{
    public class Booking
    {

        public int Id { get; set; }
        [Display(Name = "Varenummer")]
        public Machine Machine { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Afhentningsdato")]
        public DateTime PickUpDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Afleveringsdato")]
        public DateTime ReturnDate { get; set; }

        public bool SoftDeleted { get; set; }
        public string Customer { get; set; } // der skal laves API




    }
}
