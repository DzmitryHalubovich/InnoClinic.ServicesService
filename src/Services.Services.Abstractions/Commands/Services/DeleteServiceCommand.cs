using MediatR;
using OneOf;
using OneOf.Types;

namespace Services.Services.Abstractions.Commands.Services;

public record DeleteServiceCommand(Guid Id) : IRequest<OneOf<Success, NotFound>>
{ }
