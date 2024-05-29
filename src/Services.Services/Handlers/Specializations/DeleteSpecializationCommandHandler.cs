using MediatR;
using OneOf.Types;
using OneOf;
using Services.Services.Abstractions.Commands.Specializations;
using Services.Domain.Interfaces;

namespace Services.Services.Handlers.Specializations;

public class DeleteSpecializationCommandHandler : IRequestHandler<DeleteSpecializationCommand, OneOf<Success, NotFound>>
{
    private readonly ISpecializationsRepository _specializationsRepository;

    public DeleteSpecializationCommandHandler(ISpecializationsRepository specializationsRepository)
    {
        _specializationsRepository = specializationsRepository;
    }

    public async Task<OneOf<Success, NotFound>> Handle(DeleteSpecializationCommand request, CancellationToken cancellationToken)
    {
        var specializationEntity = await _specializationsRepository.GetByIdAsync(request.Id);

        if (specializationEntity is null)
        {
            return new NotFound();
        }

        await _specializationsRepository.DeleteAsync(specializationEntity);

        return new Success();
    }
}
