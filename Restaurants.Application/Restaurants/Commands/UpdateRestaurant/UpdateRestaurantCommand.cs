using MediatR;

namespace Restaurants.Application.Commands.UpdateRestaurant;

public class UpdateRestaurantCommand : IRequest
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public bool HasDelivery { get; set; }
}

