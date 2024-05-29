using AutoMapper;
using Restaurants.Domain.Entities;

namespace Restaurants.Application.Restaurants.Dtos;

public class RestaurantsProfile : Profile
{
    public RestaurantsProfile()
    {
        CreateMap<Restaurant, RestaurantDto>()
            .ForMember(dto => dto.City, opt => opt.MapFrom(r => r.Address == null ? null : r.Address.City))
            .ForMember(dto => dto.Street, opt => opt.MapFrom(r => r.Address == null ? null : r.Address.Street))
            .ForMember(dto => dto.PostalCode, opt => opt.MapFrom(r => r.Address == null ? null : r.Address.PostalCode))
            // When the dishes mapping occurs, MapFrom() knows to use the profile in DishesProfile to map the dishes.
            // This is beause the source and destination types are the same as defined inside the DishesProfile.
            .ForMember(dto => dto.Dishes, opt => opt.MapFrom(r => r.Dishes));
    }
}