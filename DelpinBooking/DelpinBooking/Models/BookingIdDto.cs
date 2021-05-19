using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DelpinBooking.Models
{
    public class BookingIdDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
