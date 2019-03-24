using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using tenancy_manager_server.Models;
using tenancy_manager_server.Services;

namespace tenancy_manager_server.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    [ApiController]
    public class RentReviewsController : ControllerBase
    {
        private IRepository _repo;

        public RentReviewsController(IRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public IActionResult GetNextRentReviews()
        {
            var rentReviewEntities = _repo.GetNextRentReviews();
            var rentReviewDtos = Mapper.Map<IEnumerable<RentReviewDto>>(rentReviewEntities);
            return Ok(rentReviewDtos);
        }

        [HttpGet("{reviewId}/execute")]
        public IActionResult ExecuteReview(int reviewId)
        {
            if (!_repo.RentReviewExists(reviewId))
            {
                return NotFound();
            }

            var reviewEntity = _repo.GetRentReview(reviewId);

            reviewEntity.IsInEffect = true;
            reviewEntity.IsNext = false;
            reviewEntity.NoticeHasBeenServed = true;

            var leaseEntity = _repo.GetLease(reviewEntity.Lease.Id);

            leaseEntity.Rent = (int)reviewEntity.ReviewedRent;
            _repo.CreateDefaultRentReviewForExistingLease(leaseEntity);

            if (!_repo.Save())
            {
                return StatusCode(500, "An error occurred when processing your request.");
            }

            var leaseDto = Mapper.Map<LeaseDto>(leaseEntity);

            return Ok(leaseDto);

        }
    }
}