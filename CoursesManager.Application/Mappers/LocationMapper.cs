using CoursesManager.Application.Dtos.Locations;
using CoursesManager.Domain.Entities;
using System.Linq.Expressions;

namespace CoursesManager.Application.Mappers;

public static class LocationMapper
{
    public static Expression<Func<LocationEntity, LocationDto>> ToLocationDtoExpr =>
        l => new LocationDto(
            l.Id, 
            l.Name, 
            l.CreatedAt, 
            l.UpdatedAt
        );

    public static LocationDto ToLocationDto(LocationEntity entity) =>
        new(
            entity.Id, 
            entity.Name, 
            entity.CreatedAt, 
            entity.UpdatedAt
        );
}
