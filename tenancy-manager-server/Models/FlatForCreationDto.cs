using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tenancy_manager_server.Models
{
    public class FlatForCreationDto
    {
        [Required]
        public int Number { get; set; }
        public string MPRN_Number { get; set; }
    }
}
