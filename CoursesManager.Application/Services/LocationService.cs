using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Application.Common.Errors;
using CoursesManager.Application.Common.Results;
using CoursesManager.Application.Dtos.Locations;
using CoursesManager.Application.Mappers;
using CoursesManager.Domain.Entities;

namespace CoursesManager.Application.Services;

public class LocationService(ILocationRepository locationRepository)
{
    private readonly ILocationRepository _locationRepository = locationRepository;

    public async Task<ErrorOr<LocationDto>> CreateLocationAsync(CreateLocationDto dto, CancellationToken ct = default)
    {
        var exists = await _locationRepository.ExistAsync(x => x.Name == dto.Name);
        if (exists)
            return Error.Conflict("Location.Conflict", $"Location '{dto.Name}' already exists.");

        var saved = await _locationRepository.CreateAsync(new LocationEntity
        {
            Name = dto.Name,
            UpdatedAt = DateTime.UtcNow
        }, ct);

        return LocationMapper.ToLocationDto(saved);
    }

    public async Task<ErrorOr<LocationDto>> GetOneLocationAsync(int id, CancellationToken ct = default)
    {
        var location = await _locationRepository.GetOneAsync(
            where: x => x.Id == id,
            select: LocationMapper.ToLocationDtoExpr,
            ct: ct
        );

        return location is null
            ? Error.NotFound("Location.NotFound", $"Location with id '{id}' was not found.")
            : location;
    }

    public async Task<IReadOnlyList<LocationDto>> GetAllLocationsAsync(CancellationToken ct = default)
    {
        return await _locationRepository.GetAllAsync(
            select: LocationMapper.ToLocationDtoExpr,
            orderBy: q => q.OrderBy(x => x.Name),
            ct: ct
        );
    }

    public async Task<ErrorOr<LocationDto>> UpdateLocationAsync(int id, UpdateLocationDto dto, CancellationToken ct = default)
    {
        var location = await _locationRepository.GetOneAsync(x => x.Id == id, ct);
        if (location is null)
            return Error.NotFound("Location.NotFound", $"Location with id '{id}' was not found.");

        var nameTaken = await _locationRepository.ExistAsync(x => x.Name == dto.Name && x.Id != id);
        if (nameTaken)
            return Error.Conflict("Location.Conflict", $"Location '{dto.Name}' already exists.");

        location.Name = dto.Name;
        location.UpdatedAt = DateTime.UtcNow;

        await _locationRepository.SaveChangesAsync(ct);
        return LocationMapper.ToLocationDto(location);
    }

    public async Task<ErrorOr<Deleted>> DeleteLocationAsync(int id, CancellationToken ct = default)
    {
        var location = await _locationRepository.GetOneAsync(x => x.Id == id, ct);
        if (location is null)
            return Error.NotFound("Location.NotFound", $"Location with id '{id}' was not found.");

        await _locationRepository.DeleteAsync(location, ct);
        return Result.Deleted;
    }
}
