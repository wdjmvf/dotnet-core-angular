using System;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using vega.Controllers.Resources;
using vega.Models;
using vega.Core;
using System.Collections.Generic;
using vega.Core.Models;
using Microsoft.AspNetCore.Authorization;

namespace vega.Controllers
{
    [Route("/api/vehicles")]
    public class VehiclesController : Controller
    {
        private readonly IMapper mapper;
        private readonly IVehicleRepository repository;
        private readonly IUnitOfWork unitOfWork;

        public VehiclesController(IMapper mapper, IVehicleRepository repository, IUnitOfWork unitOfWork)
        {
            this.mapper = mapper;
            this.repository = repository;
            this.unitOfWork = unitOfWork;
        }
        
        [HttpGet]
        public async Task<QueryResult<VehicleResource>> GetVehicles(VehicleQueryResource filterResource){
            var filter = mapper.Map<VehicleQueryResource,VehicleQuery>(filterResource);
            var queryResult = await repository.GetVehicles(filter);
            return mapper.Map<QueryResult<Vehicle>, QueryResult<VehicleResource>>(queryResult);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateVehicle([FromBody] SaveVehicleResource vehicleResource)
        {
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var model = await repository.GetVehicle(vehicleResource.ModelId);
            if(model ==null){
                ModelState.AddModelError("ModelId","Invalid model Id");
                return BadRequest(ModelState);
            }

            var vehicle = mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource);
            vehicle.LastUpdate = DateTime.Now;

            repository.Add(vehicle);
            await unitOfWork.CompleteAsync();

            vehicle = await repository.GetVehicle(vehicle.Id);

            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateVehicle(int id, [FromBody] SaveVehicleResource vehicleResource)
        {
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }

            var vehicle = await repository.GetVehicle(id);

            if(vehicle == null){
                return NotFound();
            }
            mapper.Map<SaveVehicleResource, Vehicle>(vehicleResource, vehicle);
            vehicle.LastUpdate = DateTime.Now;

            await unitOfWork.CompleteAsync();

            vehicle = await repository.GetVehicle(vehicle.Id);
            var result = mapper.Map<Vehicle, VehicleResource>(vehicle);

            return Ok(result);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteVehicle(int id)
        {
            var vehicle = await repository.GetVehicle(id);
            if(vehicle == null){
                return NotFound();
            }

            repository.Remove(vehicle);
            await unitOfWork.CompleteAsync();
            
            return Ok(id);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetVehicle(int id)
        {
            var vehicle = await repository.GetVehicle(id, includeRelated: true);
            
            if(vehicle == null){
                return NotFound();
            }
            var vehicleResouce = mapper.Map<Vehicle, VehicleResource>(vehicle);
            return Ok(vehicleResouce);

        }
    }
}