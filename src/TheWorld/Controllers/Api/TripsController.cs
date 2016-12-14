using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TheWorld.Models;
using TheWorld.ViewModels;

namespace TheWorld.Controllers.Api
{
    // The Route is applied to all methods in the controller
    // Routes can be ammended by adding /foo/bar/etc to the Http verb to follow the api/trips/foo/bar/etc path
    [Route("api/trips")]
    [Authorize]
    public class TripsController: Controller
    {
        private ILogger<TripsController> _logger;
        private IWorldRepository _repository;

        public TripsController(IWorldRepository repository, ILogger<TripsController> logger)
        {
            _repository = repository;
            _logger = logger;
        }
        

        // Note that returning a JsonResult means we HAVE to return a JsonResult and can't return any errors.
        // Shawn suggests using IActionResult instead
        //[HttpGet("api/trips")]
        //public JsonResult Get()
        //{
        //    return Json(new Trip() { Name = "My Trip" });
        //}

        [HttpGet("")]
        public IActionResult Get()
        {
            try
            {
                var results = _repository.GetTripsByUsername(User.Identity.Name);
                return Ok(Mapper.Map<IEnumerable<TripViewModel>>(results));
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get All Trips: {ex}");

                return BadRequest("Error occured");
            }
        }

        // [FromBody] will cause the API to attempt to model bind the names of the json object to the names of theTrip object
        // Unmapped properties will recieve default values.
        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]TripViewModel theTrip)
        {
            if (ModelState.IsValid)
            {
                // Save to the datatbase
                var newTrip = Mapper.Map<Trip>(theTrip);

                // Set new trip username to the current user
                newTrip.UserName = User.Identity.Name;

                _repository.AddTrip(newTrip);

                if (await _repository.SaveChangesAsync())
                {
                    return Created($"api/trips/{theTrip.Name}", Mapper.Map<TripViewModel>(newTrip));
                }

            }
            return BadRequest("Failed to save the trip.");
        }
    }
}
