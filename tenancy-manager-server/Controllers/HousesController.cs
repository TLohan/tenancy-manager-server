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
    [Route("api/[controller]")]
    [EnableCors("CorsPolicy")]
    [ApiController]
    public class HousesController : ControllerBase
    {
        private IRepository _repo;

        public HousesController(IRepository repo)
        {
            _repo = repo;
        }


        [HttpGet]
        public IActionResult GetHouses()
        {
            var houseEntities = _repo.GetHouses();
            var houseDtos = Mapper.Map<IEnumerable<HouseDto>>(houseEntities);
            return Ok(houseDtos);
        }

        [HttpGet("{houseName}", Name = "GetHouse")]
        public IActionResult GetHouse(string houseName)
        {
            if (!_repo.HouseExists(houseName))
            {
                return NotFound();
            }

            var houseEntity = _repo.GetHouse(houseName);
            var houseDto = Mapper.Map<HouseDto>(houseEntity);
            return Ok(houseDto);
        }

        [HttpGet("{houseName}/flats/{flatNumber}")]
        public IActionResult GetFlat(string houseName, int flatNumber)
        {
            if (!_repo.HouseExists(houseName))
            {
                return NotFound();
            }

            var flat = _repo.GetFlatByHouse(houseName, flatNumber);

            if (flat == null)
            {
                return NotFound();
            }

            var flatDto = Mapper.Map<FlatDto>(flat);

            return Ok(flatDto);
        }

        [HttpPost]
        public IActionResult CreateHouse([FromBody] HouseForCreationDto houseForCreation)
        {
            if (houseForCreation == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var houseEntity = Mapper.Map<House>(houseForCreation);
            _repo.CreateHouse(houseEntity);

            if (!_repo.Save())
            {
                return StatusCode(500, "An error occurred while processing your request!");
            }

            var houseDto = Mapper.Map<HouseDto>(houseEntity);

            return CreatedAtRoute("GetHouse", new { houseName = houseEntity.Name }, houseDto);
        }

        [HttpPost("{houseName}/flats")]
        public IActionResult CreateFlatForHouse(string houseName, [FromBody] FlatForCreationDto flatForCreationDto)
        {
            if (!_repo.HouseExists(houseName))
            {
                return NotFound();
            }

            if (flatForCreationDto == null)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var houseEntity = _repo.GetHouse(houseName);

            var flatEntity = Mapper.Map<Flat>(flatForCreationDto);

            _repo.CreateFlatForHouse(houseEntity, flatEntity);

            if (!_repo.Save())
            {
                return StatusCode(500, "An error occurred while processing your request.");
            }

            return Ok(houseEntity);
        }
    }
}