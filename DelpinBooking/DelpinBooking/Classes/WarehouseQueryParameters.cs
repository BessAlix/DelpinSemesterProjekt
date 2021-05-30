using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DelpinBooking.Classes
{
    public class WarehouseQueryParameters : QueryParameters
    {
        public int Id { get; set; }
        public string City { get; set; }
        public int PostCode { get; set; }
    }
}
