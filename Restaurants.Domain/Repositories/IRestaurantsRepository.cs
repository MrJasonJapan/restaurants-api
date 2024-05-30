using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories
{
    public interface IRestaurantsRepository
    {
        Task<int> Create(Restaurant entity);

        Task<IEnumerable<Restaurant>> GetAll();

        Task<Restaurant?> GetById(int id);

        Task SaveChanges();

        Task Delete(Restaurant entity);
    }
}