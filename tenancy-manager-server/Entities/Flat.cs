using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace tenancy_manager_server.Entities
{
        public class Flat
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [Range(1, 5)]
        public int Number { get; set; }
        public string MPRN_Number { get; set; }

        [ForeignKey("HouseId")]
        public House House { get; set; }
        public List<Lease> Leases { get; set; } = new List<Lease>();

        [NotMapped]
        public Lease CurrentLease
        {
            get
            {
                return Leases.FirstOrDefault(l => l.IsCurrent == true);
            }
        }
    }
}
