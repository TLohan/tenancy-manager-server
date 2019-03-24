using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace tenancy_manager_server.Entities
{
    public class Payment
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int Amount { get; set; }

        [ForeignKey("LeaseId")]
        public Lease Lease { get; set; }

        [NotMapped]
        public int Month
        {
            get
            {
                return Date.Month;
            }
        }

        [NotMapped]
        public int Year
        {
            get
            {
                return Date.Year;
            }
        }
    }
}
