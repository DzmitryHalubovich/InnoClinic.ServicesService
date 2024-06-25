using MediatR;
using OneOf;
using OneOf.Types;

namespace Services.Services.Abstractions.Commands.Services;

public record DeleteServiceCommand(int Id) : IRequest<OneOf<Success, NotFound>>
{ }
