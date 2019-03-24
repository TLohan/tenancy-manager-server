using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace tenancy_manager_server.Models
{
    public class PaymentForUpdateDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Amount { get; set; }
        public LeaseDto Lease { get; set; }
    }
}
