using AutoMapper;
using MediatR;
using Services.Contracts.Specialization;
using Services.Domain.Entities;
using Services.Domain.Interfaces;
using Services.Services.Abstractions.Commands.Specializations;

namespace Services.Services.Handlers.Specializations;

public class CreateSpecializationCommandHandler : IRequestHandler<CreateSpecializationCommand, SpecializationResponseDTO>
{
    private readonly ISpecializationsRepository _specializationsRepository;
    private readonly IMapper _mapper;

    public CreateSpecializationCommandHandler(ISpecializationsRepository specializationsRepository, IMapper mapper)
    {
        _specializationsRepository = specializationsRepository;
        _mapper = mapper;
    }

    public async Task<SpecializationResponseDTO> Handle(CreateSpecializationCommand request, CancellationToken cancellationToken)
    {
        var specialization = _mapper.Map<Specialization>(request.NewSpecialization);

        await _specializationsRepository.CreateAsync(specialization);

        var specializationResult = _mapper.Map<SpecializationResponseDTO>(specialization);

        return specializationResult;
    }
}
