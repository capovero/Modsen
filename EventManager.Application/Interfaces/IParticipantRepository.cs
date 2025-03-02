using EventManager.Domain.Entities;

namespace EventManagement.Application.Interfaces;

public interface IParticipantRepository
{
    Task<Participant> GetByIdAsync(Guid id);
    Task<List<Participant>> GetByEventIdAsync(Guid eventId);
    Task<Participant> AddAsync(Participant participant);
    Task RemoveAsync(Participant participant);
    Task<bool> IsEmailRegisteredAsync(Guid eventId, string email);
}