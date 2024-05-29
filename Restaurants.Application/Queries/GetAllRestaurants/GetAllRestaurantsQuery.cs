using MediatR;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Queries;

public class GetAllRestaurantsQuery : IRequest<IEnumerable<RestaurantDto>>
{

}