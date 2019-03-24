using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tenancy_manager_server.Models
{
    public class LeaseForCreationDto
    {
        [Required]
        public int Rent { get; set; }

        [Required]
        public int Deposit { get; set; }

        public string RTB_Number { get; set; }
        public DateTime StartDate { get; set; }

        public int FlatId { get; set; }
        public IEnumerable<TenantForCreationDto> Tenants { get; set; }
        public IEnumerable<PaymentDto> Payments { get; set; }
    }
}
