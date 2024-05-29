using MediatR;
using OneOf.Types;
using OneOf;
using Services.Contracts.Specialization;
using Services.Services.Abstractions.Queries.Specializations;
using AutoMapper;
using Services.Domain.Interfaces;

namespace Services.Services.Handlers.Specializations;

public class GetSpecializationsQueryHandler : IRequestHandler<GetAllSpecializationsQuery,
    OneOf<List<SpecializationResponseDTO>, NotFound>>
{
    private readonly ISpecializationsRepository _specializationsRepository;
    private readonly IMapper _mapper;

    public GetSpecializationsQueryHandler(ISpecializationsRepository specializationsRepository, IMapper mapper)
    {
        _specializationsRepository = specializationsRepository;
        _mapper = mapper;
    }

    public async Task<OneOf<List<SpecializationResponseDTO>, NotFound>> Handle(GetAllSpecializationsQuery request,
        CancellationToken cancellationToken)
    {
        var specializationsList = await _specializationsRepository.
            GetAllAsync(request.QueryParameters);

        if (!specializationsList.Any())
        {
            return new NotFound();
        }

        var mappedSpecializationsList = _mapper.Map<List<SpecializationResponseDTO>>(specializationsList);

        return mappedSpecializationsList;
    }
}
