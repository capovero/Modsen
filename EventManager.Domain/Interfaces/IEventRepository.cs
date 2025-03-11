using EventManager.Domain.Entities;

namespace EventManagement.Application.Interfaces;

public interface IEventRepository
{
    Task<List<Event>> GetAllAsync();
    Task<Event?> GetByIdAsync(Guid id);
    Task<Event> AddAsync(Event entity);
    Task<Event> UpdateAsync(Event entity);
    Task DeleteAsync(Event entity);
    IQueryable<Event> GetQueryable();


}