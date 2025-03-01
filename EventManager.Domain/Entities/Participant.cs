namespace EventManager.Domain.Entities;

public class Participant
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime RegistrationDate { get; set; }
    public string Email { get; set; }
    
    public Guid EventId { get; set; }
    public Event Event { get; set; }
}