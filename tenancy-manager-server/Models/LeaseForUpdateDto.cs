using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tenancy_manager_server.Models
{
    public class LeaseForUpdateDto
    {
        public int Rent { get; set; }
        public int Deposit { get; set; }
        public int RTB_Number { get; set; }
        public bool IsCurrent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public FlatDto Flat { get; set; }
    }
}
