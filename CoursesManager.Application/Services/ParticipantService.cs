using CoursesManager.Application.Abstractions.Persistence;
using CoursesManager.Application.Common.Errors;
using CoursesManager.Application.Common.Results;
using CoursesManager.Application.Dtos.Participants;
using CoursesManager.Application.Mappers;
using CoursesManager.Domain.Entities;

namespace CoursesManager.Application.Services;

public class ParticipantService(IParticipantRepository participantRepository)
{
    private readonly IParticipantRepository _participantRepository = participantRepository;

    public async Task<ErrorOr<ParticipantDto>> CreateParticipantAsync(CreateParticipantDto dto, CancellationToken ct = default)
    {
        var exists = await _participantRepository.ExistAsync(p => p.Email == dto.Email);
        if (exists)
            return Error.Conflict("Participant.Conflict", $"Participant with email '{dto.Email}' already exists.");

        var saved = await _participantRepository.CreateAsync(new ParticipantEntity
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Email = dto.Email,
            PhoneNumber = dto.PhoneNumber,
            UpdatedAt = DateTime.UtcNow
        }, ct);

        return ParticipantMapper.ToParticipantDto(saved);
    }

    public async Task<ErrorOr<ParticipantDto>> GetOneParticipantAsync(int id, CancellationToken ct = default)
    {
        var participant = await _participantRepository.GetOneAsync(
            where: p => p.Id == id,
            select: ParticipantMapper.ToParticipantDtoExpr,
            ct: ct
        );

        return participant is null
            ? Error.NotFound("Participant.NotFound", $"Participant with id '{id}' was not found.")
            : participant;
    }

    public async Task<IReadOnlyList<ParticipantDto>> GetAllParticipantsAsync(CancellationToken ct = default)
    {
        return await _participantRepository.GetAllAsync(
            select: ParticipantMapper.ToParticipantDtoExpr,
            orderBy: q => q.OrderBy(p => p.LastName).ThenBy(p => p.FirstName),
            ct: ct
        );
    }

    public async Task<ErrorOr<ParticipantDto>> UpdateParticipantAsync(int id, UpdateParticipantDto dto, CancellationToken ct = default)
    {
        var participant = await _participantRepository.GetOneAsync(p => p.Id == id, ct);
        if (participant is null)
            return Error.NotFound("Participant.NotFound", $"Participant with id '{id}' was not found.");

        var emailTaken = await _participantRepository.ExistAsync(p => p.Email == dto.Email && p.Id != id);
        if (emailTaken)
            return Error.Conflict("Participant.Conflict", $"Participant with email '{dto.Email}' already exists.");

        participant.FirstName = dto.FirstName;
        participant.LastName = dto.LastName;
        participant.Email = dto.Email;
        participant.PhoneNumber = dto.PhoneNumber;
        participant.UpdatedAt = DateTime.UtcNow;

        await _participantRepository.SaveChangesAsync(ct);
        return ParticipantMapper.ToParticipantDto(participant);
    }

    public async Task<ErrorOr<Deleted>> DeleteParticipantAsync(int id, CancellationToken ct = default)
    {
        var participant = await _participantRepository.GetOneAsync(p => p.Id == id, ct);
        if (participant is null)
            return Error.NotFound("Participant.NotFound", $"Participant with id '{id}' was not found.");

        await _participantRepository.DeleteAsync(participant, ct);
        return Result.Deleted;
    }
}
