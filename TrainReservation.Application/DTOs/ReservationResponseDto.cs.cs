using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainReservation.Application.DTOs
{
    public class ReservationResponseDto
    {
        public bool RezervasyonYapilabilir { get; set; }
        public List<YerlesimAyrintiDto> YerlesimAyrinti { get; set; } = new List<YerlesimAyrintiDto>();
    }

    public class YerlesimAyrintiDto
    {
        public string VagonAdi { get; set; } = string.Empty;
        public int KisiSayisi { get; set; }
    }
}
