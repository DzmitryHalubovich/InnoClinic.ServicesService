using FluentValidation;
using Services.Contracts.Specialization;
using Services.Services.Abstractions.Commands.Specializations;

namespace Services.Presentation.Validators;

public class UpdateSpecializationCommandValidator : AbstractValidator<UpdateSpecializationCommand>
{
    public UpdateSpecializationCommandValidator()
    {
        RuleFor(x => x.EditedSpecialization.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.EditedSpecialization.Status)
            .NotNull()
            .InclusiveBetween(0, 1);

        RuleFor(x => x)
            .Must(x => CheckServicesStatus(x.EditedSpecialization));
    }

    private bool CheckServicesStatus(SpecializationUpdateDTO specialization)
    {
        if (specialization.Status == 0)
        {
            return true;
        }

        return specialization.Services is null
            ? true
            : specialization.Services.All(service => service.Status == 1);
    }
}
