using DockMobile.ApiClient.Models.ApiModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DockMobile.ApiClient.Models.ApiModels
{
    public class DokPartiDetay
    {
        public int Id { get; set; }
        public string? PartiNo { get; set; }
        public DateTime Tarih { get; set; } = DateTime.Now;
        public int DokPartiAnaId { get; set; }
    }
}
