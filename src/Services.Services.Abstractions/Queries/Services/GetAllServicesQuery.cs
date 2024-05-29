using MediatR;
using OneOf;
using OneOf.Types;
using Services.Contracts.Service;

namespace Services.Services.Abstractions.Queries.Services;

public record GetAllServicesQuery() : IRequest<OneOf<List<ServiceResponseDTO>, NotFound>>
{ }
