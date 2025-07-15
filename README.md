# 🚆 Train Reservation API

Bu proje, bir tren rezervasyon sistemini simüle eden RESTful bir ASP.NET Core Web API uygulamasıdır. Gelen rezervasyon taleplerine göre vagonların doluluk oranlarını ve kuralları dikkate alarak uygun yerleşim önerileri sunar.

---

## 💡 Projenin Amacı

Tren içinde birden fazla vagon ve her bir vagonun kendine ait kapasitesi vardır.  
Yolcular rezervasyon yaparken;

- Tek vagonda mı kalmak istiyor?
- Farklı vagonlara yerleştirilebilirler mi?
- Rezervasyon yapılabilir mi?
- Hangi vagonda kaç kişi olacak?

gibi sorulara cevap veren bir sistemdir.

---

## 🔧 Kullanılan Teknolojiler

- .NET 8  
- ASP.NET Core Web API  
- Clean Architecture (Katmanlı Mimari)  
- C#  
- Swagger / OpenAPI (dökümantasyon)  
- Railway.app (ücretsiz cloud deploy)  
- xUnit (Unit Test)

---

## 🌐 Canlı Uygulama

API canlı olarak Railway’de yayınlanmıştır:

🔗 **Swagger Arayüzü**  
[https://trainreservation-production.up.railway.app/swagger/index.html](https://trainreservation-production.up.railway.app/swagger/index.html)

🔗 **API Root**  
[https://trainreservation-production.up.railway.app](https://trainreservation-production.up.railway.app)

---

## 📬 API Endpoint

### 🔹 `POST /api/reservation`

#### İstek (Request) Gövdesi:

```json
{
  "Tren": {
    "Ad": "Başkent Ekspres",
    "Vagonlar": [
      { "Ad": "Vagon 1", "Kapasite": 100, "DoluKoltukAdet": 68 },
      { "Ad": "Vagon 2", "Kapasite": 90, "DoluKoltukAdet": 50 },
      { "Ad": "Vagon 3", "Kapasite": 80, "DoluKoltukAdet": 80 }
    ]
  },
  "RezervasyonYapilacakKisiSayisi": 3,
  "KisilerFarkliVagonlaraYerlestirilebilir": true
}
```

#### Yanıt (Response) Gövdesi:

**Rezervasyon yapılabiliyorsa:**

```json
{
  "RezervasyonYapilabilir": true,
  "YerlesimAyrinti": [
    { "VagonAdi": "Vagon 1", "KisiSayisi": 2 },
    { "VagonAdi": "Vagon 2", "KisiSayisi": 1 }
  ]
}
```

**Rezervasyon yapılamıyorsa:**

```json
{
  "RezervasyonYapilabilir": false,
  "YerlesimAyrinti": []
}
```

---

## 🧠 İş Kuralları (Business Rules)

- Her vagonun doluluk oranı maksimum **%70** olabilir.  
  (Örneğin kapasitesi 100 olan bir vagonda 71 kişi olamaz.)

- Eğer tüm grup tek bir vagona sığabiliyorsa, **öncelikle o vagona yerleştirilir.**

- Eğer grup tek vagona sığamıyorsa ve `KisilerFarkliVagonlaraYerlestirilebilir: true` ise,  
  **grup üyeleri farklı vagonlara dağıtılır.**

- Eğer `KisilerFarkliVagonlaraYerlestirilebilir: false` ve tek bir vagonda tüm yolculara yer yoksa,  
  **rezervasyon yapılamaz.**

- Vagonlar arasında tercih yapılırken:  
  - **Grubun tamamı bir vagona yerleşebiliyorsa**, o vagon seçilir.  
  - **Birden fazla vagon eşit uygunlukta ise**, en boş olan tercih edilir.  
  - **Eşit doluluk varsa**, rastgele bir vagon seçilebilir.

---

## 🚀 Projeyi Local'de Çalıştırma

### 1. Klonla

```bash
git clone https://github.com/gdkmuharrem/TrainReservation.git
cd TrainReservation
```

### 2. Restore ve Çalıştır

```bash
dotnet restore
dotnet run --project TrainReservation.API
```

### 3. Açılan Swagger arayüzü

[http://localhost:5000/swagger/index.html](http://localhost:5000/swagger/index.html)

---

## 🧪 Unit Testler

Unit test senaryoları `TrainReservation.Tests` projesi içinde tanımlanmıştır.

### Testleri çalıştırmak için:

```bash
dotnet test
```

### İçerdiği test senaryoları:

- Tüm grup aynı vagona sığabiliyorsa, tek vagona yerleştirilir.  
- Farklı vagonlara yerleşmeye izin varsa, uygun şekilde dağıtılır.  
- %70 sınırına uymayan vagonlara rezervasyon yapılamaz.  
- Uygunluk yoksa `RezervasyonYapilabilir: false` döner.

---

## 📦 Deployment

Uygulama ücretsiz olarak **Railway** platformu üzerinden yayınlanmıştır.

### Railway Ayarları:

- Ortam değişkeni: `PORT`  
- Uygulama bu portu dinler.

- **HTTPS yönlendirmesi** production ortamında kapalıdır.  
- **Swagger** her ortamda aktiftir.

---

## 📁 Proje Yapısı (Clean Architecture)

```
TrainReservation.sln
│
├── TrainReservation.API          → ASP.NET Core Web API (UI Katmanı)
├── TrainReservation.Application  → Servisler, DTO'lar, kurallar
├── TrainReservation.Domain       → Modeller, veri yapıları
├── TrainReservation.Tests        → xUnit testleri
```

---

## ✍️ Geliştirici

**Muharrem Gedik**  
🔗 [https://www.muharremgedik.com](https://www.muharremgedik.com)  
📧 [gdk.muharrem@gmail.com](mailto:gdk.muharrem@gmail.com)
