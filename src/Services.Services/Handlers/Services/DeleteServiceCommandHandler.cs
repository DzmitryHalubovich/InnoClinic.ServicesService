using MediatR;
using OneOf;
using OneOf.Types;
using Services.Domain.Interfaces;
using Services.Services.Abstractions.Commands.Services;

namespace Services.Services.Handlers.Services;

public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand, OneOf<Success, NotFound>>
{
    private readonly IServicesRepository _servicesRepository;

    public DeleteServiceCommandHandler(IServicesRepository servicesRepository)
    {
        _servicesRepository = servicesRepository;
    }

    public async Task<OneOf<Success, NotFound>> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        var serviceEntity = await _servicesRepository.GetByIdAsync(request.Id);

        if (serviceEntity is null)
        {
            return new NotFound();
        }

        await _servicesRepository.DeleteAsync(serviceEntity);

        return new Success();
    }
}
