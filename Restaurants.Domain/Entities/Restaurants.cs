﻿namespace Restaurants.Domain.Entities;

public class Restaurants
{
    public int Id { get; set; }

    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    public string Category { get; set; } = default!;

    public bool HasDelivery { get; set; }

    public string? ContactEmail { get; set; }

    public string? ContactNumber { get; set; }

    public Address? Address { get; set; }

    public List<Dish> Dishes { get; set; } = new();
}

