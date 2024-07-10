namespace DockMobile.Api.Models
{
    public class DokPartiDetay
    {
        public int Id { get; set; }
        public string? PartiNo { get; set; }
        public DateTime? Tarih { get; set; }
        public int DokPartiAnaId { get; set; }
    }
}
