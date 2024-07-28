using MediatR;
using OneOf.Types;
using OneOf;
using Services.Services.Abstractions.Commands.Specializations;
using AutoMapper;
using Services.Domain.Interfaces;
using Services.Domain.Entities;
using InnoClinic.SharedModels.MQMessages.Specializations;
using MassTransit;

namespace Services.Services.Handlers.Specializations;

public class ChangeSpecializationStatusCommandHandler : IRequestHandler<ChangeSpecializationStatusCommand, OneOf<Success, NotFound>>
{
    private readonly ISpecializationsRepository _specializationsRepository;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _messageProducer;

    public ChangeSpecializationStatusCommandHandler(ISpecializationsRepository specializationsRepository, IMapper mapper, 
        IPublishEndpoint messageProducer)
    {
        _specializationsRepository = specializationsRepository;
        _mapper = mapper;
        _messageProducer = messageProducer;
    }

    public async Task<OneOf<Success, NotFound>> Handle(ChangeSpecializationStatusCommand request, CancellationToken cancellationToken)
    {
        var specializationEntity = await _specializationsRepository.GetByIdAsync(request.Id);

        var statusIsChangedToInactive = false;

        if (specializationEntity is null)
        {
            return new NotFound();
        }

        if (request.EditedSpecialization.Status.Equals((int)Status.Inactive))
        {
            statusIsChangedToInactive = true;

            specializationEntity.Status = (Status)request.EditedSpecialization.Status;
        }
        else
        {
            _mapper.Map(request.EditedSpecialization, specializationEntity);
        }

        await _specializationsRepository.UpdateStatusAsync(specializationEntity, statusIsChangedToInactive);

        await _messageProducer.Publish<SpecializationStatusChangedMessage>(new()
        {
            SpecializationId = specializationEntity.Id,
            Status = (int) specializationEntity.Status
        });

        return new Success();
    }
}
