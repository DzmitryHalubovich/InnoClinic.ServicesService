using MediatR;
using OneOf.Types;
using OneOf;
using Services.Services.Abstractions.Commands.Specializations;
using AutoMapper;
using Services.Domain.Interfaces;
using InnoClinic.SharedModels.MQMessages.Specializations;
using Services.Domain.Entities;
using MassTransit;

namespace Services.Services.Handlers.Specializations;

public class UpdateSpecializationCommandHandler : IRequestHandler<UpdateSpecializationCommand, OneOf<Success, NotFound>>
{
    private readonly ISpecializationsRepository _specializationsRepository;
    private readonly IMapper _mapper;
    private readonly IPublishEndpoint _messagePublisher;

    public UpdateSpecializationCommandHandler(ISpecializationsRepository specializationsRepository, IMapper mapper,
        IPublishEndpoint messagePublisher)
    {
        _specializationsRepository = specializationsRepository;
        _mapper = mapper;
    }

    public async Task<OneOf<Success, NotFound>> Handle(UpdateSpecializationCommand request, CancellationToken cancellationToken)
    {
        var specializationEntity = await _specializationsRepository.GetByIdAsync(request.Id);

        if (specializationEntity is null)
        {
            return new NotFound();
        }

        if (request.EditedSpecialization.Status.Equals((int)Status.Inactive))
        {
            var services = specializationEntity.Services;

            foreach (var service in services)
            {
                service.Status = Status.Inactive;
            }
        }

        _mapper.Map(request.EditedSpecialization, specializationEntity);

        if (request.EditedSpecialization.Services != null && request.EditedSpecialization.Services.Any())
        {
            foreach (var service in request.EditedSpecialization.Services)
            {
                var newSerivce = _mapper.Map(service, new Service());

                specializationEntity.Services.Add(newSerivce);
            }
        }

        await _specializationsRepository.UpdateAsync(specializationEntity);

        await _messagePublisher.Publish<SpecializationUpdatedMessage>(new()
        {
            SpecializationId = specializationEntity.Id,
            Name = specializationEntity.Name,
            Status = (int) specializationEntity.Status
        });

        return new Success();
    }
}
