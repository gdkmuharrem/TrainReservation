using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrainReservation.Application.DTOs;
using TrainReservation.Application.Services;

namespace TrainReservation.Application.Services
{
    public class ReservationService : IReservationService
    {
        public Task<ReservationResponseDto> RezervasyonYapAsync(ReservationRequestDto request)
        {
            var response = new ReservationResponseDto();
            int kalanKisiSayisi = request.RezervasyonYapilacakKisiSayisi;

            var yerlesim = new List<YerlesimAyrintiDto>();

            if (request.KisilerFarkliVagonlaraYerlestirilebilir)
            {
                // 1️⃣ Tüm kişilerin sığabileceği uygun vagonları bul
                var uygunVagonlar = request.Tren.Vagonlar
                    .Select(v => new
                    {
                        Vagon = v,
                        BosKoltuk = (int)(v.Kapasite * 0.7) - v.DoluKoltukAdet
                    })
                    .Where(x => x.BosKoltuk >= kalanKisiSayisi)
                    .ToList();

                if (uygunVagonlar.Any())
                {
                    // En fazla boşluğu olan vagon(lar)
                    int enFazlaBos = uygunVagonlar.Max(v => v.BosKoltuk);
                    var enUygunlar = uygunVagonlar
                        .Where(v => v.BosKoltuk == enFazlaBos)
                        .ToList();

                    // Eşitse rastgele birini seç
                    var random = new Random();
                    var secilen = enUygunlar.Count == 1
                        ? enUygunlar.First()
                        : enUygunlar[random.Next(enUygunlar.Count)];

                    response.RezervasyonYapilabilir = true;
                    response.YerlesimAyrinti.Add(new YerlesimAyrintiDto
                    {
                        VagonAdi = secilen.Vagon.Ad,
                        KisiSayisi = kalanKisiSayisi
                    });

                    return Task.FromResult(response);
                }

                // 2️⃣ Tek vagona sığmıyorsa → parça parça dağıt
                foreach (var vagon in request.Tren.Vagonlar)
                {
                    int maxKapasite = (int)(vagon.Kapasite * 0.7);
                    int bosKoltuk = maxKapasite - vagon.DoluKoltukAdet;

                    if (bosKoltuk <= 0) continue;

                    int yerlestirilecek = Math.Min(kalanKisiSayisi, bosKoltuk);

                    yerlesim.Add(new YerlesimAyrintiDto
                    {
                        VagonAdi = vagon.Ad,
                        KisiSayisi = yerlestirilecek
                    });

                    kalanKisiSayisi -= yerlestirilecek;

                    if (kalanKisiSayisi <= 0)
                        break;
                }

                response.RezervasyonYapilabilir = kalanKisiSayisi == 0;
                response.YerlesimAyrinti = response.RezervasyonYapilabilir ? yerlesim : new List<YerlesimAyrintiDto>();
            }
            else
            {
                // 🔒 Herkes aynı vagonda olmalı
                var uygunVagon = request.Tren.Vagonlar.FirstOrDefault(v =>
                    (int)(v.Kapasite * 0.7) - v.DoluKoltukAdet >= kalanKisiSayisi);

                if (uygunVagon != null)
                {
                    response.RezervasyonYapilabilir = true;
                    response.YerlesimAyrinti.Add(new YerlesimAyrintiDto
                    {
                        VagonAdi = uygunVagon.Ad,
                        KisiSayisi = kalanKisiSayisi
                    });
                }
                else
                {
                    response.RezervasyonYapilabilir = false;
                }
            }

            return Task.FromResult(response);
        }
    }
}
