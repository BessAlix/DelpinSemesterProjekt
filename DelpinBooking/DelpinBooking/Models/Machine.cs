using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DelpinBooking.Models
{
    public class Machine
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        [Display(Name = "Modelnavn")]
        public string Name { get; set; }
        [Display(Name = "Kategori")]
        public string Type { get; set; }
        [Display(Name = "Varehus")]
        public Warehouse Warehouse { get; set; }
    }
}
