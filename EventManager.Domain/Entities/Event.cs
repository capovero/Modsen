using System.ComponentModel.DataAnnotations;

namespace EventManager.Domain.Entities;

public class Event
{
    public Guid Id { get; set; }
    [Required] public string Title { get; set; }
    [Required] public string Description { get; set; }
    public DateTime Date { get; set; }
    [Required] public string Location { get; set; }
    [Required] public string Category { get; set; }
    public int MaxParticipants { get; set; }
    public string? ImageUrl { get; set; }
    
    public List<Participant> Participants { get; set; } = new();
}