using System.ComponentModel.DataAnnotations;

namespace EventManager.Domain.Entities;

public class Event
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public DateTime Date { get; set; }
    public required string Location { get; set; }
    public required string Category { get; set; }
    public int MaxParticipants { get; set; }
    public string? ImageUrl { get; set; }
    
    public List<Participant> Participants { get; set; } = new();
}