using System.ComponentModel.DataAnnotations;

namespace AntalyaOtelApi.Models;

public class OdaTipi
{
    [Key]
    public int OdaTipId { get; set; }

    public string TipAdi { get; set; } = string.Empty;
    public decimal GunlukFiyat { get; set; }
    public string Aciklama { get; set; } = string.Empty;
}