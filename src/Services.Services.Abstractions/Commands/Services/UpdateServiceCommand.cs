using MediatR;
using OneOf;
using OneOf.Types;
using Services.Contracts.Service;

namespace Services.Services.Abstractions.Commands.Services;

public record UpdateServiceCommand(int Id, ServiceUpdateDTO EditedService) : IRequest<OneOf<Success, NotFound>>
{ }
