using Microsoft.AspNetCore.Mvc;
using AntalyaOtelApi.Data;
using AntalyaOtelApi.Models;

namespace AntalyaOtelApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MusterilerController : ControllerBase
{
    private readonly AppDbContext _context;

    public MusterilerController(AppDbContext context)
    {
        _context = context;
    }

    // 1. Tüm müşterileri getir
    [HttpGet]
    public List<Musteri> GetMusteriler()
    {
        return _context.Musteriler.ToList();
    }

    // 2. ID ile tek müşteri getir
    [HttpGet("{id}")]
    public ActionResult<Musteri> GetMusteriById(int id)
    {
        var musteri = _context.Musteriler.Find(id);
        if (musteri == null)
        {
            return NotFound("Müşteri bulunamadı.");
        }
        return musteri;
    }

    // 3. Yeni müşteri ekle
    [HttpPost]
    public string AddMusteri(Musteri yeniMusteri)
    {
        _context.Musteriler.Add(yeniMusteri);
        _context.SaveChanges();
        return "Müşteri başarıyla eklendi.";
    }

    // 4. Müşteri sil (Senin yazdığın kod!)
    [HttpDelete("{id}")]
    public string DeleteMusteri(int id)
    {
        var musteri = _context.Musteriler.Find(id);
        if (musteri == null)
        {
            return "Böyle bir müşteri sistemde bulunamadı!";
        }

        _context.Musteriler.Remove(musteri);
        _context.SaveChanges();
        return "Müşteri başarıyla silindi.";
    }
}