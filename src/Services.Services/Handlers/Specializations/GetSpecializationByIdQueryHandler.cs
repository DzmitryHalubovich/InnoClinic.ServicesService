using MediatR;
using OneOf.Types;
using OneOf;
using Services.Contracts.Specialization;
using Services.Services.Abstractions.Queries.Specializations;
using AutoMapper;
using Services.Domain.Interfaces;

namespace Services.Services.Handlers.Specializations;

public class GetSpecializationByIdQueryHandler : IRequestHandler<GetSpecializationByIdQuery, OneOf<SpecializationResponseDTO, NotFound>>
{
    private readonly ISpecializationsRepository _specializationsRepository;
    private readonly IMapper _mapper;

    public GetSpecializationByIdQueryHandler(ISpecializationsRepository specializationsRepository, IMapper mapper)
    {
        _specializationsRepository = specializationsRepository;
        _mapper = mapper;
    }

    public async Task<OneOf<SpecializationResponseDTO, NotFound>> Handle(GetSpecializationByIdQuery request, 
        CancellationToken cancellationToken)
    {
        var specializationEntity = await _specializationsRepository.GetByIdAsync(request.Id);

        if (specializationEntity is null)
        {
            return new NotFound();
        }

        var mappedSpecialization = _mapper.Map<SpecializationResponseDTO>(specializationEntity);

        return mappedSpecialization;
    }
}
