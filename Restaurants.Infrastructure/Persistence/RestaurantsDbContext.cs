using Microsoft.EntityFrameworkCore;
using Restaurants.Domain.Entities;

namespace Restaurants.Infrastructure.Persistence;

internal class RestaurantsDbContext(DbContextOptions<RestaurantsDbContext> options) : DbContext(options)
{
    internal DbSet<Restaurant> Restaurants { get; set; }

    internal DbSet<Dish> Dishes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Restaurant>()
            .OwnsOne(r => r.Address);

        // specify that a restaurant has many dishes
        modelBuilder.Entity<Restaurant>()
            .HasMany(r => r.Dishes)
            .WithOne()
            .HasForeignKey(d => d.RestaurantId);        
    }
}