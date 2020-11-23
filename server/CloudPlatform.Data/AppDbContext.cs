using System.Linq;
using Microsoft.EntityFrameworkCore;
using CloudPlatform.Data.Entities;
using CloudPlatform.Data.Extensions;

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
            modelBuilder.SetDefaultUserValues();

            modelBuilder
                .Entity<Folder>()
                .HasOne(x => x.User)
                .WithMany(x => x.Folders)
                .HasForeignKey(x => x.Owner)
                .HasPrincipalKey(x => x.Username);

            modelBuilder
                .Entity<Folder>()
                .HasMany(x => x.Folders)
                .WithOne(x => x.Parent)
                .HasForeignKey(x => x.FolderId)
                .IsRequired(false);

            modelBuilder
                .Entity<User>()
                .HasMany(x => x.SharedFolders)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.Username)
                .HasPrincipalKey(x => x.Username)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder
                .Entity<User>()
                .HasMany(x => x.SharedNotes)
                .WithOne(x => x.User)
                .HasForeignKey(x => x.Username)
                .HasPrincipalKey(x => x.Username)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

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