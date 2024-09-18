using Restaurants.Domain.Constants;
using Restaurants.Domain.Entities;

namespace Restaurants.Domain.Repositories
{
    public interface IRestaurantsRepository
    {
        Task<int> Create(Restaurant entity);

        Task<IEnumerable<Restaurant>> GetAll();

        Task<Restaurant?> GetById(int id);

        Task<(IEnumerable<Restaurant>, int)> GetAllMatching(
            string? searchPhrase, int pageNumber, int pageSize, string? sortBy, SortDirection sortDirection);

        Task SaveChanges();

        Task Delete(Restaurant entity);
    }
}