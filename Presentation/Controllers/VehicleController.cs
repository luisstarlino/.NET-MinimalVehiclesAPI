using _NET_MinimalAPI.Domain.DTOs;
using _NET_MinimalAPI.Domain.Entities;
using _NET_MinimalAPI.Domain.Interfaces;
using _NET_MinimalAPI.Domain.ModelViews;
using _NET_MinimalAPI.Domain.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace _NET_MinimalAPI.Presentation.Controllers
{
    
    [ApiController]
    [Route("[controller]")]
    public class VehicleController : Controller
    {
        private readonly IVehicleService _vehicleService;
        public VehicleController(IVehicleService vehicleService)
        {
            this._vehicleService = vehicleService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="vehicleDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize(Roles = "adm,editor")]
        public IActionResult CreateVehicle([FromBody] VehicleDTO vehicleDTO)
        {
            var validate = validateVehicle(vehicleDTO);
            if (validate.Messages.Count > 0) return BadRequest(validate);

            var vehicle = new Vehicle
            {
                Name = vehicleDTO.Name,
                Branch = vehicleDTO.Branch,
                Year = vehicleDTO.Year,
            };

            _vehicleService.Add(vehicle);

            return Created($"/vehicle/{vehicle.Id}", vehicle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Roles = "adm,editor")]
        public IActionResult GetAllVehicles([FromQuery] int? page)
        {
            var vehicles = _vehicleService.GetAll(page);

            if (vehicles.Count == 0) return NoContent();
            else return Ok(vehicles);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        [Authorize(Roles = "adm,editor")]
        public IActionResult GetUniqueVehicle([FromRoute] int id)
        {
            var vehicle = _vehicleService.GetUniqueById(id);

            if (vehicle == null) return NotFound();
            else return Ok(vehicle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="vehicleDTO"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("/{id}")]
        [Authorize(Roles = "adm,editor")]
        public IActionResult UpdateVehicle([FromRoute] int id, VehicleDTO vehicleDTO)
        {
            var vehicle = _vehicleService.GetUniqueById(id);
            if (vehicle == null) return NotFound();

            var validate = validateVehicle(vehicleDTO);
            if (validate.Messages.Count > 0) return BadRequest(validate);

            vehicle.Name = vehicleDTO.Name;
            vehicle.Branch = vehicleDTO.Branch;
            vehicle.Year = vehicleDTO.Year;

            _vehicleService.Update(vehicle);
            return Ok(vehicle);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("/{id}")]
        [Authorize(Roles ="adm")]
        public IActionResult DeleteVehicle([FromRoute] int id)
        {
            var vehicle = _vehicleService.GetUniqueById(id);

            if (vehicle == null) return NotFound();

            _vehicleService.Delete(vehicle);
            return Ok(vehicle);

        }


        private ErrorHandler validateVehicle(VehicleDTO vehicleDTO)
        {
            // --- Handle
            var messagesError = new ErrorHandler();

            if (String.IsNullOrEmpty(vehicleDTO.Name)) messagesError.Messages.Add("Parameter 'Name' as required!");
            if (String.IsNullOrEmpty(vehicleDTO.Branch)) messagesError.Messages.Add("Parameter 'Brand' as required!");
            if (vehicleDTO.Year < 1950) messagesError.Messages.Add("This year is not valid. Only gets vehicles w/year after 1950!");

            return messagesError;
        }
    }
}
