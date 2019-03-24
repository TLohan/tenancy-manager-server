using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace tenancy_manager_server.Entities
{
    public class House
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }


        public List<Flat> Flats { get; set; } = new List<Flat>();

        [NotMapped]
        public int TotalRentPerMonth
        {
            get
            {
                return Flats.Sum(f => {
                    if (f.CurrentLease != null)
                    {
                        return f.CurrentLease.Rent;
                    }
                    return 0;
                });
            }
        }

        [NotMapped]
        public int TotalRentPaidThisYear
        {
            get
            {
                return Flats.Sum(f =>
                {
                    if (f.CurrentLease != null)
                    {
                        return f.CurrentLease.RentPaidThisYear;
                    }
                    return 0;
                });
            }
        }

    }
}
