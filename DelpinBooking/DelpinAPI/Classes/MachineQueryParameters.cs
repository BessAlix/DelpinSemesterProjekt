using DelpinAPI.APIModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DelpinAPI.Classes
{
    public class MachineQueryParameters : QueryParameters
    {
     
        public int Id { get; set; }

        public string Name { get; set; }

        public string Type { get; set; }

        public string WarehouseCity { get; set; }

    }
}
