using AutoMapper;
using MediatR;
using OneOf;
using OneOf.Types;
using Services.Contracts.Service;
using Services.Domain.Entities;
using Services.Domain.Interfaces;
using Services.Services.Abstractions.Commands.Services;

namespace Services.Services.Handlers.Services;

public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, OneOf<ServiceResponseDTO, NotFound>>
{
    private readonly IServicesRepository _servicesRepository;
    private readonly ISpecializationsRepository _specializationsRepository;
    private readonly IServiceCategoryRepository _serviceCategoryRepository;
    private readonly IMapper _mapper;

    public CreateServiceCommandHandler(IServicesRepository servicesRepository, ISpecializationsRepository specializationsRepository, 
        IServiceCategoryRepository serviceCategoryRepository, IMapper mapper)
    {
        _servicesRepository = servicesRepository;
        _specializationsRepository = specializationsRepository;
        _serviceCategoryRepository = serviceCategoryRepository;
        _mapper = mapper;
    }

    public async Task<OneOf<ServiceResponseDTO, NotFound>> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        var specialization = await _specializationsRepository.GetByIdAsync(request.NewService.SpecializationId);

        if (specialization is null)
        {
            return new NotFound();
        }

        var serviceCategory = await _serviceCategoryRepository.GetByIdAsync(request.NewService.ServiceCategoryId);

        if (serviceCategory is null)
        {
            return new NotFound();
        }

        var service = _mapper.Map<Service>(request.NewService);

        await _servicesRepository.CreateAsync(service);

        var serviceResponse = _mapper.Map<ServiceResponseDTO>(service);

        serviceResponse.ServiceCategory = _mapper.Map<ServiceCategoryDTO>(serviceCategory);

        return serviceResponse;
    }
}
