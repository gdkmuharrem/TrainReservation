using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainReservation.Application.DTOs
{
    public class ReservationRequestDto
    {
        public TrainDto Tren { get; set; } = new TrainDto();
        public int RezervasyonYapilacakKisiSayisi { get; set; }
        public bool KisilerFarkliVagonlaraYerlestirilebilir { get; set; }
    }

    public class TrainDto
    {
        public string Ad { get; set; } = string.Empty;
        public List<VagonDto> Vagonlar { get; set; } = new List<VagonDto>();
    }

    public class VagonDto
    {
        public string Ad { get; set; } = string.Empty;
        public int Kapasite { get; set; }
        public int DoluKoltukAdet { get; set; }
    }
}
