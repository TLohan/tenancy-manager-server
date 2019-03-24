using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace tenancy_manager_server.Entities
{
    public class Lease
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Rent { get; set; }
        [Required]
        public int Deposit { get; set; }
        public string RTB_Number { get; set; }
        public bool IsCurrent { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [ForeignKey("FlatId")]
        public Flat Flat { get; set; }
        public ICollection<Tenant> Tenants { get; set; } = new List<Tenant>();
        public List<RentReview> RentReviews { get; set; } = new List<RentReview>();
        public ICollection<Payment> Payments { get; set; } = new List<Payment>();

        [NotMapped]
        public RentReview NextRentReview
        {
            get
            {
                return RentReviews.FirstOrDefault(rr => rr.IsNext == true);
            }
        }

        [NotMapped]
        public int RentPaidThisYear
        {
            get
            {
                return Payments.Where(p => p.Year == DateTime.Now.Year).Sum(p => p.Amount);
            }
        }
    }
}
