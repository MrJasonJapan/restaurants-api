using System.Data.Common;
using Microsoft.AspNetCore.Identity;
using Restaurants.Domain.Constants;
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

        if (!dbContext.Roles.Any())
        {
            var roles = GetRoles();
            dbContext.Roles.AddRange(roles);
            await dbContext.SaveChangesAsync();
        }

        // manual role setup note
        // INSERT INTO AspNetUserRoles
        // (UserId, RoleId)
        // VALUES 
        // ('03ec03ff-2ce7-4e1c-ad53-8e4457ae2c10', '12ffe98b-143b-4ba9-9665-6480eaef7b26'),
        // ('42d97258-772a-44ea-9cb5-a23f31e8897c', '1e959375-cc4c-4985-858f-fc98875179e1'),
        // ('080b11b1-d8dd-4852-8c9c-123c3331b403', 'c5cfd582-5d06-40e0-acf5-d4a3b3d0bf91')

    }

    private IEnumerable<IdentityRole> GetRoles()
    {
        List<IdentityRole> roles = [
            new (UserRoles.User),
            new (UserRoles.Owner),
            new (UserRoles.Admin)
        ];

        return roles;
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