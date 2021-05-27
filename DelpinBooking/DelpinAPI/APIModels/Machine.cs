using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DelpinAPI.APIModels
{
    public class Machine
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        
        public string Name { get; set; }
        
        public string Type { get; set; }
        
        public Warehouse Warehouse { get; set; }

        public Booking Booking { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
