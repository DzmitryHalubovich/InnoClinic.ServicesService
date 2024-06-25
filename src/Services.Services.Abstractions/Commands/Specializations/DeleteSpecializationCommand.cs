using MediatR;
using OneOf.Types;
using OneOf;

namespace Services.Services.Abstractions.Commands.Specializations;

public record DeleteSpecializationCommand(int Id) : IRequest<OneOf<Success, NotFound>>
{ }
