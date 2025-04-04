namespace EventManager.Web.DTO.Responses;

public class EventResponse
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public string Location { get; set; }
    public string Category { get; set; }
    public int MaxParticipants { get; set; }
    public string? ImageUrl { get; set; }
    public int ParticipantsCount { get; set; }
} 