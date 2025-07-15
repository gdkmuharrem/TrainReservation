using TrainReservation.Application.DTOs;
using TrainReservation.Application.Services;

namespace TrainReservation.Tests
{
    public class ReservationServiceTests
    {
        private readonly IReservationService _service;

        public ReservationServiceTests()
        {
            _service = new ReservationService();
        }

        [Fact]
        public async Task TumKisilerTekVagonaSigarsa_OrayaYerlestirilmeli()
        {
            var request = new ReservationRequestDto
            {
                RezervasyonYapilacakKisiSayisi = 3,
                KisilerFarkliVagonlaraYerlestirilebilir = true,
                Tren = new TrainDto
                {
                    Ad = "Test Ekspresi",
                    Vagonlar = new List<VagonDto>
                    {
                        new VagonDto { Ad = "V1", Kapasite = 100, DoluKoltukAdet = 60 }, // boþ 10
                        new VagonDto { Ad = "V2", Kapasite = 90, DoluKoltukAdet = 80 }   // boþ 0
                    }
                }
            };

            var response = await _service.RezervasyonYapAsync(request);

            Assert.True(response.RezervasyonYapilabilir);
            Assert.Single(response.YerlesimAyrinti);
            Assert.Equal("V1", response.YerlesimAyrinti[0].VagonAdi);
            Assert.Equal(3, response.YerlesimAyrinti[0].KisiSayisi);
        }

        [Fact]
        public async Task SirmaYlaDagitilabilirse_YerlesimDogruOlmali()
        {
            var request = new ReservationRequestDto
            {
                RezervasyonYapilacakKisiSayisi = 5,
                KisilerFarkliVagonlaraYerlestirilebilir = true,
                Tren = new TrainDto
                {
                    Ad = "Dagitim Ekspresi",
                    Vagonlar = new List<VagonDto>
                    {
                        new VagonDto { Ad = "V1", Kapasite = 100, DoluKoltukAdet = 68 }, // boþ 2
                        new VagonDto { Ad = "V2", Kapasite = 100, DoluKoltukAdet = 66 }, // boþ 4
                    }
                }
            };

            var response = await _service.RezervasyonYapAsync(request);

            Assert.True(response.RezervasyonYapilabilir);
            Assert.Equal(2, response.YerlesimAyrinti.Count);
            Assert.Equal(2, response.YerlesimAyrinti.First(x => x.VagonAdi == "V1").KisiSayisi);
            Assert.Equal(3, response.YerlesimAyrinti.First(x => x.VagonAdi == "V2").KisiSayisi);
        }

        [Fact]
        public async Task SiganVagonYoksa_RezervasyonYapilamaz()
        {
            var request = new ReservationRequestDto
            {
                RezervasyonYapilacakKisiSayisi = 10,
                KisilerFarkliVagonlaraYerlestirilebilir = false,
                Tren = new TrainDto
                {
                    Ad = "Yetersiz Ekspres",
                    Vagonlar = new List<VagonDto>
                    {
                        new VagonDto { Ad = "V1", Kapasite = 100, DoluKoltukAdet = 95 }, // boþ 0
                        new VagonDto { Ad = "V2", Kapasite = 100, DoluKoltukAdet = 90 }  // boþ 0
                    }
                }
            };

            var response = await _service.RezervasyonYapAsync(request);

            Assert.False(response.RezervasyonYapilabilir);
            Assert.Empty(response.YerlesimAyrinti);
        }

        [Fact]
        public async Task EsitBoslukta_RastgeleSecimYapilir()
        {
            var request = new ReservationRequestDto
            {
                RezervasyonYapilacakKisiSayisi = 2,
                KisilerFarkliVagonlaraYerlestirilebilir = true,
                Tren = new TrainDto
                {
                    Ad = "Rastgele Ekspres",
                    Vagonlar = new List<VagonDto>
                    {
                        new VagonDto { Ad = "V1", Kapasite = 100, DoluKoltukAdet = 68 }, // boþ 2
                        new VagonDto { Ad = "V2", Kapasite = 100, DoluKoltukAdet = 68 }  // boþ 2
                    }
                }
            };

            var vagonSecimleri = new Dictionary<string, int>
            {
                { "V1", 0 },
                { "V2", 0 }
            };

            // Rastgele çalýþmayý test etmek için algoritmayý 20 kez çalýþtýr
            for (int i = 0; i < 20; i++)
            {
                var response = await _service.RezervasyonYapAsync(request);
                Assert.True(response.RezervasyonYapilabilir);
                Assert.Single(response.YerlesimAyrinti);

                string secilenVagon = response.YerlesimAyrinti[0].VagonAdi;
                vagonSecimleri[secilenVagon]++;
            }

            // En az 1 kez her vagon seçilmiþ olmalý
            Assert.True(vagonSecimleri["V1"] > 0);
            Assert.True(vagonSecimleri["V2"] > 0);
        }
    }
}