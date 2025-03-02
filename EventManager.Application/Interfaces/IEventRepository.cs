using EventManager.Domain.Entities;

namespace EventManagement.Application.Interfaces;

public interface IEventRepository
{
    Task<List<Event>> GetAllAsync();
    Task<Event> GetByIdAsync(Guid id);
    Task<Event> AddAsync(Event entity);
    Task<Event> UpdateAsync(Event entity);
    Task DeleteAsync(Guid id);
    Task<List<Event>> GetByCriteriaAsync(
        string? title, 
        DateTime? date, 
        string? location, 
        string? category,
        int pageNumber = 1,
        int pageSize = 10
    );
    Task<int> GetParticipantsCountAsync(Guid eventId);
    
}