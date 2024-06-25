using MediatR;
using OneOf;
using OneOf.Types;
using Services.Contracts.Service;

namespace Services.Services.Abstractions.Queries.Services;

public record GetServiceByIdQuery(int Id) : IRequest<OneOf<ServiceResponseDTO, NotFound>>
{ }
