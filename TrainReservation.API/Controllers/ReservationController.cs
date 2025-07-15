using Microsoft.AspNetCore.Mvc;
using TrainReservation.Application.DTOs;
using TrainReservation.Application.Services;

namespace TrainReservation.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost]
        public async Task<IActionResult> MakeReservation([FromBody] ReservationRequestDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _reservationService.RezervasyonYapAsync(request);

            return Ok(result);
        }
    }
}
