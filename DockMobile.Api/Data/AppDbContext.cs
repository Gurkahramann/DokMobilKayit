using DockMobile.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DockMobile.Api.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<DokPartiAna> DokPartiAna { get; set; }
        public DbSet<DokPartiDetay> DokPartiDetay { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
