using Microsoft.EntityFrameworkCore;
using AntalyaOtelApi.Models;

namespace AntalyaOtelApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Musteri> Musteriler { get; set; }
    public DbSet<OdaTipi> OdaTipleri { get; set; }
    public DbSet<Oda> Odalar { get; set; }
    public DbSet<Rezervasyon> Rezervasyonlar { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Varsayılan Oda Tipleri
        modelBuilder.Entity<OdaTipi>().HasData(
            new OdaTipi { OdaTipId = 1, TipAdi = "Standart", GunlukFiyat = 1000, Aciklama = "1 Çift Kişilik Yatak, Şehir Manzaralı" },
            new OdaTipi { OdaTipId = 2, TipAdi = "Deluxe", GunlukFiyat = 2500, Aciklama = "Geniş Balkon, Deniz Manzaralı, Jakuzili" },
            new OdaTipi { OdaTipId = 3, TipAdi = "Kral Dairesi", GunlukFiyat = 7500, Aciklama = "Özel Teras, Özel Havuz, Ultra Lüks" }
        );

        // 2. Varsayılan Odalar
        modelBuilder.Entity<Oda>().HasData(
            new Oda { OdaId = 1, OdaNo = 101, Kat = 1, OdaTipId = 1 },
            new Oda { OdaId = 2, OdaNo = 102, Kat = 1, OdaTipId = 1 },
            new Oda { OdaId = 3, OdaNo = 201, Kat = 2, OdaTipId = 2 },
            new Oda { OdaId = 4, OdaNo = 202, Kat = 2, OdaTipId = 2 },
            new Oda { OdaId = 5, OdaNo = 301, Kat = 3, OdaTipId = 3 }
        );
    }
}