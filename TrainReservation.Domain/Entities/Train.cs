using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainReservation.Domain.Entities
{
    public class Train
    {
        public string Ad { get; set; } = string.Empty;
        public List<Vagon> Vagonlar { get; set; } = new List<Vagon>();
    }
}
