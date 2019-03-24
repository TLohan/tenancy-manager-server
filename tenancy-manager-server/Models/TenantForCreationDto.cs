using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tenancy_manager_server.Models
{
    public class TenantForCreationDto
    {
        [Required]
        [MinLength(2)]
        public string FirstName { get; set; }
        [MinLength(2)]
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Employment { get; set; }
        public int LeaseId { get; set; }
    }
}
