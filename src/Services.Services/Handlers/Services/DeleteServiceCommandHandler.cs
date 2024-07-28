using InnoClinic.SharedModels.MQMessages.Services;
using MassTransit;
using MediatR;
using OneOf;
using OneOf.Types;
using Services.Domain.Interfaces;
using Services.Services.Abstractions.Commands.Services;

namespace Services.Services.Handlers.Services;

public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand, OneOf<Success, NotFound>>
{
    private readonly IServicesRepository _servicesRepository;
    private readonly IPublishEndpoint _messageProducer;

    public DeleteServiceCommandHandler(IServicesRepository servicesRepository, 
        IPublishEndpoint messageProducer)
    {
        _servicesRepository = servicesRepository;
        _messageProducer = messageProducer;
    }

    public async Task<OneOf<Success, NotFound>> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        var serviceEntity = await _servicesRepository.GetByIdAsync(request.Id);

        if (serviceEntity is null)
        {
            return new NotFound();
        }

        await _servicesRepository.DeleteAsync(serviceEntity);

        await _messageProducer.Publish(new ServiceDeletedMessage()
        {
            ServiceId = serviceEntity.Id
        });

        return new Success();
    }
}
