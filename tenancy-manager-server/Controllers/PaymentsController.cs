using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tenancy_manager_server.Entities;
using tenancy_manager_server.Models;
using tenancy_manager_server.Services;

namespace tenancy_manager_server.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        IRepository _repo;

        public PaymentsController(IRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("AutoUpdateRentPayments")]
        public IActionResult AutoUpdateRentPayments()
        {
            var currentLeaseEntities = _repo.GetCurrentLeases();
            List<Payment> payments = new List<Payment>();

            foreach (var lease in currentLeaseEntities)
            {
                var rent = lease.Rent;
                Payment payment = new Payment();
                payment.Amount = rent;
                payment.Date = DateTime.Now;
                payments.Add(payment);
                lease.Payments.Add(payment);
            }

            _repo.Save();

            return Ok(payments);
        }

        [HttpGet("{leaseId}")]
        public IActionResult GetPaymentsForLease(int leaseId)
        {
            if (!_repo.LeaseExists(leaseId))
            {
                return NotFound();
            }

            var payments = _repo.GetPaymentsForLease(leaseId);

            var paymentDtos = Mapper.Map<IEnumerable<PaymentDto>>(payments);

            return Ok(paymentDtos);
        }

        [HttpPost("{paymentId}")]
        public IActionResult SavePayment(int paymentId, [FromBody] PaymentForUpdateDto paymentForUpdate)
        {

            if (!_repo.PaymentExists(paymentId))
            {
                if (!_repo.LeaseExists(paymentForUpdate.Lease.Id))
                {
                    return NotFound();
                }
                else
                {
                    Payment payment = Mapper.Map<Payment>(paymentForUpdate);
                    _repo.AddPaymentToLease(paymentForUpdate.Lease.Id, payment);

                }
            }
            else
            {
                Payment paymentEntity = Mapper.Map<Payment>(paymentForUpdate);
                _repo.UpdatePayment(paymentEntity);
            }

            if (!_repo.Save())
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }

            IEnumerable<Payment> payments = _repo.GetPaymentsForLease(paymentForUpdate.Lease.Id);

            var paymentDtos = Mapper.Map<IEnumerable<PaymentDto>>(payments);

            return Ok(paymentDtos);
        }

        [HttpGet("GetAnnualIncomeForLastFiveYears")]
        public IActionResult GetAnnualIncomeForLastFiveYears()
        {
            List<int> result = new List<int>(5);
            List<Payment> payments = _repo.GetPayments().ToList();
            int thisYear = DateTime.Now.Year;
            for (int i = 5; i > 0; i--)
            {
                int year = thisYear - i;
                List<Payment> paymentsForYear = payments.Where(p => p.Year == year).ToList();
                result.Add(paymentsForYear.Sum(p => { return p.Amount;  }));
            }
            return Ok(result);
        }

        [HttpGet("IncomeForEachHouseThisYear")]
        public IActionResult GetIncomeForEachHouseThisYear()
        {
            int thisYear = DateTime.Now.Year;
            List<Payment> payments = _repo.GetPayments().Where(p => p.Year == thisYear).ToList();
            List<House> houses = _repo.GetHouses().ToList();
            Dictionary<string, int> result = new Dictionary<string, int>();
            houses.ForEach(house =>
            {
                int totalIncome = payments.Sum(p =>
                {
                    if (p.Lease.Flat.House.Equals(house))
                    {
                        return p.Amount;
                    }
                    return 0;
                });
                result.Add(house.Name, totalIncome);
            });
            return Ok(result);
        }
    }
}