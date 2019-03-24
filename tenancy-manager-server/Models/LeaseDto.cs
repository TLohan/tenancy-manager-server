using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace tenancy_manager_server.Models
{
    public class LeaseDto
    {
        public int Id { get; set; }
        [Required]
        public int Rent { get; set; }
        [Required]
        public int Deposit { get; set; }
        public string RTB_Number { get; set; }
        public bool IsCurrent { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public FlatDto Flat { get; set; }
        public ICollection<TenantDto> Tenants { get; set; }
        public IEnumerable<RentReviewDto> RentReviews { get; set; }
        public IEnumerable<PaymentDto> Payments { get; set; }
        public RentReviewDto NextRentReview { get; set; }
        public decimal RentPaidThisYear { get; set; }
    }
}
