using EventManagement.Application.DTO;
namespace EventManager.Application.DTO;

public class EventDto
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public string Location { get; set; }
    public string Category { get; set; }
    public int MaxParticipants { get; set; }
    public string? ImageUrl { get; set; }
    public List<ParticipantDto> Participants { get; set; } = new();
}