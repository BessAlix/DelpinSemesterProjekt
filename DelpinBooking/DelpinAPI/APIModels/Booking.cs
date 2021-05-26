using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DelpinAPI.APIModels
{
    public class Booking
    {
        public int Id { get; set; }
        public List<Machine> Machines { get; set; }
        [DataType(DataType.Date)]
        public DateTime PickUpDate { get; set; }
        [DataType(DataType.Date)]
        public DateTime ReturnDate { get; set; }
        public string Customer { get; set; }
        public bool SoftDeleted { get; set; }
        
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
