using System.ComponentModel.DataAnnotations;

namespace EventManager.Web.DTO.Requests;

public class CreateEventRequest
{
    [Required, MaxLength(200)]
    public string Title { get; set; }

    [Required, MaxLength(2000)]
    public string Description { get; set; }

    [Required]
    public DateTime Date { get; set; }

    [Required, MaxLength(300)]
    public string Location { get; set; }

    [Required, MaxLength(100)]
    public string Category { get; set; }

    [Range(1, 1000)]
    public int MaxParticipants { get; set; }

    public IFormFile? Image { get; set; }
}