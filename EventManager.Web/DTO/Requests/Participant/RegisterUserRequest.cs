using System.ComponentModel.DataAnnotations;

namespace EventManager.Web.DTO.Requests;

public class RegisterUserRequest
{
    [Required, EmailAddress]
    public string Email { get; set; }

    [Required, MinLength(8)]
    public string Password { get; set; }
}