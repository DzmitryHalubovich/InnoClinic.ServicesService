using InnoClinic.SharedModels.MQMessages.Services;
using MassTransit;
using MediatR;
using OneOf;
using OneOf.Types;
using Services.Domain.Entities;
using Services.Domain.Interfaces;
using Services.Services.Abstractions.Commands.Services;

namespace Services.Services.Handlers.Services;

public class ChangeStatusServiceHandler : IRequestHandler<ChangeStatusServiceCommand, OneOf<Success, NotFound>>
{
    private readonly IServicesRepository _servicesRepository;
    private readonly IPublishEndpoint _messageProducer;

    public ChangeStatusServiceHandler(IServicesRepository servicesRepository, IPublishEndpoint messageProducer)
    {
        _servicesRepository = servicesRepository;
        _messageProducer = messageProducer;
    }

    public async Task<OneOf<Success, NotFound>> Handle(ChangeStatusServiceCommand request, CancellationToken cancellationToken)
    {
        var serviceEntity = await _servicesRepository.GetByIdAsync(request.Id);

        if (serviceEntity == null)
        {
            return new NotFound();
        }

        serviceEntity.Status = (Status) request.Status;

        await _servicesRepository.UpdateAsync(serviceEntity);

        if (serviceEntity.Status == Status.Inactive)
        {
            await _messageProducer.Publish<ServiceStatusChangedToInactiveMessage>(new()
            {
                ServiceId = serviceEntity.Id
            });
        }

        return new Success();
    }
}
