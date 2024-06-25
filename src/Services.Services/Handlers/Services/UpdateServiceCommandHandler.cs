using AutoMapper;
using MediatR;
using OneOf;
using OneOf.Types;
using Services.Domain.Interfaces;
using Services.Services.Abstractions.Commands.Services;

namespace Services.Services.Handlers.Services;

public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand, OneOf<Success, NotFound>>
{
    private readonly IServicesRepository _servicesRepository;
    private readonly IMapper _mapper;

    public UpdateServiceCommandHandler(IServicesRepository servicesRepository, IMapper mapper)
    {
        _servicesRepository = servicesRepository;
        _mapper = mapper;
    }

    public async Task<OneOf<Success, NotFound>> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        var serviceEntity = await _servicesRepository.GetByIdAsync(request.Id);

        if (serviceEntity is null)
        {
            return new NotFound();
        }

        _mapper.Map(request.EditedService, serviceEntity);

        await _servicesRepository.UpdateAsync(serviceEntity);

        return new Success();
    }
}
