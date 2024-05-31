using MediatR;
using Microsoft.AspNetCore.Mvc;
using Restaurants.Application.Dishes.Commands.CreateDish;

namespace restaurants_api.Controllers;

[ApiController]
[Route("api/restaurants/{restaurantId}/dishes")]
public class DishesController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateDish([FromRoute] int restaurantId, CreateDishCommand command)
    {
        command.RestaurantId = restaurantId;
        await mediator.Send(command);

        return Created();
    }

}