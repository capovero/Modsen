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
    
    public async Task<Event?> GetByIdAsync(Guid id) 
        => await _context.Events.FirstOrDefaultAsync(e => e.Id == id);

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

    public async Task DeleteAsync(Event entity)
    {
        _context.Events.Remove(entity);
        await _context.SaveChangesAsync();
    }
    
    public IQueryable<Event> GetQueryable()
        => _context.Events.AsQueryable();
}