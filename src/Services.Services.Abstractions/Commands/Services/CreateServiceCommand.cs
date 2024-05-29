using MediatR;
using OneOf;
using OneOf.Types;
using Services.Contracts.Service;

namespace Services.Services.Abstractions.Commands.Services;

public record CreateServiceCommand(ServiceCreateDTO NewService) : IRequest<OneOf<ServiceResponseDTO, NotFound>>
{ }
