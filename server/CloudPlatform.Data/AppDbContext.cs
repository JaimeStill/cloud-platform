using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace CloudPlatform.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Folder> Folders { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<SharedFolder> SharedFolders { get; set; }
        public DbSet<SharedNote> SharedNotes { get; set; }
        public DbSet<User> Users { get; set; }

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