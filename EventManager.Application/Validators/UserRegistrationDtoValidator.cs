using EventManagement.Application.DTO;
using FluentValidation;

namespace EventManagement.Application.Validators;

public class UserRegistrationDtoValidator : AbstractValidator<UserRegistrationDto>
{
    public UserRegistrationDtoValidator()
    {
        RuleFor(u => u.Email)
            .NotEmpty().WithMessage("Email обязателен.")
            .EmailAddress().WithMessage("Некорректный формат email.");

        RuleFor(u => u.Password)
            .NotEmpty().WithMessage("Пароль обязателен.")
            .MinimumLength(8).WithMessage("Пароль должен содержать минимум 8 символов.")
            .Matches("[A-Z]").WithMessage("Пароль должен содержать хотя бы одну заглавную букву.")
            .Matches("[a-z]").WithMessage("Пароль должен содержать хотя бы одну строчную букву.")
            .Matches("[0-9]").WithMessage("Пароль должен содержать хотя бы одну цифру.");
    }
}