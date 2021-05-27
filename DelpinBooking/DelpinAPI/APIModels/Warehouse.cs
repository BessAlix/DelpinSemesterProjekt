using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace DelpinAPI.APIModels
{
    public class Warehouse
    {
        public int Id { get; set; }
        public string City { get; set; }
        public int PostCode { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
