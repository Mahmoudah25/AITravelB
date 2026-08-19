using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AITravelB.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IMediator mediatR;
        public BookingsController(IMediator mediatR)
        {
            this.mediatR = mediatR;
        }

        [HttpPost]
        public async Task<IActionResult> CreateBooking([FromBody] Application.Bookings.Commands.CreateBooking.CreateBookingCommand command)
        {
            var bookingId = await mediatR.Send(command);
            return Ok(bookingId);
        }
    }
}
