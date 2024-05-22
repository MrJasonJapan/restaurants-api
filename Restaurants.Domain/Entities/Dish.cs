﻿namespace Restaurants.Domain.Entities;

public class Dish
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public decimal Price { get; set; }


    // --- Relationships ---
    public int RestaurantId { get; set; }
}

