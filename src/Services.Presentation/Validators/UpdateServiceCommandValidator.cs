using FluentValidation;
using Services.Services.Abstractions.Commands.Services;

namespace Services.Presentation.Validators;

public class UpdateServiceCommandValidator : AbstractValidator<UpdateServiceCommand>
{
    public UpdateServiceCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotNull();

        RuleFor(x => x.EditedService.Name)
            .NotEmpty()
            .MaximumLength(150);

        RuleFor(x => x.EditedService.Price)
            .NotNull()
            .GreaterThan(0);

        RuleFor(x => x.EditedService.Status)
            .NotNull()
            .InclusiveBetween(0, 1);

        RuleFor(x => x.EditedService.ServiceCategoryId)
            .NotNull();
    }
}
