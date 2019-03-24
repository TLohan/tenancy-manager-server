using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tenancy_manager_server.Models
{
    public class PaymentDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public LeaseDto Lease { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
    }
}
