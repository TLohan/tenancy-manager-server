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
    public class TenantsController : ControllerBase
    {
        private IRepository _repo;

        public TenantsController(IRepository repo)
        {
            _repo = repo;
        }


        [HttpPost("{tenantId}")]
        public IActionResult UpdateTenant(int tenantId, [FromBody] TenantForUpdateDto tenantForUpdateDto)
        {
            if (!_repo.TenantExists(tenantId))
            {
                return NotFound();
            }

            if (tenantForUpdateDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var tenantEntity = Mapper.Map<Tenant>(tenantForUpdateDto);

            _repo.UpdateTenant(tenantEntity);

            if (!_repo.Save())
            {
                return StatusCode(500, "An error occurred while processing you request.");
            }

            var tenantDto = Mapper.Map<TenantDto>(tenantEntity);

            return CreatedAtRoute(tenantId, tenantDto);
        }
    }
}