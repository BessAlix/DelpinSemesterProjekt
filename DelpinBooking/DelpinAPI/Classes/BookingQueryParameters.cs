using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DelpinAPI.APIModels;

namespace DelpinAPI.Classes
{
    public class BookingQueryParameters : QueryParameters
    {
        public int Id { get; set; }
        public List<Machine> Machines { get; set; }
        public DateTime PickUpDate { get; set; }
        public DateTime ReturnDate { get; set; }
        public string Customer { get; set; }
    }
}
