using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainReservation.Application.DTOs;

namespace TrainReservation.Application.DTOs
{
    public interface IReservationService
    {
        Task<ReservationResponseDto> RezervasyonYapAsync(ReservationRequestDto request);
    }
}
