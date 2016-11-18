using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CRRMS.Web.Models;
using Microsoft.Extensions.Logging;
using AutoMapper;
using CRRMS.Web.ViewModels;
using CRRMS.Web.Services;
using Microsoft.AspNetCore.Authorization;

// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace CRRMS.Web.Controllers.Api
{
    [Route("/api/trips/{tripName}/stops")]
    [Authorize]
    public class StopsController : Controller
    {
        private GeoCoordsService _coordsService;
        private ILogger<StopsController> _logger;
        private IWorldRepository _repository;

        public StopsController(IWorldRepository repository, 
            ILogger<StopsController> logger,
            GeoCoordsService coordsService)
        {
            _repository = repository;
            _logger = logger;
            _coordsService = coordsService;
        }

        [HttpGet("")]
        public IActionResult Get(string tripName)
        {
            try
            {
                // var trip = _repository.GetTripByName(tripName);
                var trip = _repository.GetTripByName(tripName,User.Identity.Name);

                return Ok(Mapper.Map<IEnumerable<StopViewModel>>(trip.Stops.OrderBy(s=>s.Order).ToList()));
            }
            catch (Exception ex)
            {
                _logger.LogError("Failes to get stops: {0}", ex);
            }

            return BadRequest("Failed to get stops");
        }

        [HttpPost("")]
        public async Task<IActionResult> Post(string tripName , [FromBody]StopViewModel vm)
        {
            try
            {
                //if the VM is valid
                if(ModelState.IsValid)
                {
                    var newStop = Mapper.Map<Stop>(vm);


                    // Look up the GeoCodes

                    var result = await _coordsService.GetCoordsAsync(newStop.Name);

                    if (!result.Success)
                    {
                        _logger.LogError(result.Message);
                    }

                    else
                    {
                        newStop.Latitude = result.Latitude;
                        newStop.Longitude = result.Longitude;
                        //SAve to the db
                        _repository.AddStop(tripName,User.Identity.Name, newStop);
                        if (await _repository.SaveChangesAsync())
                        {
                            return Created($"/api/trips/{tripName}/stops/{newStop.Name}", Mapper.Map<StopViewModel>(newStop));
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                _logger.LogError("Failed to save new stop : {0}" , ex);
            }

            return BadRequest("Failed to save new stop");
        }
    }
}
