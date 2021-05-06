using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DelpinBooking.Models
{
    public class Machine
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public Warehouse Warehouse { get; set; }
    }
}
