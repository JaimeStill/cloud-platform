using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CloudPlatform.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .Model
                .GetEntityTypes()
                .ToList()
                .ForEach(entity =>
                {
                    modelBuilder
                        .Entity(entity.Name)
                        .ToTable(entity.Name.Split('.').Last());
                });
        }
    }
}