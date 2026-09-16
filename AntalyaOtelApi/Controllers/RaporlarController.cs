using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AntalyaOtelApi.Data;

namespace AntalyaOtelApi.Controllers;

public class DashboardOzetDto
{
    public int ToplamOdaSayisi { get; set; }
    public int AktifRezervasyonSayisi { get; set; }
    public int IptalEdilenRezervasyonSayisi { get; set; }
    public decimal ToplamKazanilanCiro { get; set; }
}

[ApiController]
[Route("api/[controller]")]
public class RaporlarController : ControllerBase
{
    private readonly AppDbContext _context;

    public RaporlarController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet("ozet")]
    public async Task<ActionResult<DashboardOzetDto>> GetOzetRapor()
    {
        int toplamOda = await _context.Odalar.CountAsync();

        int aktifRezervasyon = await _context.Rezervasyonlar
            .CountAsync(r => !r.IptalEdildi);

        int iptalRezervasyon = await _context.Rezervasyonlar
            .CountAsync(r => r.IptalEdildi);

        decimal toplamCiro = await _context.Rezervasyonlar
            .Where(r => !r.IptalEdildi)
            .SumAsync(r => (decimal?)r.ToplamTutar) ?? 0;

        var rapor = new DashboardOzetDto
        {
            ToplamOdaSayisi = toplamOda,
            AktifRezervasyonSayisi = aktifRezervasyon,
            IptalEdilenRezervasyonSayisi = iptalRezervasyon,
            ToplamKazanilanCiro = toplamCiro
        };

        return Ok(rapor);
    }
}