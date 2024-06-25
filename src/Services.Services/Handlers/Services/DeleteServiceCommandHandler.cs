using InnoClinic.SharedModels.MQMessages.Services;
using MediatR;
using OneOf;
using OneOf.Types;
using Services.Domain.Interfaces;
using Services.Services.Abstractions.Commands.Services;
using Services.Services.Abstractions.Contracts;

namespace Services.Services.Handlers.Services;

public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand, OneOf<Success, NotFound>>
{
    private readonly IServicesRepository _servicesRepository;
    private readonly IMessageProducer _messageProducer;

    public DeleteServiceCommandHandler(IServicesRepository servicesRepository, IMessageProducer messageProducer)
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

        _messageProducer.SendServiceDeletedMessage(new ServiceDeletedMessage()
        {
            ServiceId = serviceEntity.Id
        });

        return new Success();
    }
}
