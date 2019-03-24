using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace tenancy_manager_server.Entities
{
    public class RentReview
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public DateTime TakesEffectOn { get; set; }
        public DateTime ServeNoticeBy { get; set; }
        public decimal Percent { get; set; }
        public bool IsNext { get; set; }
        public bool NoticeHasBeenServed { get; set; }
        public bool IsInEffect { get; set; }
        public Lease Lease { get; set; }
        public int PreviousRent { get; set; }

        [NotMapped]
        public decimal ReviewedRent
        {
            get
            {
                return PreviousRent * (1 + Percent / 100);
            }
        }
    }
}
