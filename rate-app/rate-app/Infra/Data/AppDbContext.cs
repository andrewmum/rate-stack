using Microsoft.EntityFrameworkCore;
using rate_it_api.Core.Entities;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Item> Items { get; set; }
    public DbSet<Rating> Ratings { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Rating>()
            .HasOne(r => r.User)
            .WithMany(u => u.Ratings)
            .HasForeignKey(r => r.UserId);

        modelBuilder.Entity<Rating>()
            .HasOne(r => r.Item)
            .WithMany(i => i.Ratings)
            .HasForeignKey(r => r.ItemId);
        modelBuilder.Entity<User>().HasIndex(u => u.Email).IsUnique();

    }
}






