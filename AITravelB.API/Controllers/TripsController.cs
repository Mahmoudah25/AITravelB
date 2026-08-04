using AITravelB.Application.Trips.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AITravelB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly IMediator mediator;
        public TripsController(IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateTrip([FromBody] CreateTripCommand command)
        {
            var tripId = await mediator.Send(command);
            return Ok(tripId);
        }

    }
}
