using FluentValidation;
using Services.Contracts.Specialization;
using Services.Services.Abstractions.Commands.Specializations;

namespace Services.Presentation.Validators;

public class CreateSpecializationCommandValidator : AbstractValidator<CreateSpecializationCommand>
{
    public CreateSpecializationCommandValidator()
    {
        RuleFor(x => x.NewSpecialization.Name)
            .NotEmpty()
            .MaximumLength(100);

        RuleFor(x => x.NewSpecialization.Status)
            .NotNull()
            .InclusiveBetween(0, 1);

        RuleFor(x => x)
            .Must(x => CheckServicesStatus(x.NewSpecialization))
            .WithMessage("Cannot add active service for inactive specialization");
    }

    private bool CheckServicesStatus(SpecializationCreateDTO specialization)
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
