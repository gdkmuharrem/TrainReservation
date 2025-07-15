using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrainReservation.Domain.Entities
{
    public class Vagon
    {
        public string Ad { get; set; } = string.Empty;
        public int Kapasite { get; set; }
        public int DoluKoltukAdet { get; set; }

        // Maksimum %70 doluluk sınırı
        public int MaxKapasite => (int)(Kapasite * 0.7);

        // Boş koltuk sayısı (maks doluluğa göre)
        public int BosKoltuk => MaxKapasite - DoluKoltukAdet;
    }
}
