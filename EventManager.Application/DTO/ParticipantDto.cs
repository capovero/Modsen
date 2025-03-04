namespace EventManagement.Application.DTO;

public class ParticipantDto
{
    public Guid Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime BirthDate { get; set; }
    public DateTime RegistrationDate { get; set; }
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
}