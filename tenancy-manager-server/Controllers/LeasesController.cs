using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using tenancy_manager_server.Models;
using tenancy_manager_server.Services;

namespace tenancy_manager_server.Controllers
{
    [Produces("application/json")]
    [EnableCors("CorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class LeasesController : ControllerBase
    {
        private IRepository _repo;

        public LeasesController(IRepository repo)
        {
            this._repo = repo;
        }

        [HttpGet("current")]
        public IActionResult GetCurrentLeases()
        {
            var currentLeaseEntities = _repo.GetCurrentLeases();
            var currentLeaseDtos = Mapper.Map<IEnumerable<LeaseDto>>(currentLeaseEntities);
            return Ok(currentLeaseDtos);
        }

        [HttpGet("{leaseId}")]
        public IActionResult GetLease(int leaseId)
        {
            if (!_repo.LeaseExists(leaseId))
            {
                return NotFound();
            }

            var leaseEntity = _repo.GetLease(leaseId);
            var leaseDto = Mapper.Map<LeaseDto>(leaseEntity);

            return Ok(leaseDto);
        }

        [HttpPatch("{leaseId}")]
        public IActionResult UpdateLease(int leaseId, [FromBody] JsonPatchDocument<LeaseForUpdateDto> patchDocument)
        {
            if (!_repo.LeaseExists(leaseId))
            {
                return NotFound();
            }

            if (patchDocument == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var leaseEntity = _repo.GetPlainLease(leaseId);

            var leaseUpdateDto = Mapper.Map<LeaseForUpdateDto>(leaseEntity);

            patchDocument.ApplyTo(leaseUpdateDto, ModelState);

            TryValidateModel(leaseUpdateDto);

            leaseEntity = Mapper.Map(leaseUpdateDto, leaseEntity);
            leaseUpdateDto = null;
            if (!_repo.Save())
            {
                return StatusCode(500, "An error has occurred while processing your request.");
            }

            leaseEntity = _repo.GetLeaseWithFlat(leaseEntity.Id);
            var previousLeaseEntities = _repo.GetPreviousLeasesForFlat(leaseEntity.Flat.Id);

            var previousLeaseDtos = Mapper.Map<IEnumerable<LeaseDto>>(previousLeaseEntities);

            return Ok(previousLeaseDtos);

        }
    }
}