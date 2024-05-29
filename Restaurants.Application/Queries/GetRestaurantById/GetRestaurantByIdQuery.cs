using MediatR;
using Restaurants.Application.Restaurants.Dtos;

namespace Restaurants.Application.Queries.GetRestaurantById;

public class GetRestaurantByIdQuery(int id) : IRequest<RestaurantDto?>
{
    public int Id { get; set; } = id;
}