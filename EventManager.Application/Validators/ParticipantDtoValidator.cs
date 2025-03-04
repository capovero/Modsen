using EventManagement.Application.DTO;
using EventManager.Domain.Entities;
using FluentValidation;

namespace EventManagement.Application.Validators;

public class ParticipantDtoValidator : AbstractValidator<ParticipantDto>
{
    public ParticipantDtoValidator()
    {
        RuleFor(p => p.FirstName)
            .NotEmpty().WithMessage("Имя участника обязательно.")
            .MaximumLength(100).WithMessage("Имя не должно превышать 100 символов.");

        RuleFor(p => p.LastName)
            .NotEmpty().WithMessage("Фамилия участника обязательна.")
            .MaximumLength(100).WithMessage("Фамилия не должна превышать 100 символов.");

        RuleFor(p => p.BirthDate)
            .LessThan(DateTime.UtcNow.AddYears(-16))
            .WithMessage("Участник должен быть старше 16 лет.");

        RuleFor(p => p.EventId)
            .NotEmpty().WithMessage("Идентификатор события обязателен.");
    }
}