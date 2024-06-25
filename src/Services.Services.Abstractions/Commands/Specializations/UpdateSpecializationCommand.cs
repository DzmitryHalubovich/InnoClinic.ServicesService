using MediatR;
using OneOf.Types;
using OneOf;
using Services.Contracts.Specialization;

namespace Services.Services.Abstractions.Commands.Specializations
{
    public record UpdateSpecializationCommand(int Id, SpecializationUpdateDTO EditedSpecialization) : 
        IRequest<OneOf<Success, NotFound>>
    { }
}
