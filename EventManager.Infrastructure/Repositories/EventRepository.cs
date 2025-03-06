using EventManagement.Application.Interfaces;
using EventManagement.Infrastructure.Data;
using EventManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly AppDbContext _context;

    public EventRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Event>> GetAllAsync() 
        => await _context.Events.ToListAsync();
    
    public async Task<Event> GetByIdAsync(Guid id) 
        => await _context.Events
        .Include(e => e.Participants)
        .FirstOrDefaultAsync(e => e.Id == id);
        
    public async Task<Event> AddAsync(Event entity)
    {
        await _context.Events.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Event> UpdateAsync(Event entity)
    {
        _context.Events.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id);
        _context.Events.Remove(entity);
        await _context.SaveChangesAsync();
    }
    
    public async Task<List<Event>> GetByCriteriaAsync(
        string? title,
        DateTime? date,
        string? location,
        string? category,
        int pageNumber = 1,
        int pageSize = 10)
    {
        var query = _context.Events.AsQueryable();

        if (!string.IsNullOrEmpty(title))
            query = query.Where(e => EF.Functions.ILike(e.Title, $"%{title}%"));
    
        if (date.HasValue)
            query = query.Where(e => e.Date.Date == date.Value.Date);
    
        if (!string.IsNullOrEmpty(location))
            query = query.Where(e => e.Location == location);
    
        if (!string.IsNullOrEmpty(category))
            query = query.Where(e => e.Category == category);

        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
    }

    public async Task<int> GetParticipantsCountAsync(Guid eventId)
        => await _context.Participants.CountAsync(p => p.EventId == eventId);
}