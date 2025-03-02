using System.ComponentModel.DataAnnotations;

namespace EventManager.Web.DTO.Requests;

public class RegisterParticipantRequest
{
    [Required]
    public Guid EventId { get; set; }

    [Required, MaxLength(100)]
    public string FirstName { get; set; }

    [Required, MaxLength(100)]
    public string LastName { get; set; }

    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public DateTime BirthDate { get; set; }
}