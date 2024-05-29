using FluentValidation;
using Services.Services.Abstractions.Commands.Services;

namespace Services.Presentation.Validators;

public class CreateServiceCommandValidator : AbstractValidator<CreateServiceCommand>
{
    public CreateServiceCommandValidator()
    {
        RuleFor(x => x.NewService.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.NewService.Price)
            .NotNull()
            .GreaterThan(0);

        RuleFor(x => x.NewService.Status)
            .NotNull()
            .InclusiveBetween(0, 1);

        RuleFor(x => x.NewService.ServiceCategoryId)
            .NotNull();
    }
}
