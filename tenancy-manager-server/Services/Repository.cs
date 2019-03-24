using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using tenancy_manager_server.Entities;

namespace tenancy_manager_server.Services
{
    public class Repository: IRepository
    {
        private AppDbContext _db { get; set; }

        public Repository(AppDbContext db)
        {
            _db = db;
        }

        public void CreateLeaseForFlat(int flatId, Lease lease)
        {
            Flat flat = _db.Flats.Where(f => f.Id == flatId).FirstOrDefault();
            flat.Leases.Add(lease);
        }

        public void CreateDefaultRentReviewForNewLease(Lease lease)
        {
            RentReview defaultRentReview = new RentReview()
            {
                TakesEffectOn = lease.StartDate.AddYears(2),
                ServeNoticeBy = lease.StartDate.AddMonths(21),
                Percent = 4,
                IsNext = true,
                NoticeHasBeenServed = false,
                IsInEffect = false,
                PreviousRent = lease.Rent
            };
            lease.RentReviews.Add(defaultRentReview);
        }

        public void CreateDefaultRentReviewForExistingLease(Lease lease)
        {
            var lastRentReview = lease.RentReviews.LastOrDefault();
            RentReview defaultRentReview = new RentReview()
            {
                TakesEffectOn = lastRentReview.TakesEffectOn.AddYears(2),
                ServeNoticeBy = lastRentReview.TakesEffectOn.AddMonths(21),
                Percent = 4,
                IsNext = true,
                NoticeHasBeenServed = false,
                IsInEffect = false,
                PreviousRent = lease.Rent
            };
            lease.RentReviews.Add(defaultRentReview);
        }

        public bool FlatExists(int flatId)
        {
            return _db.Flats.Any(f => f.Id == flatId);
        }

        public Lease GetCurrentLeaseForFlat(int flatId)
        {
            return _db.Leases.Where(l => (l.IsCurrent == true && l.Flat.Id == flatId)).Include(l => l.Tenants).Include(l => l.RentReviews).FirstOrDefault();
        }

        public House GetHouse(string name)
        {
            return _db.Houses.Where(h => h.Name.ToLower() == name.ToLower()).Include(h => h.Flats).FirstOrDefault();
        }

        public IEnumerable<House> GetHouses()
        {
            return _db.Houses.Include("Flats.Leases.Payments").ToList();
        }

        public IEnumerable<Lease> GetLeasesForFlat(int flatId)
        {
            Flat flat = _db.Flats.Where(f => f.Id == flatId).FirstOrDefault();
            if (flat.Leases != null)
            {
                return flat.Leases.ToList();
            }
            return null;
        }

        public IEnumerable<Tenant> GetTenantsForLease(int leaseId)
        {
            return _db.Leases.Where(l => l.Id == leaseId).FirstOrDefault().Tenants.ToList();
        }

        public bool HouseExists(string name)
        {
            return _db.Houses.Any(h => h.Name.ToLower() == name.ToLower());
        }

        public bool LeaseExists(int leaseId)
        {
            return _db.Leases.Any(l => l.Id == leaseId);
        }

        public bool TenantExists(int tenantId)
        {
            return _db.Tenants.Any(t => t.Id == tenantId);
        }

        public void UpdateLease(int leaseId, Lease lease)
        {

        }

        public void UpdateTenant(Tenant tenant)
        {
            _db.Tenants.Update(tenant);

        }

        public bool Save()
        {
            return _db.SaveChanges() >= 0;
        }

        public IEnumerable<Flat> GetFlatsWithCurrentLeases()
        {
            return _db.Flats.Include(f => f.CurrentLease).ToList();
        }

        public void CreateHouse(House house)
        {
            _db.Houses.Add(house);
        }

        public Flat GetFlat(int flatId)
        {
            return _db.Flats.Where(f => f.Id == flatId).Include(f => f.Leases).FirstOrDefault();
        }

        public void CreateFlatForHouse(House house, Flat flat)
        {
            _db.Houses.Where(h => h.Id == house.Id).FirstOrDefault().Flats.Add(flat);
        }

        public Flat GetFlatByHouse(string houseName, int flatNumber)
        {
            return _db.Flats.Where(f => f.House.Name == houseName && f.Number == flatNumber).Include(f => f.House).Include(f => f.Leases).FirstOrDefault();
        }

        public IEnumerable<Lease> GetPreviousLeasesForFlat(int flatId)
        {
            return _db.Leases.Where(l => l.Flat.Id == flatId && l.IsCurrent != true).Include(l => l.Tenants).ToList();
        }

        public Lease GetLease(int leaseId)
        {
            return _db.Leases
                .Where(l => l.Id == leaseId)
                .Include(l => l.Tenants)
                .Include(l => l.Flat.House)
                .Include(l => l.RentReviews)
                .Include(l => l.Payments)
                .FirstOrDefault();
        }

        public Lease GetLeaseWithFlat(int leaseId)
        {
            return _db.Leases.Where(l => l.Id == leaseId).Include(l => l.Flat).FirstOrDefault();
        }

        public IEnumerable<Lease> GetCurrentLeases()
        {
            return _db.Leases.
                Where(l => l.IsCurrent == true)
                .Include(l => l.Flat.House)
                .Include(l => l.Tenants)
                .Include(l => l.Payments)
                .OrderBy(l => l.Flat.House.Name)
                .ThenBy(l => l.Flat.Number)
                .ToList();
        }

        public IEnumerable<RentReview> GetNextRentReviews()
        {
            return _db.RentReviews.Where(rr => rr.IsNext == true).Include(rr => rr.Lease.Flat.House).OrderBy(rr => rr.TakesEffectOn).ToList();
        }

        public bool RentReviewExists(int reviewId)
        {
            return _db.RentReviews.Any(r => r.Id == reviewId);
        }

        public RentReview GetRentReview(int reviewId)
        {
            return _db.RentReviews.Where(r => r.Id == reviewId).Include(r => r.Lease).FirstOrDefault();
        }

        public Lease GetPlainLease(int leaseId)
        {
            return _db.Leases.FirstOrDefault(l => l.Id == leaseId);
        }

        public void GetRentsForLast5Years()
        {

        }

        public IEnumerable<Payment> GetPaymentsForLease(int leaseId)
        {
            return _db.Payments.Where(p => p.Lease.Id == leaseId).ToList();
        }

        public bool PaymentExists(int paymentId)
        {
            return _db.Payments.Any(p => p.Id == paymentId);
        }

        public void UpdatePayment(Payment payment)
        {
            Payment savedPayment = _db.Payments.Where(p => p.Id == payment.Id).FirstOrDefault();
            savedPayment.Amount = payment.Amount;
        }

        public void AddPaymentToLease(int leaseId, Payment payment)
        {
            Lease lease = GetLease(leaseId);
            lease.Payments.Add(payment);
        }

        public IEnumerable<Payment> GetPayments()
        {
            return _db.Payments;
        }
    }
}
