using EventManagement.Application.Interfaces;
using EventManagement.Infrastructure.Data;
using EventManager.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventManagement.Infrastructure.Repositories;

public class ParticipantRepository : IParticipantRepository
{
    private readonly AppDbContext _context;

    public ParticipantRepository(AppDbContext context)
    {
        _context = context;
    }
    
    public async Task<Participant> GetByIdAsync(Guid id)
        => await _context.Participants.FindAsync(id);

    public async Task<List<Participant>> GetByEventIdAsync(Guid eventId)
        => await _context.Participants
            .Where(p => p.EventId == eventId)
            .ToListAsync();

    public async Task<Participant> AddAsync(Participant participant)
    {
        await _context.Participants.AddAsync(participant);
        await _context.SaveChangesAsync();
        return participant;
    }

    public async Task RemoveAsync(Participant participant)
    {
        _context.Participants.Remove(participant);
        await _context.SaveChangesAsync();
    }

    public async Task<bool> IsUserRegisteredAsync(Guid eventId, Guid userId)
        => await _context.Participants
            .AnyAsync(p => p.EventId == eventId && p.UserId == userId);
}