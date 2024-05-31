using Restaurants.Domain.Entities;
using Restaurants.Domain.Repositories;
using Restaurants.Infrastructure.Persistence;

namespace Restaurants.Infrastructure.Repositories;

public class DishesRepository(RestaurantsDbContext dbContext) : IDishesRepository
{
    public async Task<int> Create(Dish entity)
    {
        dbContext.Dishes.Add(entity);
        await dbContext.SaveChangesAsync();

        return entity.Id;
    }
}