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
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class FlatsController : ControllerBase
    {
        public IRepository _repo;

        public FlatsController(IRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("current")]
        public IActionResult GetFlatsWithCurrentLeases()
        {
            var flats = _repo.GetFlatsWithCurrentLeases();
            var flatDtos = Mapper.Map<FlatDto>(flats);
            return Ok(flatDtos);
        }

        [HttpGet("{flatId}")]
        public IActionResult GetFlat(int flatId)
        {
            if (!_repo.FlatExists(flatId))
            {
                return BadRequest();
            }

            var flatEntity = _repo.GetFlat(flatId);
            var flatDto = Mapper.Map<FlatDto>(flatEntity);

            return Ok(flatDto);
        }

        [HttpGet("{flatId}/leases")]
        public IActionResult GetLeasesForFlat(int flatId)
        {
            if (!_repo.FlatExists(flatId))
            {
                return BadRequest();
            }

            var leaseEntities = _repo.GetLeasesForFlat(flatId);
            var leaseDtos = Mapper.Map<IEnumerable<LeaseDto>>(leaseEntities);

            return Ok(leaseDtos);
        }

        [HttpGet("{flatId}/currentLease")]
        public IActionResult getCurrentLeaseForFlat(int flatId)
        {
            if (!_repo.FlatExists(flatId))
            {
                return NotFound();
            }

            var leaseEntity = _repo.GetCurrentLeaseForFlat(flatId);
            var leaseDto = Mapper.Map<LeaseDto>(leaseEntity);

            return Ok(leaseDto);
        }

        [HttpGet("{flatId}/previousLeases")]
        public IActionResult GetPreviousLeases(int flatId)
        {
            if (!_repo.FlatExists(flatId))
            {
                return NotFound();
            }

            var leaseEntities = _repo.GetPreviousLeasesForFlat(flatId);
            var leaseDtos = Mapper.Map<IEnumerable<LeaseDto>>(leaseEntities);

            return Ok(leaseDtos);
        }

        [HttpPost("{flatId}/leases")]
        public IActionResult CreateNewLease(int flatId, [FromBody] LeaseForCreationDto leaseForCreationDto, [FromQuery] bool autoGenRentReview)
        {
            if (!_repo.FlatExists(flatId))
            {
                return NotFound();
            }

            if (leaseForCreationDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var flatEntity = _repo.GetFlat(flatId);

            // update the current lease's isCurrent property
            var currentLease = _repo.GetCurrentLeaseForFlat(flatId);
            if (currentLease != null)
            {
                currentLease.IsCurrent = false;
            }

            var leaseEntity = Mapper.Map<Lease>(leaseForCreationDto);

            leaseEntity.IsCurrent = true;

            if (autoGenRentReview)
            {
                _repo.CreateDefaultRentReviewForNewLease(leaseEntity);
            }

            int startMonth = leaseEntity.StartDate.Month;
            int startYear = leaseEntity.StartDate.Year;

            int thisYear = DateTime.Now.Year;
            int thisMonth = DateTime.Now.Month;

            for (int y = startYear; y <= thisYear; y++)
            {
                for (int m = 1; m <= 12; m++)
                {
                    if (!(y == thisYear && m >= thisMonth) && !(y == startYear && m <= startMonth))
                    {
                        Payment payment = new Payment();
                        payment.Amount = leaseEntity.Rent;
                        payment.Date = new DateTime(y, m, 1);
                        leaseEntity.Payments.Add(payment);
                    }
                }

            }

            _repo.CreateLeaseForFlat(flatId, leaseEntity);

            if (!_repo.Save())
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }

            var leaseDto = Mapper.Map<LeaseDto>(leaseEntity);

            return Ok(leaseDto);

        }
    }
}