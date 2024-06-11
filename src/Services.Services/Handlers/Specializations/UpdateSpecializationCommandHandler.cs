using MediatR;
using OneOf.Types;
using OneOf;
using Services.Services.Abstractions.Commands.Specializations;
using AutoMapper;
using Services.Domain.Interfaces;
using Services.Services.Abstractions.Contracts;
using Services.Contracts.MQMessages;

namespace Services.Services.Handlers.Specializations;

public class UpdateSpecializationCommandHandler : IRequestHandler<UpdateSpecializationCommand, OneOf<Success, NotFound>>
{
    private readonly ISpecializationsRepository _specializationsRepository;
    private readonly IMapper _mapper;
    private readonly IMessageProducer _messageProducer;

    public UpdateSpecializationCommandHandler(ISpecializationsRepository specializationsRepository, IMapper mapper,
        IMessageProducer messageProducer)
    {
        _specializationsRepository = specializationsRepository;
        _mapper = mapper;
        _messageProducer = messageProducer;
    }

    public async Task<OneOf<Success, NotFound>> Handle(UpdateSpecializationCommand request, CancellationToken cancellationToken)
    {
        var specializationEntity = await _specializationsRepository.GetByIdAsync(request.Id);

        if (specializationEntity is null)
        {
            return new NotFound();
        }

        _mapper.Map(request.EditedSpecialization, specializationEntity);

        await _specializationsRepository.UpdateAsync(specializationEntity);

        _messageProducer.SendSpecializationUpdatedMessage(new SpecializationUpdatedMessage
        {
            Id = specializationEntity.Id,
            Name = specializationEntity.Name,
            Status = (int) specializationEntity.Status
        });

        return new Success();
    }
}
