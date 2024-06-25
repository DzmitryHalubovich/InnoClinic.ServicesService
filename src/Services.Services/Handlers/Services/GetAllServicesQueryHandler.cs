using AutoMapper;
using MediatR;
using OneOf;
using OneOf.Types;
using Services.Contracts.Service;
using Services.Domain.Interfaces;
using Services.Services.Abstractions.Queries.Services;

namespace Services.Services.Handlers.Services;

public class GetAllServicesQueryHandler : IRequestHandler<GetAllServicesQuery, OneOf<List<ServiceResponseDTO>, NotFound>>
{
    private readonly IServicesRepository _servicesRepository;
    private readonly IMapper _mapper;

    public GetAllServicesQueryHandler(IServicesRepository servicesRepository, IMapper mapper)
    {
        _servicesRepository = servicesRepository;
        _mapper = mapper;
    }

    async Task<OneOf<List<ServiceResponseDTO>, NotFound>> IRequestHandler<GetAllServicesQuery, OneOf<List<ServiceResponseDTO>, NotFound>>
        .Handle(GetAllServicesQuery request, CancellationToken cancellationToken)
    {
        var services = await _servicesRepository.GetAllAsync();

        if (!services.Any())
        {
            return new NotFound();
        }

        var mappedServices = _mapper.Map<List<ServiceResponseDTO>>(services);

        return mappedServices;
    }
}
