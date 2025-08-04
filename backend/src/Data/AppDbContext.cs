using Microsoft.EntityFrameworkCore;
using backend.src.Models;

namespace backend.src.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Cliente> Clientes { get; set; }
        
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Configurações adicionais, se necessário
        }
    }
}