namespace EventManager.Domain.Entities;

public class Event
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public DateTime Date { get; set; }
    public string Location { get; set; }
    public string Categoty { get; set; }
    public int MaxParticipants { get; set; }
    public string ImageUrl { get; set; }
    
    public List<Participant> Participants { get; set; } = new();
}