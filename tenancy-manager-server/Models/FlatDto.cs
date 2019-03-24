using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tenancy_manager_server.Models
{
    public class FlatDto
    {
        public int Id { get; set; }
        [Required]
        [Range(1, 5)]
        public int Number { get; set; }
        public string MPRN_Number { get; set; }

        public HouseDto House { get; set; }
        public IEnumerable<LeaseDto> Leases { get; set; }
        public LeaseDto CurrentLease { get; set; }
    }
}
