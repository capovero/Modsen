using EventManagement.Application.DTO;
using EventManager.Application.DTO;
using FluentValidation;

namespace EventManagement.Application.Validators;

public class EventDtoValidator : AbstractValidator<EventDto>
{
    public EventDtoValidator()
    {
        RuleFor(e => e.Title)
            .NotEmpty().WithMessage("Название события обязательно.")
            .MaximumLength(200).WithMessage("Название не должно превышать 200 символов.");

        RuleFor(e => e.Description)
            .NotEmpty().WithMessage("Описание события обязательно.")
            .MaximumLength(2000).WithMessage("Описание не должно превышать 2000 символов.");

        RuleFor(e => e.Date)
            .GreaterThan(DateTime.UtcNow).WithMessage("Дата события должна быть в будущем.");

        RuleFor(e => e.Location)
            .NotEmpty().WithMessage("Место проведения обязательно.")
            .MaximumLength(300).WithMessage("Место проведения не должно превышать 300 символов.");

        RuleFor(e => e.Category)
            .NotEmpty().WithMessage("Категория события обязательна.")
            .MaximumLength(100).WithMessage("Категория не должна превышать 100 символов.");

        RuleFor(e => e.MaxParticipants)
            .GreaterThan(0).WithMessage("Максимальное количество участников должно быть больше 0.");
    }
}