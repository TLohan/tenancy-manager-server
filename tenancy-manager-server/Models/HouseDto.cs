using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tenancy_manager_server.Models
{
    public class HouseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public IEnumerable<FlatDto> Flats { get; set; }
        public int TotalRentPerMonth { get; set; }
        public int TotalRentPaidThisYear { get; set; }
    }
}
