﻿using InnoClinic.SharedModels.MQMessages.Services;
using MediatR;
using OneOf;
using OneOf.Types;
using Services.Domain.Entities;
using Services.Domain.Interfaces;
using Services.Services.Abstractions.Commands.Services;
using Services.Services.Abstractions.Contracts;

namespace Services.Services.Handlers.Services;

public class ChangeStatusServiceHandler : IRequestHandler<ChangeStatusServiceCommand, OneOf<Success, NotFound>>
{
    private readonly IServicesRepository _servicesRepository;
    private readonly IMessageProducer _messageProducer;

    public ChangeStatusServiceHandler(IServicesRepository servicesRepository, IMessageProducer messageProducer)
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
            _messageProducer.SendServiceStatusChangedToInactiveMessage(new ServiceStatusChangedToInactiveMessage()
            {
                ServiceId = serviceEntity.Id
            });
        }

        return new Success();
    }
}
