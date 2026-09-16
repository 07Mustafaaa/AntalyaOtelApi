Antalya Otel PMS (Property Management System) - Backend Çekirdeği

Bu proje; Antalya turizm sektöründeki otellerin oda yönetimi, tarih çakışmasız dinamik rezervasyon ve finansal operasyon ihtiyaçlarını karşılamak üzere modern backend mimarisi ile tasarlanmış bir Otel Yönetim Sistemi (PMS) çekirdeğidir.

---

Öne Çıkan Mühendislik Yetkinlikleri

* **Çift Rezervasyon Önleme (Double-Booking Prevention):** Belirli tarih aralıklarında oda çakışmasını veritabanı seviyesinde engelleyen tarih kesişim algoritması.
* **Finansal Veri Güvenliği (Soft-Delete):** Muhasebe ve denetim geçmişini kaybetmemek adına iptal edilen rezervasyonlar silinmez; `IptalEdildi` flag mimarisiyle işaretlenerek oda yeniden satışa açılır.
* **İlişkisel Veri Birleştirme & DTO Deseni:** `Rezervasyonlar`, `Odalar`, `OdaTipleri` ve `Musteriler` tabloları LINQ Join sorgularıyla birleştirilerek frontend tüketimine optimize edilmiş `RezervasyonDetayDto` modelleri üretilir.
* **Dinamik Fiyatlandırma & Ciro Takibi:** Konaklama gün sayısı ve oda tipinin günlük birim maliyeti üzerinden anlık toplam tutar ve toplam ciro hesaplama.

---

 Kullanılan Teknolojiler

* **Backend:** C# / .NET 8.0 Web API
* **Veritabanı ORM:** Entity Framework Core (Code-First & Migration mimarisi)
* **Veritabanı Motoru:** Microsoft SQL Server
* **Dokümantasyon & Test:** Swagger / OpenAPI
* **Dashboard:** JavaScript Fetch API & Bootstrap 5

---

 Veri Modelleri (Entities)

| Model | Açıklama |
| :--- | :--- |
| `Oda` | Fiziksel oda numarası, kat ve oda tipi ilişkisi |
| `OdaTipi` | Standart, Deluxe ve Kral Dairesi konseptleri ile günlük taban fiyatlar |
| `Musteri` | Misafir kimlik ve iletişim bilgileri |
| `Rezervasyon` | Tarih aralıkları, toplam hesap tutarı ve soft-delete durumu |
