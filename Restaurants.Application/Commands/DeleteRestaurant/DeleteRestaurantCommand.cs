using MediatR;

namespace Restaurants.Application.Commands.DeleteRestaurant;

public class DeleteRestaurantCommand(int id) : IRequest<bool>
{
    public int Id { get; set; } = id;
}

