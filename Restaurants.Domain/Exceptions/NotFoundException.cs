namespace Restaurants.Domain.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string resourceType, string resourceIdentifier)
        : base($"{resourceType} with identifier {resourceIdentifier} does not exist.")
    {

    }
}