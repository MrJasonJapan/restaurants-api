using System.Data.Common;
using Restaurants.Domain.Entities;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Seeders;

public interface IRestaurantSeeder
{
    Task Seed();
}

internal class RestaurantSeeder(RestaurantsDbContext dbContext) : IRestaurantSeeder
{
    // seed data
    public async Task Seed()
    {
        if (await dbContext.Database.CanConnectAsync())
        {
            if (!dbContext.Restaurants.Any())
            {
                var restaurants = GetRestaurants();
                dbContext.Restaurants.AddRange(restaurants);
                await dbContext.SaveChangesAsync();
            }
        }
    }

    // GetRestaurants
    private static IEnumerable<Restaurant> GetRestaurants()
    {
        return new List<Restaurant>
        {
            new Restaurant
            {
                Name = "KDF",
                Category = "Fast Food",
                Description = "KDF is a fast food restaurant",
                ContactEmail = "contact@kfc.com",
                HasDelivery = true,
                Dishes = new List<Dish>
                {
                    new Dish
                    {
                        Name = "Chicken",
                        Description = "Fried chicken",
                        Price = 100
                    },
                    new Dish
                    {
                        Name = "Chips",
                        Description = "Fried chips",
                        Price = 50
                    }
                },
                Address = new Address
                {
                    City = "Nairobi",
                    Street = "Moi Avenue",
                    PostalCode = "00001"
                }
            },
            // McDonalds
            new Restaurant
            {
                Name = "McDonalds",
                Category = "Fast Food",
                Description = "McDonalds is a fast food restaurant",
                ContactEmail = "contact@mcd.com",
                HasDelivery = true,
                Dishes = new List<Dish>
                {
                    new Dish
                    {
                        Name = "Burger",
                        Description = "Beef burger",
                        Price = 150
                    },
                    new Dish
                    {
                        Name = "Fries",
                        Description = "Fried fries",
                        Price = 70
                    }
                },
                Address = new Address
                {
                    City = "Nairobi",
                    Street = "Kenyatta Avenue",
                    PostalCode = "00002"
                }
            }
        };
    }
}