using FluentValidation;
using Restaurants.Application.Commands.CreateRestaurant;

namespace Restaurants.Application.Restaurants.Validators;

// An "Abastract" class is a class that cannot be instantiated on its own, but can be inherited by other classes.
// This can be beneficial when you want to create a class that should not be instantiated, but should be inherited by other classes.
public class CreateRestaurantCommandValidator : AbstractValidator<CreateRestaurantCommand>
{

    private readonly List<string> validCategories = ["Italian", "Mexican", "Japanese", "American", "Indian"];

    public CreateRestaurantCommandValidator()
    {
        RuleFor(dto => dto.Name)
            .Length(3, 100);

        RuleFor(dto => dto.Description)
            .NotEmpty()
            .WithMessage("Description may not be empty");

        RuleFor(dto => dto.Category)
            .Must(validCategories.Contains)
            .WithMessage("Invalid category. Please choose from the valid categories.");

        RuleFor(dto => dto.ContactEmail)
            .EmailAddress().When(dto => dto.ContactEmail != null)
            .WithMessage("Invalid email address");

        RuleFor(dto => dto.PostalCode)
            .Matches(@"^\d{3}-\d{4}$").When(dto => dto.PostalCode != null)
            .WithMessage("Invalid postal code. Please use the format (XXX-XXXX).");
    }
}