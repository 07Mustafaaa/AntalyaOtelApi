namespace AntalyaOtelApi.Models
{
    public class Rezervasyon
    {
        public int RezervasyonId { get; set; }
        public int MusteriId { get; set; }

        public int OdaId { get; set; }
        public DateTime GirisTarihi { get; set; }
        public DateTime CikisTarihi { get; set; }
        public decimal ToplamTutar {  get; set; }
        public bool IptalEdildi { get; set; } = false;



    }
}
