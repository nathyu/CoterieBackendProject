using Microsoft.EntityFrameworkCore;

namespace BackendTakeHome.Models
{
    public class QuoteContext : DbContext
    {
        public QuoteContext(DbContextOptions<QuoteContext> options)
            : base(options)
        {
        }

        public virtual DbSet<State> States { get; set; }
        public virtual DbSet<Business> Businesses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<State>()
                .HasKey(s => s.Name);
            modelBuilder.Entity<Business>()
                .HasKey(b => b.Name);
        }
    }
}
