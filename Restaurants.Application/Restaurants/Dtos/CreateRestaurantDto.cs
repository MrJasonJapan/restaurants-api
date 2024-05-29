using System.ComponentModel.DataAnnotations;

namespace Restaurants.Application.Restaurants.Dtos;

public class CreateRestaurantDto
{
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = default!;

    public string Description { get; set; } = default!;

    [Required(ErrorMessage = "Category is required")]
    public string Category { get; set; } = default!;

    public bool HasDelivery { get; set; }

    [EmailAddress(ErrorMessage = "Invalid email address")]
    public string? ContactEmail { get; set; }

    [Phone(ErrorMessage = "Invalid phone number")]
    public string? ContactNumber { get; set; }

    public string? City { get; set; }

    public string? Street { get; set; }

    [RegularExpression(@"^\d{3}-\d{4}$", ErrorMessage = "Invalid postal code. Please use the format XXX-XXXX")]
    public string? PostalCode { get; set; }
}