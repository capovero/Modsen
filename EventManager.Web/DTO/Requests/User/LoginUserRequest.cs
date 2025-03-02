using System.ComponentModel.DataAnnotations;

namespace EventManager.Web.DTO.Requests;

public class LoginUserRequest
{
    [Required, EmailAddress]
    public string Email { get; set; }

    [Required]
    public string Password { get; set; }
}