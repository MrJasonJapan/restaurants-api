using Microsoft.AspNetCore.Mvc;
using MediatR;
using Restaurants.Application.Queries;
using Restaurants.Application.Queries.GetRestaurantById;
using Restaurants.Application.Commands.UpdateRestaurant;
using Restaurants.Application.Restaurants.Commands.CreateRestaurant;
using Restaurants.Application.Restaurants.Commands.DeleteRestaurant;

namespace Restaurants.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RestaurantsController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var restaurants = await mediator.Send(new GetAllRestaurantsQuery());

        return Ok(restaurants);
    }

    [HttpGet]
    [Route("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var restaurant = await mediator.Send(new GetRestaurantByIdQuery(id));

        if (restaurant == null)
            return NotFound();

        return Ok(restaurant);
    }

    [HttpPost]
    // FluentValidator will automatically validate the request because we have scanned the assembly and it knows that CreateRestaurantCommand has a validator.
    public async Task<IActionResult> CreateRestaurant(CreateRestaurantCommand command)
    {
        int id = await mediator.Send(command);

        return CreatedAtAction(nameof(GetById), new { id }, null);
    }

    [HttpPatch("{id}")]
    public async Task<IActionResult> UpdateRestaurant([FromRoute] int id, UpdateRestaurantCommand command)
    {
        command.Id = id;

        var idUpdated = await mediator.Send(command);

        if (idUpdated)
            return NoContent();

        return NotFound();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteRestaurant([FromRoute] int id)
    {
        var isDeleted = await mediator.Send(new DeleteRestaurantCommand(id));

        if (isDeleted)
            return NoContent();

        return NotFound();
    }
}