using FactoryFlow.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FactoryFlow.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Machine> Machines => Set<Machine>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Machine>(entity =>
            {
                entity.HasKey(x => x.Id);

                entity.Property(x => x.Code)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.HasIndex(x => x.Code)
                    .IsUnique();

                entity.Property(x => x.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(x => x.Status)
                    .HasConversion<string>()
                    .HasMaxLength(20)
                    .IsRequired();

                entity.HasData(
                    new { Id = 1, Code = "MC-001", Name = "CNC", Status = MachineStatus.Idle },
                    new { Id = 2, Code = "MC-002", Name = "Laser", Status = MachineStatus.Idle },
                    new { Id = 3, Code = "MC-003", Name = "Packing", Status = MachineStatus.Idle });
            });
        }
    }
}
