using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tenancy_manager_server.Entities;

namespace tenancy_manager_server.Services
{
    public interface IRepository
    {
        IEnumerable<House> GetHouses();
        House GetHouse(string name);

        void CreateHouse(House house);

        Flat GetFlat(int flatId);
        Flat GetFlatByHouse(string houseName, int flatNumber);
        IEnumerable<Flat> GetFlatsWithCurrentLeases();
        IEnumerable<Lease> GetLeasesForFlat(int flatId);
        Lease GetCurrentLeaseForFlat(int flatId);
        IEnumerable<Lease> GetPreviousLeasesForFlat(int flatId);
        void CreateFlatForHouse(House house, Flat flat);

        Lease GetLease(int leaseId);
        Lease GetPlainLease(int leaseId);
        IEnumerable<Lease> GetCurrentLeases();
        Lease GetLeaseWithFlat(int leaseId);
        void CreateDefaultRentReviewForNewLease(Lease lease);
        void CreateDefaultRentReviewForExistingLease(Lease lease);
        void CreateLeaseForFlat(int flatId, Lease lease);
        void UpdateLease(int leaseId, Lease lease);
        IEnumerable<Tenant> GetTenantsForLease(int leaseId);

        IEnumerable<RentReview> GetNextRentReviews();
        RentReview GetRentReview(int reviewId);

        IEnumerable<Payment> GetPayments();
        IEnumerable<Payment> GetPaymentsForLease(int leaseId);
        void AddPaymentToLease(int leaseId, Payment payment);
        void UpdatePayment(Payment payment);

        void UpdateTenant(Tenant tenant);

        bool Save();
        bool HouseExists(string name);
        bool FlatExists(int flatId);
        bool LeaseExists(int leaseId);
        bool TenantExists(int tenantId);
        bool RentReviewExists(int reviewId);
        bool PaymentExists(int paymentId);

        void GetRentsForLast5Years();
    }
}
