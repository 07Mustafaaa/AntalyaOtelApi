using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AntalyaOtelApi.Data;
using AntalyaOtelApi.Models;

namespace AntalyaOtelApi.Controllers;

// 1. Raporlama için birleşik rezervasyon detay kutusu
public class RezervasyonDetayDto
{
    public int RezervasyonId { get; set; }
    public string MusteriTamAd { get; set; } = string.Empty;
    public string MusteriTelefon { get; set; } = string.Empty;
    public int OdaNo { get; set; }
    public int Kat { get; set; }
    public string OdaTipiAdi { get; set; } = string.Empty;
    public DateTime GirisTarihi { get; set; }
    public DateTime CikisTarihi { get; set; }
    public decimal ToplamTutar { get; set; }
}

// 2. Müsait odaları listelemek için gösterilecek bilgi kutusu
public class MusaitOdaDto
{
    public int OdaId { get; set; }
    public int OdaNo { get; set; }
    public int Kat { get; set; }
    public string OdaTipiAdi { get; set; } = string.Empty;
    public decimal GunlukFiyat { get; set; }
    public decimal TahminiToplamFiyat { get; set; }
}

// 3. Rezervasyon yaparken dışarıdan beklediğimiz form verisi
public class RezervasyonTalepDto
{
    public int MusteriId { get; set; }
    public int OdaId { get; set; }
    public DateTime GirisTarihi { get; set; }
    public DateTime CikisTarihi { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class RezervasyonlarController : ControllerBase
{
    private readonly AppDbContext _context;

    public RezervasyonlarController(AppDbContext context)
    {
        _context = context;
    }

    // GET 1: Mevcut ve aktif olan tüm rezervasyonları detaylarıyla getirir (ASENKRON)
    // GET 1: Mevcut ve aktif olan tüm rezervasyonları detaylarıyla getirir (GÜVENLİ LEFT JOIN)
    [HttpGet]
    public async Task<ActionResult<List<RezervasyonDetayDto>>> GetRezervasyonlar()
    {
        var liste = await (from rez in _context.Rezervasyonlar
                           where !rez.IptalEdildi
                           join musteri in _context.Musteriler on rez.MusteriId equals musteri.MusteriId into musteriGrup
                           from m in musteriGrup.DefaultIfEmpty()
                           join oda in _context.Odalar on rez.OdaId equals oda.OdaId into odaGrup
                           from o in odaGrup.DefaultIfEmpty()
                           join odaTipi in _context.OdaTipleri on o.OdaTipId equals odaTipi.OdaTipId into tipGrup
                           from t in tipGrup.DefaultIfEmpty()
                           select new RezervasyonDetayDto
                           {
                               RezervasyonId = rez.RezervasyonId,
                               MusteriTamAd = m != null ? m.Ad + " " + m.Soyad : "Genel Misafir",
                               MusteriTelefon = m != null ? m.Telefon : "-",
                               OdaNo = o != null ? o.OdaNo : 0,
                               Kat = o != null ? o.Kat : 0,
                               OdaTipiAdi = t != null ? t.TipAdi : "Standart",
                               GirisTarihi = rez.GirisTarihi,
                               CikisTarihi = rez.CikisTarihi,
                               ToplamTutar = rez.ToplamTutar
                           }).ToListAsync();

        return Ok(liste);
    }

    // GET 2: Belirli tarihler arasında boşta olan odaları hesaplayıp getirir (ASENKRON)
    [HttpGet("musait-odalar")]
    public async Task<ActionResult<List<MusaitOdaDto>>> GetMusaitOdalar(DateTime girisTarihi, DateTime cikisTarihi)
    {
        if (girisTarihi >= cikisTarihi)
        {
            return BadRequest("Çıkış tarihi giriş tarihinden sonra olmalıdır.");
        }

        int geceSayisi = (int)(cikisTarihi - girisTarihi).TotalDays;

        // O tarihlerde çakışan ve iptal edilmemiş odaların ID'lerini bul
        var doluOdaIdleri = await _context.Rezervasyonlar
            .Where(r => !r.IptalEdildi &&
                        girisTarihi < r.CikisTarihi &&
                        cikisTarihi > r.GirisTarihi)
            .Select(r => r.OdaId)
            .ToListAsync();

        // Dolu listesinde olmayan odaları Oda Tipiyle birleştirip listele
        var musaitOdalar = await (from oda in _context.Odalar
                                  join tip in _context.OdaTipleri on oda.OdaTipId equals tip.OdaTipId
                                  where !doluOdaIdleri.Contains(oda.OdaId)
                                  select new MusaitOdaDto
                                  {
                                      OdaId = oda.OdaId,
                                      OdaNo = oda.OdaNo,
                                      Kat = oda.Kat,
                                      OdaTipiAdi = tip.TipAdi,
                                      GunlukFiyat = tip.GunlukFiyat,
                                      TahminiToplamFiyat = tip.GunlukFiyat * geceSayisi
                                  }).ToListAsync();

        return Ok(musaitOdalar);
    }

    // POST: Yeni rezervasyon oluşturma (ASENKRON)
    [HttpPost]
    public async Task<ActionResult> PostRezervasyon(RezervasyonTalepDto talep)
    {
        if (talep.GirisTarihi >= talep.CikisTarihi)
        {
            return BadRequest("Çıkış tarihi giriş tarihinden sonra olmalıdır.");
        }

        bool odaDoluMu = await _context.Rezervasyonlar.AnyAsync(r =>
            !r.IptalEdildi &&
            r.OdaId == talep.OdaId &&
            talep.GirisTarihi < r.CikisTarihi &&
            talep.CikisTarihi > r.GirisTarihi);

        if (odaDoluMu)
        {
            return BadRequest("Seçilen oda bu tarihler arasında doludur!");
        }

        var oda = await _context.Odalar.FindAsync(talep.OdaId);
        if (oda == null) return NotFound("Oda bulunamadı.");

        var odaTipi = await _context.OdaTipleri.FindAsync(oda.OdaTipId);
        if (odaTipi == null) return NotFound("Oda tipi bulunamadı.");

        int gunSayisi = (int)(talep.CikisTarihi - talep.GirisTarihi).TotalDays;
        decimal toplamHesap = gunSayisi * odaTipi.GunlukFiyat;

        var yeniRezervasyon = new Rezervasyon
        {
            MusteriId = talep.MusteriId,
            OdaId = talep.OdaId,
            GirisTarihi = talep.GirisTarihi,
            CikisTarihi = talep.CikisTarihi,
            ToplamTutar = toplamHesap,
            IptalEdildi = false
        };

        await _context.Rezervasyonlar.AddAsync(yeniRezervasyon);
        await _context.SaveChangesAsync();

        return Ok($"Rezervasyon başarıyla kaydedildi! Toplam Tutar: {toplamHesap} TL");
    }

    // DELETE: Rezervasyon iptal etme - Soft Delete (ASENKRON)
    [HttpDelete("{id}")]
    public async Task<ActionResult> IptalEt(int id)
    {
        var rezervasyon = await _context.Rezervasyonlar.FindAsync(id);
        if (rezervasyon == null)
        {
            return NotFound("İptal edilecek rezervasyon bulunamadı.");
        }

        if (rezervasyon.IptalEdildi)
        {
            return BadRequest("Bu rezervasyon zaten daha önce iptal edilmiş.");
        }

        rezervasyon.IptalEdildi = true;
        await _context.SaveChangesAsync();

        return Ok($"Rezervasyon #{id} başarıyla iptal edildi. Oda tekrar satışa açıldı.");
    }
}