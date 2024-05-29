using AutoMapper;
using MediatR;
using OneOf;
using OneOf.Types;
using Services.Contracts.Service;
using Services.Domain.Interfaces;
using Services.Services.Abstractions.Queries.Services;

namespace Services.Services.Handlers.Services;

public class GetServiceByIdQueryHandler : IRequestHandler<GetServiceByIdQuery, OneOf<ServiceResponseDTO, NotFound>>
{
    private readonly IServicesRepository _servicesRepository;
    private readonly IMapper _mapper;

    public GetServiceByIdQueryHandler(IServicesRepository servicesRepository, IMapper mapper)
    {
        _servicesRepository = servicesRepository;
        _mapper = mapper;
    }

    public async Task<OneOf<ServiceResponseDTO, NotFound>> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
    {
        var activeServicesList = await _servicesRepository.GetByIdAsync(request.Id);

        if (activeServicesList is null)
        {
            return new NotFound();
        }

        var mappedServicesList = _mapper.Map<ServiceResponseDTO>(activeServicesList);

        return mappedServicesList;
    }
}
